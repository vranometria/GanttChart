using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanttChart.Class
{
    public class TagInfo
    {
        public int Id { get; set; }

        public string Label { get; set; }

        /// <summary>
        /// このタグが付与されたタスクのID
        /// </summary>
        public List<string> RelatedIds { get; set; } = new List<string>();
    }
}
