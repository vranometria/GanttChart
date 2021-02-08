using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanttChart
{
    public class InnerException : Exception
    {
        public InnerException(string msg) : base(msg) { }


    }
}
