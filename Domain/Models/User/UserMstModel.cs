﻿namespace Domain.Models.User;

public class UserMstModel
{
    public UserMstModel(int userId, int jobCd, int managerKbn, int kaId,
        string kanaName, string name, string sname, string drName, string loginId,
        string loginPass, string mayakuLicenseNo, int startDate, int endDate,
        int sortNo, string renkeiCd1, int isDeleted)
    {
        UserId = userId;
        JobCd = jobCd;
        ManagerKbn = managerKbn;
        KaId = kaId;
        KanaName = kanaName;
        Name = name;
        Sname = sname;
        DrName = drName;
        LoginId = loginId;
        LoginPass = loginPass;
        MayakuLicenseNo = mayakuLicenseNo;
        StartDate = startDate;
        EndDate = endDate;
        SortNo = sortNo;
        RenkeiCd1 = renkeiCd1;
        IsDeleted = isDeleted;
    }

    public int UserId { get; private set; }
    public int JobCd { get; private set; }
    public int ManagerKbn { get; private set; }
    public int KaId { get; private set; }
    public string KanaName { get; private set; }
    public string Name { get; private set; }
    public string Sname { get; private set; }
    public string DrName { get; private set; }
    public string LoginId { get; private set; }
    public string LoginPass { get; private set; }
    public string MayakuLicenseNo { get; private set; }
    public int StartDate { get; private set; }
    public int EndDate { get; private set; }
    public int SortNo { get; private set; }
    public string RenkeiCd1 { get; private set; }
    public int IsDeleted { get; private set; }
}
