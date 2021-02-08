using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanttChart.Data
{
    public class CalendarRange
    {
        private DateTime RangeStart;

        private DateTime RangeEnd;

        public delegate void EventHandler();

        public EventHandler RangeChanged;

        public DateTime Start
        {
            get => RangeStart;
            set
            {
                RangeStart = value.Date;
                if (RangeChanged != null) { RangeChanged(); }
            }
        }

        public DateTime End 
        {
            get => RangeEnd;
            set
            {
                RangeEnd = value.Date;
                if (RangeChanged != null) { RangeChanged(); }
            }
        }

    }
}
