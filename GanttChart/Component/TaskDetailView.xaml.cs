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
using GanttChart.Class;

namespace GanttChart.Component
{
    /// <summary>
    /// TaskDetailView.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskDetailView : UserControl
    {
        AppDataManager AppDataManager;
        public TaskDetailModel Model { get; private set; }

        private TaskInfo TaskInfo;

        public TaskInfo Task 
        {
            get => TaskInfo;
            set
            {
                TaskInfo = value;
                DataContext = Model = new TaskDetailModel(TaskInfo);
            } 
        }

        public bool IsEditMode { get => Model.IsEnabled; set => Model.IsEnabled = value; }


        /// <summary>
        /// コンストラクタ１
        /// </summary>
        public TaskDetailView() : this(new TaskInfo()) { }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="taskInfo"></param>
        public TaskDetailView(TaskInfo taskInfo)
        {
            InitializeComponent();

            Task = taskInfo;
            AppDataManager = AppDataManager.Instance;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LaunchTags();
        }

        private void LaunchTags()
        {
            if(TaskInfo == null) { return; }

            TagArea.Children.Clear();

            AppDataManager.GetTags(TaskInfo).ForEach(tag =>
            {
                var view = new TagView(tag)
                {
                    UseSelection = false,
                    EditEnabled = false,
                };
                TagArea.Children.Add(view);
            });
        }

        private void TagEdit_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectTagWindow(TaskInfo);
            if (window.ShowDialog() == true)
            {
                LaunchTags();
            }
        }
    }
}
