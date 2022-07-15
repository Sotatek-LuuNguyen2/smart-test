﻿namespace EmrCloudApi.Tenant.Constants
{
    public static class ResponseMessage
    {
        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        public static readonly string GetUserListSuccessed = "Get userList successed!!!";


        //Reception controller
        public static readonly string GetReceptionInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetReceptionNotExisted = "Not existed";
        public static readonly string GetReceptionSuccessed = "Successed";

        //OrdInf controller
        public static readonly string GetOrdInfInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetOrdInfInvalidHpId = "Invalid hpId";
        public static readonly string GetOrdInfInvalidPtId = "Invalid ptId";
        public static readonly string GetOrdInfInvalidSinDate = "Invalid sinDate";
        public static readonly string GetOrdInfNoData = "Invalid sinDate";
        public static readonly string GetOrdInfSuccessed = "Successed";

    }
}
