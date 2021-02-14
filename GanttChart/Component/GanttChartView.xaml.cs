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
using GanttChart.Class;
using GanttChart.Data;

namespace GanttChart.Component
{
    /// <summary>
    /// GanttChartView.xaml の相互作用ロジック
    /// </summary>
    public partial class GanttChartView : UserControl
    {
        public List<TaskInfo> TaskInfos = new List<TaskInfo>();

        private AppDataManager AppDataManager;

        public delegate void RangeSelectedEventHandler(object sender, RangeSelectedEventArgs e);

        public event RangeSelectedEventHandler RangeSelected;

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
            if (AppDataManager.IsInitialized)
            { 
                AppDataManager = AppDataManager.Instance;
                ExcludeHoliday.IsChecked = AppDataManager.ExcludeHoliday;
            }

            ShowHeader();

            ShowTasks();

            MoveTerm();
        }


        private void ShowHeader() 
        {
            if (NoSelectedRange) { return; }

            var header = new TaskLineView(
                start: RangeStart.SelectedDate.Value, 
                end: RangeEnd.SelectedDate.Value, 
                excludeHoliday: ExcludeHoliday.IsChecked==true
                );
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

            Tasks.Where(task => start <= task.Start && task.End <= task.End && Util.HasTerm(task, ExcludeHoliday.IsChecked == true))
                .Select(task => new TaskLineView(
                    task: task,
                    start: start,
                    end: end,
                    excludeHoliday: ExcludeHoliday.IsChecked == true)
                )
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

        private RangeSelectedEventArgs CreateEventArgs()
        {
            return new RangeSelectedEventArgs(RangeStart.SelectedDate, RangeEnd.SelectedDate);
        }

        private void RangeStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeCalendarRange();


            if (RangeSelected != null)
            {
                RangeSelected(this, CreateEventArgs());
            }

        }

        private void RangeEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeCalendarRange();


            if (RangeSelected != null)
            {
                RangeSelected(this, CreateEventArgs());
            }
        }

        private void ExcludeHoliday_Checked(object sender, RoutedEventArgs e)
        {
            ChangeCalendarRange();

            AppDataManager.ExcludeHoliday = true;
        }

        private void ExcludeHoliday_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeCalendarRange();

            AppDataManager.ExcludeHoliday = false;
        }
    }
}
