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

namespace GanttChart.Component
{
    /// <summary>
    /// SelectTagWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectTagWindow : Window
    {
        private TaskInfo TaskInfo;

        private AppDataManager AppDataManager;

        public SelectTagWindow() : this(null) { }

        public SelectTagWindow(TaskInfo taskInfo)
        {
            InitializeComponent();

            AppDataManager = AppDataManager.Instance;
            TaskInfo = taskInfo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateTagViews().ForEach(x => TagArea.Children.Add(x));
        }

        private List<TagView> CreateTagViews()
        {
            var selectedTag = AppDataManager.GetTags(TaskInfo);

            return AppDataManager.GetTags().Select(tag =>
            {
                bool isSelected = selectedTag.FirstOrDefault(t => t == tag) != null;

                var view = new TagView(tag)
                {
                    IsSelected = isSelected,
                    EditEnabled = false,
                };

                return view;
            }).ToList();
        }

        private void Decision_Click(object sender, RoutedEventArgs e)
        {
            var selectedTags = TagArea.Children.Cast<TagView>()
                .Where(v => v.IsSelected).Select(t => t.TagInfo)
                .ToList();

            AppDataManager.UpdateTagRelation(TaskInfo, selectedTags);

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void NameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TagArea.Children.Clear();

            CreateTagViews().Where(x => x.TagInfo.Label.Contains(NameFilter.Text.Trim()))
                .ToList()
                .ForEach(x => TagArea.Children.Add(x));
        }
    }
}
