﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class CalendarGridModel
    {
        public ObservableCollection<WeekOfMonthModel> WeekOfMonths { get; private set; }
        public DateTime MonthYear { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public int MonthTextMode { get; private set; } = 1;
        public bool HighlightToday { get; private set; } = true;
        public bool HighCurrentMonth { get; private set; }
        public string MonthText { get; private set; }
        public string MonthFontWeight { get; private set; } = "Normal";
        public List<KeyValuePair<int, int>> RaiinDayDict { get; private set; }
        public Dictionary<int, string> Holidays { get; private set; }
        public List<HolidayModel> HolidayModels { get; private set; }
        public int SinDate { get; private set; }
        public List<KeyValuePair<int, string>> RaiinTags { get; private set; }
        public List<KeyValuePair<int, RaiinStateDictObjectValue>> RaiinStateDict { get; private set; }

        public CalendarGridModel(DateTime date)
        {
            WeekOfMonths = new();
            MonthYear = date;
            Month = MonthYear.Month;
            Year = MonthYear.Year;
            if (MonthTextMode == 1)
            {
                MonthText = string.Format("{0}年{1}月", Year, Month);
            }
            else
            {
                MonthText = string.Format("{0}月", Month);
            }
            RaiinDayDict = new();
            Holidays = new();
            HolidayModels = new();
            SinDate = 0;
            RaiinTags = new();
            RaiinStateDict = new();
        }
        public CalendarGridModel(CalendarGridModel model)
        {
            WeekOfMonths = model.WeekOfMonths;
            MonthYear = model.MonthYear;
            Month = model.Month;
            Year = model.Year;
            MonthText = model.MonthText;
            HighlightToday = model.HighlightToday;
            HighCurrentMonth = model.HighCurrentMonth;
            MonthFontWeight = model.MonthFontWeight;
            RaiinDayDict = model.RaiinDayDict;
            Holidays = model.Holidays;
            HolidayModels = model.HolidayModels;
            SinDate = model.SinDate;
            RaiinTags = model.RaiinTags;
            RaiinStateDict = model.RaiinStateDict;
        }

        public CalendarGridModel(CalendarGridModel model, int sinDate, List<HolidayModel> holidays, List<KeyValuePair<int, RaiinStateDictObjectValue>> stateList, List<KeyValuePair<int, string>> tagList, List<KeyValuePair<int, int>> tempList)
        {
            WeekOfMonths = model.WeekOfMonths;
            MonthYear = model.MonthYear;
            Month = model.Month;
            Year = model.Year;
            MonthText = model.MonthText;
            HighlightToday = model.HighlightToday;
            HighCurrentMonth = model.HighCurrentMonth;
            MonthFontWeight = model.MonthFontWeight;
            RaiinDayDict = tempList;
            Holidays = model.Holidays;
            HolidayModels = holidays;
            SinDate = sinDate;
            RaiinTags = tagList;
            RaiinStateDict = stateList;
        }
    }
    public class RaiinStateDictObjectValue
    {
        public int Value { get; private set; }
        public long RaiinNo { get; private set; }

        public RaiinStateDictObjectValue(int value, long raiinNo)
        {
            Value = value;
            RaiinNo = raiinNo;
        }
    }
}
