﻿using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.GetInfoRefresh
{
    public class RefreshTokenByUserInputData : IInputData<RefreshTokenByUserOutputData>
    {
        public RefreshTokenByUserInputData(int userId, string refreshToken, string newRefreshToken)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            NewRefreshToken = newRefreshToken;
        }

        public int UserId { get; private set; }

        public string RefreshToken { get; private set; }

        public string NewRefreshToken { get; private set; }
    }
}
