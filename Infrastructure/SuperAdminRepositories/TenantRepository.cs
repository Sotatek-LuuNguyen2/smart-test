﻿using Amazon.Runtime.Internal.Util;
using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Helper.Common;
using Helper.Enum;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;
using System.Text;

namespace Infrastructure.SuperAdminRepositories
{
    public class TenantRepository : SuperAdminRepositoryBase, ITenantRepository
    {
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;

        public TenantRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        public TenantModel Get(int tenantId)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.TenantId == tenantId && t.IsDeleted == 0).FirstOrDefault();
            var tenantModel = tenant == null ? new() : ConvertEntityToModel(tenant);
            return tenantModel;
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public int GetBySubDomainAndIdentifier(string subDomain, string Identifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain == subDomain && t.RdsIdentifier == Identifier && t.IsDeleted == 0).FirstOrDefault();
            return tenant == null ? 0 : tenant.TenantId;
        }

        public int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain == subDomain && t.RdsIdentifier == dbIdentifier && t.IsDeleted == 0);
            if (tenant != null)
            {
                return tenant.Count();
            }
            return 0;
        }

        public int CreateTenant(TenantModel model)
        {
            var tenant = new Tenant();
            tenant.Hospital = model.Hospital;
            tenant.AdminId = model.AdminId;
            tenant.Password = model.Password;
            tenant.SubDomain = model.SubDomain;

            tenant.Status = 2; //Status: creating
            tenant.Db = model.Db;
            tenant.Size = model.Size;
            tenant.SizeType = model.SizeType;
            tenant.Type = model.Type;
            tenant.EndPointDb = model.SubDomain;
            tenant.EndSubDomain = model.SubDomain;
            tenant.RdsIdentifier = model.RdsIdentifier;
            tenant.IsDeleted = 0;
            tenant.CreateDate = CIUtil.GetJapanDateTimeNow();
            tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
            TrackingDataContext.Tenants.Add(tenant);
            TrackingDataContext.SaveChanges();
            return tenant.TenantId;
        }

        public bool UpdateInfTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier)
        {
            var tenant = TrackingDataContext.Tenants.FirstOrDefault(i => i.TenantId == tenantId);
            if (tenant != null)
            {
                if (!string.IsNullOrEmpty(endPointDb))
                {
                    tenant.EndPointDb = endPointDb;
                }
                if (!string.IsNullOrEmpty(endSubDomain))
                {
                    tenant.SubDomain = endSubDomain;
                }
                if (!string.IsNullOrEmpty(dbIdentifier))
                {
                    tenant.RdsIdentifier = dbIdentifier;
                }
                tenant.Status = status;
                tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
                tenant.CreateDate = TimeZoneInfo.ConvertTimeToUtc(tenant.CreateDate);
                TrackingDataContext.Tenants.Update(tenant);
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public TenantModel UpgradePremium(int tenantId, string dbIdentifier, string endPoint)
        {
            try
            {
                var tenant = TrackingDataContext.Tenants.FirstOrDefault(x => x.TenantId == tenantId && x.IsDeleted == 0);
                if (tenant == null)
                {
                    return new();
                }
                tenant.EndPointDb = endPoint;
                tenant.Type = 1;
                tenant.Status = 1;
                tenant.RdsIdentifier = dbIdentifier;
                TrackingDataContext.SaveChanges();
                var tenantModel = ConvertEntityToModel(tenant);
                return tenantModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new();
            }
        }

        public bool UpdateStatusTenant(int tenantId, byte status)
        {
            try
            {
                var tenant = TrackingDataContext.Tenants.FirstOrDefault(x => x.TenantId == tenantId && x.IsDeleted == 0);
                if (tenant == null)
                {
                    return false;
                }
                tenant.Status = status;
                TrackingDataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<TenantModel> GetTenantList(SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take)
        {
            List<TenantModel> result;
            IQueryable<Tenant> query = NoTrackingDataContext.Tenants;
            if (!searchModel.IsEmptyModel)
            {
                // filte data ignore storageFull
                query = FilterData(query, searchModel);
            }

            // sort data ignore storageFull
            if (searchModel.StorageFull == StorageFullEnum.Empty && !sortDictionary.ContainsKey(TenantEnum.StorageFull))
            {
                var querySortList = SortTenantQuery(query, sortDictionary);
                querySortList = (IOrderedQueryable<Tenant>)querySortList.Skip(skip).Take(take);
                result = querySortList.Select(tenant => new TenantModel(
                                                            tenant.TenantId,
                                                            tenant.Hospital,
                                                            tenant.Status,
                                                            tenant.AdminId,
                                                            tenant.Password,
                                                            tenant.SubDomain,
                                                            tenant.Db,
                                                            tenant.Size,
                                                            tenant.SizeType,
                                                            tenant.Type,
                                                            tenant.EndPointDb,
                                                            tenant.EndSubDomain,
                                                            tenant.Action,
                                                            tenant.ScheduleDate,
                                                            tenant.ScheduleTime,
                                                            tenant.CreateDate,
                                                            tenant.RdsIdentifier))
                                      .ToList();
                result = ChangeStorageFull(result);
                result = SortTenantList(result, sortDictionary).ToList();
                return result;
            }
            result = query.Select(tenant => new TenantModel(
                                            tenant.TenantId,
                                            tenant.Hospital,
                                            tenant.Status,
                                            tenant.AdminId,
                                            tenant.Password,
                                            tenant.SubDomain,
                                            tenant.Db,
                                            tenant.Size,
                                            tenant.SizeType,
                                            tenant.Type,
                                            tenant.EndPointDb,
                                            tenant.EndSubDomain,
                                            tenant.Action,
                                            tenant.ScheduleDate,
                                            tenant.ScheduleTime,
                                            tenant.CreateDate,
                                            tenant.RdsIdentifier))
                          .ToList();
            result = ChangeStorageFull(result);
            if (searchModel.StorageFull != StorageFullEnum.Empty)
            {
                switch (searchModel.StorageFull)
                {
                    case StorageFullEnum.Under70Percent:
                        result = result.Where(item => item.StorageFull <= 70).ToList();
                        break;
                    case StorageFullEnum.Over70Percent:
                        result = result.Where(item => item.StorageFull >= 70).ToList();
                        break;
                    case StorageFullEnum.Over80Percent:
                        result = result.Where(item => item.StorageFull >= 80).ToList();
                        break;
                    case StorageFullEnum.Over90Percent:
                        result = result.Where(item => item.StorageFull >= 90).ToList();
                        break;
                }
            }
            result = SortTenantList(result, sortDictionary).Skip(skip).Take(take).ToList();
            return result;
        }

        #region private function
        private IQueryable<Tenant> FilterData(IQueryable<Tenant> query, SearchTenantModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.KeyWord))
            {
                int tenantIdQuery = searchModel.KeyWord.AsInteger();
                query = query.Where(item => (tenantIdQuery > 0 && item.TenantId == tenantIdQuery)
                                            || item.SubDomain.Contains(searchModel.KeyWord)
                                            || (tenantIdQuery > 0 && item.AdminId == tenantIdQuery)
                                            || item.Hospital.Contains(searchModel.KeyWord));
            }
            if (searchModel.FromDate != null)
            {
                query = query.Where(item => item.CreateDate >= searchModel.FromDate);
            }
            if (searchModel.ToDate != null)
            {
                query = query.Where(item => item.CreateDate <= searchModel.ToDate);
            }
            if (searchModel.Type != 0)
            {
                query = query.Where(item => item.Type == searchModel.Type);
            }
            if (searchModel.Status != 0)
            {
                query = query.Where(item => item.Status == searchModel.Status);
            }
            return query;
        }

        private IOrderedQueryable<Tenant> SortTenantQuery(IQueryable<Tenant> query, Dictionary<TenantEnum, int> sortDictionary)
        {
            bool firstSort = true;
            IOrderedQueryable<Tenant> querySortList = query.OrderByDescending(item => item.TenantId);
            foreach (var sortItem in sortDictionary)
            {
                switch (sortItem.Value)
                {
                    // DESC
                    case 1:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Size);
                                break;
                            case TenantEnum.Status:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Status);
                                break;
                        }
                        break;
                    // ASC
                    default:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Size);
                                break;
                            case TenantEnum.Status:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Status);
                                break;
                        }
                        break;
                }
                firstSort = false;
            }
            return querySortList;
        }

        private IOrderedEnumerable<TenantModel> SortTenantList(List<TenantModel> tenantList, Dictionary<TenantEnum, int> sortDictionary)
        {
            bool firstSort = true;
            IOrderedEnumerable<TenantModel> querySortList = tenantList.OrderByDescending(item => item.TenantId);
            foreach (var sortItem in sortDictionary)
            {
                switch (sortItem.Value)
                {
                    // DESC
                    case 1:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Size);
                                break;
                            case TenantEnum.Status:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Status);
                                break;
                            case TenantEnum.StorageFull:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.StorageFull);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.StorageFull);
                                break;
                        }
                        break;
                    // ASC
                    default:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Size);
                                break;
                            case TenantEnum.Status:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Status);
                                break;
                            case TenantEnum.StorageFull:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.StorageFull);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.StorageFull);
                                break;
                        }
                        break;
                }
                firstSort = false;
            }
            return querySortList;
        }

        private List<TenantModel> ChangeStorageFull(List<TenantModel> tenantList)
        {
            Parallel.ForEach(tenantList, tenant =>
            {
                double storageInDB = 0;
                int port = 5432;
                string id = "postgres";
                string password = "Emr!23456789";
                StringBuilder connectionStringBuilder = new();
                connectionStringBuilder.Append("host=");
                connectionStringBuilder.Append(tenant.EndPointDb);
                connectionStringBuilder.Append(";port=");
                connectionStringBuilder.Append(port.ToString());
                connectionStringBuilder.Append(";database=");
                connectionStringBuilder.Append(tenant.Db);
                connectionStringBuilder.Append(";user id=");
                connectionStringBuilder.Append(id);
                connectionStringBuilder.Append(";password=");
                connectionStringBuilder.Append(password);
                string connectionString = connectionStringBuilder.ToString();
                string finalKey = string.Format("{0}_{1}_{2}", connectionString, tenant.Size.ToString(), tenant.SizeType);
                if (_cache.KeyExists(finalKey))
                {
                    var storageFull = _cache.StringGet(finalKey).AsInteger();
                    tenant.ChangeStorageFull(storageFull);
                }
                else
                {
                    var connStr = new NpgsqlConnectionStringBuilder(connectionString);
                    connStr.TrustServerCertificate = true;
                    try
                    {
                        using (var conn = new NpgsqlConnection(connStr.ToString()))
                        {
                            conn.Open();
                            string sqlQuery = string.Format("select pg_database_size('{0}')", tenant.Db);
                            using (var command = new NpgsqlCommand(sqlQuery, conn))
                            {
                                NpgsqlDataReader reader = command.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    /// 1: MB; 2: GB
                                    switch (tenant.SizeType)
                                    {
                                        case 1:
                                            storageInDB = (reader.GetInt64(0) / 1024 / 1024);
                                            break;
                                        case 2:
                                            storageInDB = (reader.GetInt64(0) / 1024 / 1024 / 1024);
                                            break;
                                    }
                                }
                                reader.Close();
                            }
                        }
                        var storageFull = Math.Round((storageInDB / tenant.Size) * 100);
                        if (storageFull > 0)
                        {
                            _cache.StringSet(finalKey, storageFull.ToString());
                            _cache.KeyExpire(finalKey, new TimeSpan(1, 0, 0));
                        }
                        tenant.ChangeStorageFull(storageFull);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Can not connect to database " + tenant.EndPointDb + tenant.Db + "\n" + ex.ToString());
                    }
                }
            });
            return tenantList;
        }

        private TenantModel ConvertEntityToModel(Tenant tenant)
        {
            return new TenantModel(
                       tenant.TenantId,
                       tenant.Hospital,
                       tenant.Status,
                       tenant.AdminId,
                       tenant.Password,
                       tenant.SubDomain,
                       tenant.Db,
                       tenant.Size,
                       tenant.SizeType,
                       tenant.Type,
                       tenant.EndPointDb,
                       tenant.EndSubDomain,
                       tenant.Action,
                       tenant.ScheduleDate,
                       tenant.ScheduleTime,
                       tenant.CreateDate,
                       tenant.RdsIdentifier);
        }
    }
}