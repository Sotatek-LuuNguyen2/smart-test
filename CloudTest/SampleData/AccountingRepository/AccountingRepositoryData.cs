﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.SampleData.AccountingRepository
{
    public class AccountingRepositoryData
    {
        public static List<RaiinInf> ReadRaiinInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var raiinInfs = new List<RaiinInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RaiinInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var raiinInf = new RaiinInf();
                        raiinInf.CreateId = 1;
                        raiinInf.CreateDate = DateTime.UtcNow;
                        raiinInf.UpdateId = 1;
                        raiinInf.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    raiinInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int raiinNo);
                                    raiinInf.RaiinNo = raiinNo;
                                    break;
                                case "C":
                                    long.TryParse(text, out long ptId);
                                    raiinInf.PtId = ptId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDate);
                                    raiinInf.SinDate = sinDate;
                                    break;
                                case "E":
                                    long.TryParse(text, out long oyaRaiinNo);
                                    raiinInf.OyaRaiinNo = oyaRaiinNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int status);
                                    raiinInf.Status = status;
                                    break;
                                default:
                                    break;
                            }
                        }
                        raiinInfs.Add(raiinInf);
                    }
                }
            }

            return raiinInfs;
        }

        public static List<KaikeiInf> ReadKaikeiInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var kaikeiInfs = new List<KaikeiInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "KaikeiInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var kaikeiInf = new KaikeiInf();
                        kaikeiInf.CreateId = 1;
                        kaikeiInf.CreateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    kaikeiInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    kaikeiInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinDate);
                                    kaikeiInf.SinDate = sinDate;
                                    break;
                                case "D":
                                    int.TryParse(text, out int raiinNo);
                                    kaikeiInf.RaiinNo = raiinNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        kaikeiInfs.Add(kaikeiInf);
                    }
                }
            }

            return kaikeiInfs;
        }

        public static List<PtInf> ReadPtInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptInfs = new List<PtInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptInf = new PtInf();
                        ptInf.CreateId = 1;
                        ptInf.CreateDate = DateTime.UtcNow;
                        ptInf.UpdateId = 1;
                        ptInf.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptInf.SeqNo = seqNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptInfs.Add(ptInf);
                    }
                }
            }
            return ptInfs;
        }

        public static List<PtHokenInf> ReadPtHokenInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptHokenInfs = new List<PtHokenInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtHokenInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenInf = new PtHokenInf();
                        ptHokenInf.CreateId = 1;
                        ptHokenInf.CreateDate = DateTime.UtcNow;
                        ptHokenInf.UpdateId = 1;
                        ptHokenInf.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptHokenInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenInf.HokenId = hokenId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptHokenInf.SeqNo = seqNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenInfs.Add(ptHokenInf);
                    }
                }
            }

            return ptHokenInfs;
        }

        public static List<PtHokenPattern> ReadPtHokenPattern()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptHokenPatterns = new List<PtHokenPattern>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtHokenPattern").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenPattern = new PtHokenPattern();
                        ptHokenPattern.CreateId = 1;
                        ptHokenPattern.CreateDate = DateTime.UtcNow;
                        ptHokenPattern.UpdateId = 1;
                        ptHokenPattern.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptHokenPattern.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenPattern.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenPId);
                                    ptHokenPattern.HokenPid = hokenPId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptHokenPattern.SeqNo = seqNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenPattern.HokenId = hokenId;
                                    break;
                                case "H":
                                    int.TryParse(text, out int kohi1Id);
                                    ptHokenPattern.Kohi1Id = kohi1Id;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenPatterns.Add(ptHokenPattern);
                    }
                }
            }

            return ptHokenPatterns;
        }

        public static List<PtKohi> ReadPtKohi()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptKohis = new List<PtKohi>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtKohi").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptKohi = new PtKohi();
                        ptKohi.CreateId = 1;
                        ptKohi.CreateDate = DateTime.UtcNow;
                        ptKohi.UpdateId = 1;
                        ptKohi.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptKohi.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptKohi.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    ptKohi.HokenId = hokenId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptKohi.SeqNo = seqNo;
                                    break;

                                case "F":
                                    int.TryParse(text, out int hokenNo);
                                    ptKohi.HokenNo = hokenNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenEdaNo);
                                    ptKohi.HokenEdaNo = hokenEdaNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptKohis.Add(ptKohi);
                    }
                }
            }
            return ptKohis;
        }

        public static List<PtHokenCheck> ReadPtHokenCheck()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptHokenChecks = new List<PtHokenCheck>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtHokenCheck").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenCheck = new PtHokenCheck();
                        ptHokenCheck.CreateId = 1;
                        ptHokenCheck.CreateDate = DateTime.UtcNow;
                        ptHokenCheck.UpdateId = 1;
                        ptHokenCheck.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptHokenCheck.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenCheck.PtID = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenGrp);
                                    ptHokenCheck.HokenGrp = hokenGrp;
                                    break;
                                case "D":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenCheck.HokenId = hokenId;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenChecks.Add(item: ptHokenCheck);
                    }
                }
            }
            return ptHokenChecks;
        }

        public static List<HokenMst> ReadHokenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var hokenMsts = new List<HokenMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "HokenMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var hokenMst = new HokenMst();
                        hokenMst.CreateId = 1;
                        hokenMst.CreateDate = DateTime.UtcNow;
                        hokenMst.UpdateId = 1;
                        hokenMst.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    hokenMst.HpId = hpId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenNo);
                                    hokenMst.HokenNo = hokenNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int hokenEdaNo);
                                    hokenMst.HokenEdaNo = hokenEdaNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        hokenMsts.Add(hokenMst);
                    }
                }
            }
            return hokenMsts;
        }

        private static Worksheet GetworksheetBySheetName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {

            var workbookPart = spreadsheetDocument.WorkbookPart;
            StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Name.Equals(sheetName))?.Id ?? string.Empty;

            var worksheet = (workbookPart != null && !string.IsNullOrEmpty(relationshipId.ToString())) ? ((WorksheetPart)workbookPart.GetPartById(relationshipId?.Value ?? string.Empty)).Worksheet : new();

            return worksheet;
        }

        private static string GetColumnName(string text)
        {
            var check = int.TryParse(text.Skip(1).FirstOrDefault().ToString(), out int number);
            if (check)
            {
                return text.FirstOrDefault().ToString();
            }
            else
            {
                return text.Substring(0, 2);
            }
        }
    }
}
