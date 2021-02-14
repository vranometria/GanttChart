using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GanttChart.Class;

namespace GanttChart.Data
{
    public class AppData
    {
        public double WindowWidth { get; set; } = AppDataManager.DEFAULT_WIDTH;

        public double WindowHeight { get; set; } = AppDataManager.DEFAULT_HEIGHT;

        /// <summary>
        /// 休日を除いた表示かどうか
        /// </summary>
        public bool ExcludeHoliday { get; set; }


        public DateTime? RangeStart { get; set; }

        public DateTime? RangeEnd { get; set; }

        public List<TaskInfo> TaskInfos { get; set; } = new List<TaskInfo>();

        public List<TagInfo> Tags { get; set; } = new List<TagInfo>();

    }
}
