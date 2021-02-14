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
using GanttChart.Class;

namespace GanttChart.Component
{
    /// <summary>
    /// TagView.xaml の相互作用ロジック
    /// </summary>
    public partial class TagView : UserControl
    {
        public TagInfo TagInfo { get; set; }

        private string BeforeLabel;

        public bool IsSelected { get => Selection.IsChecked == true; set => Selection.IsChecked = value; }

        public delegate void DeleteHandler(object sender);

        public event DeleteHandler ClickDleted;

        public bool UseSelection 
        {
            get => Selection.Visibility == Visibility.Visible; 
            set => Selection.Visibility = value ? Visibility.Visible : Visibility.Hidden; 
        }

        public bool EditEnabled 
        {
            set => EditMenu.Visibility = value ? Visibility.Visible : Visibility.Hidden ; 
        }

        public TagView() : this(null) { }

        public TagView(TagInfo tag)
        {
            InitializeComponent();

            TagInfo = tag;

            Launch();
        }

        private void Launch()
        {
            DisplayLabel.Content = TagInfo.Label;
            EditTextBox.Text = TagInfo.Label;
            EditTextBox.Width = double.NaN;
        }

        private void ChangeDisplayMode()
        {
            EditTextBox.Visibility = Visibility.Hidden;
            DisplayLabel.Visibility = Visibility.Visible;
            DisplayLabel.Width = double.NaN;
            EditTextBox.Width = double.NaN;
        }

        private void ChangeEditMode()
        {
            EditTextBox.Visibility = Visibility.Visible;
            DisplayLabel.Visibility = Visibility.Hidden;
            EditTextBox.Width = 500;
        }

        private void EditMenu_Click(object sender, RoutedEventArgs e)
        {
            ChangeEditMode();
            BeforeLabel = DisplayLabel.Content as string;
        }

        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EditTextBox.Text.Trim()))
            {
                EditTextBox.Text = BeforeLabel;
            }
            else
            {
                TagInfo.Label = EditTextBox.Text;
            }

            Launch();

            ChangeDisplayMode();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ClickDleted(this);
        }
    }
}
