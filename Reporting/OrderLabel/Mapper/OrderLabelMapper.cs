﻿using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;

namespace Reporting.OrderLabel.Mapper;

public class OrderLabelMapper : CommonReportingRequest
{
    private readonly List<CoOrderLabelPrintDataModel> _printOutData;

    public OrderLabelMapper(List<CoOrderLabelPrintDataModel> printOutData)
    {
        _printOutData = printOutData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.OrderLabel;
    }

    public override string GetRowCountFieldName()
    {
        return "lsOrder";
    }

    public override string GetJobName()
    {
        return "オーダーラベル";
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        Dictionary<string, string> data = new();

        return data;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        if (_printOutData == null)
        {
            return new();
        }
        List<Dictionary<string, CellModel>> result = new();
        foreach (var item in _printOutData)
        {
            Dictionary<string, CellModel> data = new Dictionary<string, CellModel>
            {
                { "lsOdrKbn", new CellModel(item.InOut) },
                { "lsRpNo", new CellModel(item.RpNo) },
                { "lsOrder", new CellModel(item.Data) },
                { "lsOrderWide", new CellModel(item.DataWide) },
                { "lsComment", new CellModel(item.Comment) },
                { "lsSuryo", new CellModel(item.Suuryo) },
                { "lsTani", new CellModel(item.Tani) }
            };
            result.Add(data);
        }
        return result;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        if (_printOutData == null ||
            !_printOutData.Any())
        {
            return new Dictionary<string, bool>();
        }
        Dictionary<string, bool> data = new();
        return data;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new Dictionary<string, bool>() {
                   { "lsOrder", true },
                   { "lsOrderWide", true },
                   { "lsComment", true },
                   { "lsSuryo", true }
        };
    }
}
