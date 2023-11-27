﻿namespace UseCase.SuperAdmin.UpgradePremium
{
    public enum UpgradePremiumStatus : byte
    {
        Successed = 1,
        InvalidTenantId = 2,
        FailedTenantIsPremium = 3,
        Failed = 4,
        TenantDoesNotExist = 5,
        RdsDoesNotExist = 6,
        NewDomainAleadyExist = 7,
        InvalidSize = 8,
        InvalidSizeType = 9,
        InvalidDomain = 10,
    }
}
