using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GanttChart.Data;

namespace GanttChart.Component
{
    /// <summary>
    /// GanttChartView.xaml の相互作用ロジック
    /// </summary>
    public partial class GanttChartView : UserControl
    {
        public List<TaskInfo> TaskInfos = new List<TaskInfo>();

        public List<TaskInfo> Tasks 
        {
            get => TaskInfos;
            set 
            {
                TaskInfos = value;
                ShowTasks();
            }
        }

        public bool NoSelectedRange => RangeStart.SelectedDate == null || RangeEnd.SelectedDate == null;

        public GanttChartView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TaskInfo dummy = new TaskInfo { Start = DateTime.Now.AddDays(-3), End = DateTime.Now.AddDays(7) };
            Tasks = new List<TaskInfo> { dummy, dummy, dummy, dummy };

            ShowHeader();

            ShowTasks();

            MoveTerm();

        }

        private void ShowHeader() 
        {
            if (NoSelectedRange) { return; }

            var header = new TaskLineView(RangeStart.SelectedDate.Value, RangeEnd.SelectedDate.Value);
            HeaderScrollViewer.Content = header;

        }

        /// <summary>
        /// タスクガントチャートを表示する
        /// </summary>
        private void ShowTasks()
        {
            if (NoSelectedRange) { return; }

            TasksStack.Children.Clear();
            DateTime start = RangeStart.SelectedDate.Value;
            DateTime end = RangeEnd.SelectedDate.Value;

            Tasks.Where(task => start <= task.Start && task.Start <= task.End)
                .Select(task => new TaskLineView(task, start, end))
                .ToList()
                .ForEach(taskView => TasksStack.Children.Add(taskView));
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer target = sender == HeaderScrollViewer ? TasksScrollViewer : HeaderScrollViewer;
            target.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        public void MoveTerm()
        {
            TasksStack.Children.Cast<TaskLineView>().ToList().ForEach(x => x.MoveTermBar());
        }

        private void ChangeCalendarRange()
        {
            if (NoSelectedRange) { return; }

            ShowHeader();

            ShowTasks();

            MoveTerm();
        }

        private void RangeStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeCalendarRange();
        }

        private void RangeEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeCalendarRange();
        }
    }
}
