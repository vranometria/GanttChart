using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanttChart.Class
{
    public class RangeSelectedEventArgs
    {
        public DateTime? RangeStart { get; private set; }

        public DateTime? RangeEnd { get; private set; }

        public bool IsSelectedRangeComplete { get => RangeStart != null && RangeEnd != null; }

        public RangeSelectedEventArgs(DateTime? start, DateTime? end)
        {
            RangeStart = start;
            RangeEnd = end;
        }
    }
}
