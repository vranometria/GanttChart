using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GanttChart
{
    public class Util
    {
        /// <summary>座標位置(0,0)</summary>
        public static readonly Point PointZero = new Point(0, 0);

        public static List<DateTime> GetDateList(DateTime start, DateTime end)
        {
            DateTime date = start.Date;
            List<DateTime> days = new List<DateTime>();
            while (date <= end.Date)
            {
                days.Add(date);
                date = date.AddDays(1);
            }
            return days;
        }
    }
}
