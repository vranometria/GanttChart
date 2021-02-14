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
using GanttChart.Data;

namespace GanttChart.Component
{
    /// <summary>
    /// TaskEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskEditWindow : Window
    {
        public TaskInfo TaskInfo { get; private set; }

        /// <summary>コンストラクタ１</summary>
        public TaskEditWindow() : this(new TaskInfo(), true){}

        /// <summary>コンストラクタ２</summary>
        public TaskEditWindow(TaskInfo taskInfo) : this(taskInfo, true) { }

        /// <summary>コンストラクタ</summary>
        public TaskEditWindow(TaskInfo taskInfo ,bool IsEditMode)
        {
            InitializeComponent();

            TaskDetailView.IsEditMode = IsEditMode;

            TaskDetailView.Task = taskInfo;
        }

        private void DecisionButton_Click(object sender, RoutedEventArgs e)
        {
            TaskInfo = TaskDetailView.Task;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
