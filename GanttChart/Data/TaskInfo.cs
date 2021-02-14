using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GanttChart.Class;

namespace GanttChart.Data
{
    public class TaskInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }

        public string Memo { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

    }
}
