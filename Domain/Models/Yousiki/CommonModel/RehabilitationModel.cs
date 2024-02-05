﻿using Domain.Models.Yousiki.CommonModel.CommonOutputModel;

namespace Domain.Models.Yousiki.CommonModel;

public class RehabilitationModel
{
    public RehabilitationModel(List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<OutpatientConsultationModel> outpatientConsultationList, List<CommonForm1Model> byomeiRehabilitationList, List<PatientStatusModel> barthelIndexList, List<PatientStatusModel> fimList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        OutpatientConsultationList = outpatientConsultationList;
        ByomeiRehabilitationList = byomeiRehabilitationList;
        BarthelIndexList = barthelIndexList;
        FimList = fimList;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<OutpatientConsultationModel> OutpatientConsultationList { get; private set; }

    public List<CommonForm1Model> ByomeiRehabilitationList { get; private set; }

    public List<PatientStatusModel> BarthelIndexList { get; private set; }

    public List<PatientStatusModel> FimList { get; private set; }
}
