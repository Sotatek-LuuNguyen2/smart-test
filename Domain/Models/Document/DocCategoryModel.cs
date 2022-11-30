﻿namespace Domain.Models.Document;

public class DocCategoryModel
{
    public DocCategoryModel(int categoryCd, string categoryName, int sortNo)
    {
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        SortNo = sortNo;
    }
    public DocCategoryModel()
    {
        CategoryCd = 0;
        CategoryName = string.Empty;
        SortNo = 0;
    }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public int SortNo { get; private set; }
}
