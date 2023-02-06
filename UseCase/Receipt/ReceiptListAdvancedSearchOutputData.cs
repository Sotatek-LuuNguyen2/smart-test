﻿using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt;

public class ReceiptListAdvancedSearchOutputData : IOutputData
{
    public ReceiptListAdvancedSearchOutputData(List<ReceiptListModel> listReceipt, ReceiptListAdvancedSearchStatus status)
    {
        ListReceipt = listReceipt.Select(item => new ReceiptListAdvancedSearchOutputItem(item)).ToList();
        Status = status;
    }

    public ReceiptListAdvancedSearchOutputData(ReceiptListAdvancedSearchStatus status)
    {
        ListReceipt = new();
        Status = status;
    }

    public List<ReceiptListAdvancedSearchOutputItem> ListReceipt { get; private set; }

    public ReceiptListAdvancedSearchStatus Status { get; private set; }
}
