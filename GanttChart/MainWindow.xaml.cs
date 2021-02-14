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
using GanttChart.Class;
using GanttChart.Data;
using GanttChart.Component;

namespace GanttChart
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDataManager DataManager;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //           Gantt.MoveTerm();

            Gantt.Tasks = DataManager.GetTasks();
        }

        /// <summary>
        /// 読み込みデータの反映
        /// </summary>
        private void ApplyLoadSetting()
        {
            Width = DataManager.WindowWidth;
            Height = DataManager.WindowHeight;

            Gantt.RangeStart.SelectedDate = DataManager.RangeStart;

            Gantt.RangeEnd.SelectedDate = DataManager.RangeEnd;
            Gantt.RangeSelected += Gantt_RangeSelected;
        }

        

        private void AddTaskMenu_Click(object sender, RoutedEventArgs e)
        {
            TaskEditWindow newTaskWindow = new TaskEditWindow();
            if (newTaskWindow.ShowDialog() == true)
            {
                TaskInfo taskInfo = newTaskWindow.TaskInfo;
                DataManager.AddTask(taskInfo);

                Gantt.Tasks = DataManager.GetTasks();
            }

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataManager = AppDataManager.Instance;

            ApplyLoadSetting();
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataManager != null)
            { 
                DataManager.WindowWidth = Width;
                DataManager.WindowHeight = Height;
            }
        }

        private void Gantt_RangeSelected(object sender, RangeSelectedEventArgs e)
        {
            DataManager.RangeStart = e.RangeStart;
            DataManager.RangeEnd = e.RangeEnd;
        }

        private void TagMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (new TagEditWindow()).ShowDialog(); 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataManager.Save();
        }
    }
}
