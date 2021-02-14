using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GanttChart.Data;

namespace GanttChart
{
    public class Util
    {
        /// <summary>座標位置(0,0)</summary>
        public static readonly Point PointZero = new Point(0, 0);


        private static bool IsHoliday(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return true;

                default:
                    return false;
            }
        }

        public static List<DateTime> GetDateList(DateTime start, DateTime end,bool exxcludeHoliday)
        {
            DateTime date = start.Date;
            List<DateTime> days = new List<DateTime>();
            while (date <= end.Date)
            {
                if(exxcludeHoliday && IsHoliday(date)) 
                {
                    date = date.AddDays(1);
                    continue; 
                }

                days.Add(date);
                date = date.AddDays(1);
            }
            return days;
        }


        public static bool HasTerm(TaskInfo task, bool excludeHoliday)
        {
            if (task.Start == null || task.End == null) { return false; }

            var start = task.Start.Value;
            var end = task.End.Value;


            //通常モード　終日タスク
            if( !excludeHoliday && start.Date == end.Date) { return true; }

            //通常モード 1日以上あり
            if( !excludeHoliday && (end - start).Days > 0) { return true; }

            //休日含めないモード　開始、終了日が休日の場合は描画不能なのでfalse
            if (excludeHoliday && (IsHoliday(start) || IsHoliday(end))) { return false; }

            //休日は含めないモード  平日が1日でもあればOK
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if( !IsHoliday(date)) { return true; }
            }

            return false;
        }
    }
}
