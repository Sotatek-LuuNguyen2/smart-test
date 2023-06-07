﻿namespace Domain.Models.User
{
    public class FunctionMstModel
    {
        public FunctionMstModel(string functionCd, string functionName, List<PermissionMstModel> permissions)
        {
            FunctionCd = functionCd;
            FunctionName = functionName;
            Permissions = permissions;
        }

        public string FunctionCd { get; set; }

        public string FunctionName { get; set; }

        public List<PermissionMstModel> Permissions { get; private set; }
    }
}
