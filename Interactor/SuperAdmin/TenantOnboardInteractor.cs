﻿using Amazon.RDS.Model;
using Amazon.RDS;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using UseCase.SuperAdmin.TenantOnboard;
using Domain.SuperAdminModels.MigrationTenantHistory;
using Interactor.Realtime;
using Domain.SuperAdminModels.Notification;
using Npgsql;

namespace Interactor.SuperAdmin
{
    public class TenantOnboardInteractor : ITenantOnboardInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenant2Repository;
        private readonly IMigrationTenantHistoryRepository _migrationTenantHistoryRepository;
        private readonly IWebSocketService _iWebSocketService;
        private readonly INotificationRepository _notificationRepository;
        public TenantOnboardInteractor(
            IAwsSdkService awsSdkService,
            ITenantRepository tenantRepository,
            ITenantRepository tenant2Repository,
            IMigrationTenantHistoryRepository migrationTenantHistoryRepository,
            IWebSocketService iWebSocketService,
            INotificationRepository notificationRepository
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _migrationTenantHistoryRepository = migrationTenantHistoryRepository;
            _iWebSocketService = iWebSocketService;
            _notificationRepository = notificationRepository;
            _tenant2Repository = tenant2Repository;
        }
        public TenantOnboardOutputData Handle(TenantOnboardInputData inputData)
        {
            try
            {
                var dbName = CommonConstants.RemoveSpecialCharacters(inputData.SubDomain);
                if (inputData.Size <= 0)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSize);
                }
                else if (inputData.SizeType != ConfigConstant.SizeTypeMB && inputData.SizeType != ConfigConstant.SizeTypeGB)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSizeType);
                }
                else if (inputData.ClusterMode != ConfigConstant.TypeSharing && inputData.ClusterMode != ConfigConstant.TypeDedicate)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidClusterMode);
                }
                else if (string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(inputData.Hospital) || string.IsNullOrEmpty(inputData.Password) || inputData.AdminId <= 0)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.Failed);
                }
                var checkSubDomain = _awsSdkService.CheckSubdomainExistenceAsync(inputData.SubDomain).Result;
                if (checkSubDomain)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.SubDomainExists);
                }
                var tenantModel = new TenantModel(inputData.Hospital, 0, inputData.AdminId, inputData.Password, inputData.SubDomain, dbName, inputData.Size, inputData.SizeType, inputData.ClusterMode, string.Empty, string.Empty, 0, string.Empty, inputData.SubDomain, CommonConstants.GenerateRandomPassword());
                var tenantOnboard = TenantOnboardAsync(tenantModel).Result;
                var message = string.Empty;
                if (tenantOnboard.TryGetValue("Error", out string? errorValue))
                {
                    Console.WriteLine($"Exception: {errorValue}");
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.Failed);
                }
                if (tenantOnboard.TryGetValue("message", out string? messageValue))
                {
                    message = messageValue;
                }
                var data = new TenantOnboardItem(message);

                return new TenantOnboardOutputData(data, TenantOnboardStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        #region private Function        

        private async Task<Dictionary<string, string>> TenantOnboardAsync(TenantModel model)
        {
            string subDomain = model.SubDomain;
            string dbName = model.Db;
            int size = model.Size;
            int sizeType = model.SizeType;
            int tier = model.Type;
            string rString = CommonConstants.GenerateRandomString(6);
            string tenantUrl = "";

            try
            {
                // Provisioning SubDomain for new tenants
                tenantUrl = $"{subDomain}.{ConfigConstant.Domain}";
                // Checking Available RDS Cluster
                if (subDomain.Length > 0)
                {
                    // Checking tenant tier, if dedicated, provision new RDS instance
                    if (tier == ConfigConstant.TypeDedicate)
                    {
                        string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                        var rdsInfo = await RDSAction.GetRDSInformation();
                        if (rdsInfo.ContainsKey(dbIdentifier))
                        {
                            var id = _tenantRepository.CreateTenant(model);
                            model.ChangeRdsIdentifier(dbIdentifier);
                            _ = Task.Run(async () =>
                            {
                                await AddData(id, tenantUrl, dbName, model, dbIdentifier);
                            });
                        }
                        else
                        {
                            var id = _tenantRepository.CreateTenant(model);
                            await RDSAction.CreateNewShardAsync(dbIdentifier);
                            model.ChangeRdsIdentifier(dbIdentifier);
                            _ = Task.Run(async () =>
                            {
                                await AddData(id, tenantUrl, dbName, model, dbIdentifier);
                            });
                        }
                    }
                    else // In the rest cases, checking available RDS for new Tenant
                    {
                        var card = await CloudWatchAction.GetSummaryCardAsync();
                        List<string> availableIdentifier = card.Keys.Where(ids => card[ids]["available"] == "yes").ToList();

                        // If there's not any available RDS Cluster, provision new RDS cluster
                        if (availableIdentifier.Count == 0)
                        {
                            string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                            var id = _tenantRepository.CreateTenant(model);
                            await RDSAction.CreateNewShardAsync(dbIdentifier);
                            model.ChangeRdsIdentifier(dbIdentifier);
                            _ = Task.Run(async () =>
                            {
                                await AddData(id, tenantUrl, dbName, model, dbIdentifier);
                            });
                        }
                        else // Else, returning the first available RDS Cluster in the list
                        {
                            bool checkAvailableIdentifier = false;
                            foreach (var dbIdentifier in availableIdentifier)
                            {
                                var sumSubDomainToDbIdentifier = _tenantRepository.SumSubDomainToDbIdentifier(dbIdentifier);
                                if (sumSubDomainToDbIdentifier <= 3)
                                {
                                    checkAvailableIdentifier = true;
                                    model.ChangeRdsIdentifier(dbIdentifier);
                                    var id = _tenantRepository.CreateTenant(model);
                                    _ = Task.Run(async () =>
                                    {
                                        await AddData(id, tenantUrl, dbName, model, dbIdentifier);
                                    });
                                    break;
                                }
                            }
                            if (!checkAvailableIdentifier)
                            {
                                string dbIdentifierNew = $"develop-smartkarte-postgres-{rString}";
                                var id = _tenantRepository.CreateTenant(model);
                                await RDSAction.CreateNewShardAsync(dbIdentifierNew);
                                model.ChangeRdsIdentifier(dbIdentifierNew);
                                _ = Task.Run(async () =>
                                {
                                    await AddData(id, tenantUrl, dbName, model, dbIdentifierNew);
                                });
                            }
                        }
                    }
                }
                await Route53Action.CreateTenantDomain(subDomain);
                await CloudFrontAction.UpdateNewTenantAsync(subDomain);

                // Return message for Super Admin
                Dictionary<string, string> result = new Dictionary<string, string>
                {
                    { "message", "Please wait for 15 minutes for all resources to be available" }
                };
                var message = $"{subDomain} is created successfuly.";
                var saveDBNotify = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, message);
                await _iWebSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, saveDBNotify);
                return result;
            }
            catch (Exception ex)
            {
                var message = $"{subDomain} is created failed. Error: {ex.Message}";
                var saveDBNotify = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                await _iWebSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, saveDBNotify);
                return new Dictionary<string, string> { { "Error", ex.Message } };
            }
        }

        private async Task<string> CheckingRDSStatusAsync(string dbIdentifier, int tenantId, string tenantUrl)
        {
            try
            {
                string host = string.Empty;
                bool running = true;
                string status = string.Empty;
                DateTime startTime = DateTime.Now;

                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    var response = await rdsClient.DescribeDBInstancesAsync(new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbIdentifier
                    });

                    var dbInstances = response.DBInstances;

                    if (dbInstances.Count != 1)
                    {
                        throw new Exception("More than one Database Instance returned; this should never happen");
                    }

                    var dbInstance = dbInstances[0];
                    var checkStatus = dbInstance.DBInstanceStatus;
                    if (status != checkStatus)
                    {
                        status = checkStatus;
                        var rdsStatusDictionary = ConfigConstant.StatusTenantDictionary();
                        if (rdsStatusDictionary.TryGetValue(checkStatus, out byte statusTenant))
                        {
                            var updateStatus = _tenant2Repository.UpdateInfTenant(tenantId, statusTenant, string.Empty, string.Empty, dbIdentifier);
                        }
                    }

                    Console.WriteLine($"Last Database Shard status: {checkStatus}");
                    Thread.Sleep(5000);

                    if (checkStatus == "available")
                    {
                        var endpoint = dbInstance.Endpoint;
                        host = endpoint.Address;
                        // update status available: 1
                        var updateStatus = _tenant2Repository.UpdateInfTenant(tenantId, 1, tenantUrl, host, dbIdentifier);
                        running = false;
                    }
                    // Check if more than timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        running = false;
                        throw new Exception($"CheckingRDSStatus. Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                    }
                }

                return host;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"CheckingRDSStatus. {ex.Message}");
            }
        }

        private async Task AddData(int tenantId, string tenantUrl, string dbName, TenantModel model, string dbIdentifier)
        {
            try
            {
                string host = await CheckingRDSStatusAsync(dbIdentifier, tenantId, tenantUrl);
                if (!string.IsNullOrEmpty(host))
                {
                    var dataMigration = _migrationTenantHistoryRepository.GetMigration(tenantId);
                    RDSAction.CreateDatabase(host, dbName, model.PasswordConnect);
                    CreateDatas(host, dbName, dataMigration, tenantId);
                }
            }
            catch (Exception ex)
            {
                var message = $"{tenantUrl} is created failed. Error: {ex.Message}";
                var saveDBNotify = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                await _iWebSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, saveDBNotify);
            }
            finally
            {
                _tenant2Repository.ReleaseResource();
                _migrationTenantHistoryRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        private void CreateDatas(string host, string dbName, List<string> listMigration, int tenantId)
        {
            try
            {
                var connectionString = $"Host={host};Database={dbName};Username=postgres;Password=Emr!23456789;Port=5432";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        _CreateTable(command, listMigration, tenantId);
                        command.CommandText = $"GRANT All ON ALL TABLES IN SCHEMA public TO {dbName};";
                        command.ExecuteNonQuery();
                        _CreateFunction(command, listMigration, tenantId);
                        _CreateTrigger(command, listMigration, tenantId);
                        _CreateDataMaster(command, listMigration, tenantId);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create data: {ex.Message}");
                throw new Exception($"Error create Data.  {ex.Message}");
            }
        }
        private void _CreateTable(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                var folderPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "Table");
                if (Directory.Exists(folderPath))
                {
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");

                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();

                        // insert table
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts trigger executed successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create table: {ex.Message}");
                throw new Exception($"Error create table.  {ex.Message}");
            }
        }

        private void _CreateDataMaster(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                var folderPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "DataMaster");
                if (Directory.Exists(folderPath))
                {
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");

                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();

                        // insert data master
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts trigger executed successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create data master: {ex.Message}");
                throw new Exception($"Error create data master.  {ex.Message}");
            }
        }

        private void _CreateFunction(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                var folderPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "Function");
                if (Directory.Exists(folderPath))
                {
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");
                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();
                        // insert function
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts trigger executed successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create function: {ex.Message}");
                throw new Exception($"Error create function.  {ex.Message}");
            }
        }

        private void _CreateTrigger(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                var folderFunctionPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "Trigger");
                if (Directory.Exists(folderFunctionPath))
                {
                    var sqlFiles = Directory.GetFiles(folderFunctionPath, "*.sql");
                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();
                        // insert trigger
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderFunctionPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts trigger executed successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create trigger: {ex.Message}");
                throw new Exception($"Error create trigger.  {ex.Message}");
            }
        }

        #endregion
    }
}
