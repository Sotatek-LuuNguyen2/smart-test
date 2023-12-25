﻿
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace Interactor.SuperAdmin
{
    public class RestoreObjectS3TenantInteractor : IRestoreObjectS3TenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationRepository _notificationTaskRunRepository;
        private readonly ITenantRepository _tenantRepository;
        public RestoreObjectS3TenantInteractor(
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            INotificationRepository notificationTaskRunRepository,
            ITenantRepository tenantRepository)
        {
            _awsSdkService = awsSdkService;
            _notificationRepository = notificationRepository;
            _notificationTaskRunRepository = notificationTaskRunRepository;
            _tenantRepository = tenantRepository;
        }
        public RestoreObjectS3TenantOutputData Handle(RestoreObjectS3TenantInputData inputData)
        {
            IWebSocketService _webSocketService;
            _webSocketService = (IWebSocketService)inputData.WebSocketService;
            try
            {
                if (string.IsNullOrEmpty(inputData.ObjectName))
                {
                    return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Failed);
                }
                var checkSubDomain = _tenantRepository.CheckExistsSubDomain(inputData.ObjectName);
                if (!checkSubDomain)
                {
                    return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.SubdomainDoesNotExist);
                }
                var domain = $"{inputData.ObjectName}.{ConfigConstant.Domain}";
                Task.Run(() =>
                {
                    try
                    {
                        var restoreObjectS3 = _awsSdkService.CopyObjectsInFolderAsync(
                        ConfigConstant.SourceBucketName,
                        domain,
                        ConfigConstant.DestinationBucketName,
                        inputData.Type, inputData.IsPrefixDelete);
                        restoreObjectS3.Wait();
                        var message = $"医療機関{inputData.ObjectName} のS3 データが復元されました。";
                        var notification = _notificationTaskRunRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, message);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                    }
                    catch (Exception ex)
                    {
                        var message = $"医療機関{inputData.ObjectName} のS3 データの回復に失敗しました: {ex.Message}";
                        var notification = _notificationTaskRunRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                    }
                    finally
                    {
                        _notificationTaskRunRepository.ReleaseResource();
                    }
                });

                var message = $"医療機関{inputData.ObjectName} のS3 データを復元しています。";
                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiInfo, message);
                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Success);
            }
            catch (Exception ex)
            {
                var message = inputData.ObjectName + $" is restore data S3 failed. {ex.Message}";
                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Failed);
            }
            finally
            {
                _notificationRepository.ReleaseResource();
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
