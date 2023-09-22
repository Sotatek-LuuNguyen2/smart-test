﻿using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITenantProvider
    {
        string GetConnectionString();

        string GetTenantInfo();
       
        string GetClinicID();

        TenantNoTrackingDataContext GetNoTrackingDataContext();

        TenantDataContext GetTrackingTenantDataContext();

        TenantNoTrackingDataContext ReloadNoTrackingDataContext();

        TenantDataContext ReloadTrackingDataContext();

        TenantDataContext CreateNewTrackingDataContext();

        TenantNoTrackingDataContext CreateNewNoTrackingDataContext();

        void DisposeDataContext();

        string GetDomainFromQueryString();

        string GetDomainFromHeader();

        string GetDomain();

        string GetClientIp();

        int GetHpId();

        int GetUserId();

        int GetDepartmentId();

        Task<string> GetRequestInfoAsync();
    }
}
