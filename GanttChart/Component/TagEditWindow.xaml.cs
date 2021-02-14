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
    /// TagEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TagEditWindow : Window
    {
        AppDataManager AppDataManager;

        public TagEditWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppDataManager = AppDataManager.Instance;

            AppDataManager.GetTags().ForEach( tag =>
            { 
                var view = new TagView(tag) 
                {
                    UseSelection = false
                };
                view.ClickDleted += View_ClickDleted;
                TagArea.Children.Add(view);
            });
        }

        private void View_ClickDleted(object sender)
        {
            var view = sender as TagView;

            TagArea.Children.Remove(view);

            AppDataManager.DeleteTag(view.TagInfo);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewTag.Text.Trim()))
            {
                return;
            }

            TagInfo tag = AppDataManager.AddTag(NewTag.Text);
            TagView view = new TagView(tag)
            {
                UseSelection = false
            };
            TagArea.Children.Add(view);
        }

    }
}
