﻿using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RestoreObjectS3Tenant
{
    public sealed class RestoreObjectS3TenantInputData : IInputData<RestoreObjectS3TenantOutputData>
    {
        public RestoreObjectS3TenantInputData(string objectName, dynamic webSocketService, List<RestoreObjectS3TenantTypeEnum> type, bool isPrefixDelete)
        {
            ObjectName = objectName;
            WebSocketService = webSocketService;
            Type = type;
            IsPrefixDelete = isPrefixDelete;
        }
        /// <summary>
        /// SubDomain
        /// </summary>
        public string ObjectName { get; private set; }

        public dynamic WebSocketService { get; private set; }

        public List<RestoreObjectS3TenantTypeEnum> Type { get; private set; }

        public bool IsPrefixDelete { get; private set; }

    }
}
