using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GanttChart.Data;

namespace GanttChart.Component
{
    /// <summary>
    /// TaskLineView.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskLineView : UserControl
    {
        private TaskInfo TaskInfo;

        private CalendarRange Range;

        private bool IsHeader;
        
        private bool IsDragging { get; set; }

        private Point StartPoint { get; set; }

        private Point InitPoint { get; set; }

        private DateCell InitCell { get; set; }

        private Thickness Position;

        private enum UpdateEventReasons { None , Drag , ChangeRange , Loaded }

        private enum Direction { Right , Left , None}

        private UpdateEventReasons UpdateEventReason;

        /// <summary>つまみD&D開始時点の長さ</summary>
        private double DragStartWidth;

        public bool IsHeaderMode 
        {
            get => IsHeader;
            set
            {
                IsHeader = value;
                ChangeHeaderMode();
            } 
        }

        public bool NoSelectedRange { get => Range.Start == null || Range.End == null; }

        public List<DateCell> DateCells { get => Calendar.Children.Cast<DateCell>().ToList() ; }


        /// <summary>
        /// コンストラクタ１
        /// </summary>
        public TaskLineView() : this(null, DateTime.Now, DateTime.Now.AddDays(7), false) { }

        /// <summary>
        /// コンストラクタ２
        /// </summary>
        /// <param name="task"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public TaskLineView(TaskInfo task, DateTime start, DateTime end) : this(task, start, end, false) { }

        /// <summary>
        /// ヘッダーモードコンストラクタ
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public TaskLineView(DateTime start, DateTime end) : this(null, start, end, true) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="taskInfo"></param>

        public TaskLineView(TaskInfo taskInfo ,DateTime? rangeStart, DateTime? rangeEnd, bool isHeader)
        {
            InitializeComponent();

            TaskInfo = taskInfo;
            Range = new CalendarRange { Start = rangeStart, End = rangeEnd };
            Range.RangeChanged += RangeChanged;
            IsHeaderMode = isHeader;
            TermBar.BasicParent = UserControlGrid;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Position.Top = TermBar.Margin.Top; //TermBarのTop位置は変えない

            UpdateEventReason = UpdateEventReasons.Loaded;

            DrawCalendar();

            if (TaskInfo != null)
            { 
                TaskTitle.Content = TaskInfo.Title;
            }
        }


        /// <summary>
        /// タスクの開始日・終了日に合わせてバーを移動する。
        /// </summary>
        public void MoveTermBar()
        {
            if(DateCells.Count == 0) { return; }

            if (TaskInfo != null && TaskInfo.Start != null && TaskInfo.End != null)
            {
                double left = GetDateCell(TaskInfo.Start.Value).GetLeftBasedOn(UserControlGrid);
                TermBar.Left = left;
                TermBar.Width = GetDateCell(TaskInfo.End.Value).GetRightBasedOn(UserControlGrid) - left - 1.5;
            }
        }


        private void DrawCalendar()
        {
            Calendar.Children.Clear();

            if (NoSelectedRange) { return; }

            Util.GetDateList(Range.Start.Value, Range.End.Value)
                .ForEach(date =>
                {
                    var cell = new DateCell(date, IsHeader) { BaseContainer = UserControlGrid };
                    Calendar.Children.Add(cell);
                });
        }

        private void RangeChanged() 
        {
            UpdateEventReason = UpdateEventReasons.ChangeRange;

            DrawCalendar();
        }

        /// <summary>
        /// ヘッダーモード/非ヘッダーモードの表示切替
        /// IsHeader変更をトリガーとする。
        /// </summary>
        private void ChangeHeaderMode()
        {
            TermBar.Visibility = TaskInfoContainer.Visibility = IsHeader ? Visibility.Hidden : Visibility.Visible; ;
            Calendar.Children.Cast<DateCell>().ToList().ForEach(cell => cell.IsHeaderMode = IsHeader);
        }

        /// <summary>
        /// 指定した位置のDateCellを取得する。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private DateCell GetDateCell(double x)
        {
            return DateCells.Where(cell => cell.IsOn(x)).FirstOrDefault();
        }

        private DateCell GetDateCell(DateTime date)
        {
            return DateCells.FirstOrDefault(cell => cell.Date == date.Date);
        }

        private DateCell GetNextCell(DateCell basisCell, int index)
        {
            var n = DateCells.IndexOf(basisCell);
            return DateCells[n + index];
        }

        #region<Thumb Move>

        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TermThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            UpdateEventReason = UpdateEventReasons.Drag;
            TermThumb.Background = Brushes.Orange;
            Cursor = Cursors.Hand;
            DragStartWidth = TermBar.Width;
            e.Handled = true;
            LayoutUpdated -= UserControl_LayoutUpdated;
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TermThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {

            UpdateEventReason = UpdateEventReasons.Drag;
            double xAdjust = TermBar.Width + e.HorizontalChange;
            if (xAdjust >= 0)
            {
                TermBar.Width = xAdjust;
            }
            e.Handled = true;
        }

        /// <summary>
        /// つまみドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TermThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            TermThumb.Background = Brushes.CadetBlue;
            UpdateEventReason = UpdateEventReasons.Drag;
            Cursor = Cursors.Arrow;

            var cell = GetDateCell(TermBar.Right);
            if(cell == null) { return; }
            
            TermBar.Width = cell.Right - TermBar.Left - 1.5;

            TaskInfo.End = cell.Date;
        }

        #endregion<Thumb Move>

        #region<TermBar Move>

        private void TermBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;

            UpdateEventReason = UpdateEventReasons.Drag;
            InitPoint = StartPoint = e.GetPosition(UserControlGrid);
            InitCell = GetDateCell(TermBar.Left);
            TermBar.CaptureMouse();
        }

        

        private void TermBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging) { return; }

            UpdateEventReason = UpdateEventReasons.Drag;

            var current = e.GetPosition(UserControlGrid);

            double offsetX = current.X - StartPoint.X;
            
            Position.Left = TermBar.Margin.Left + offsetX;

            TermBar.Margin = Position;
            StartPoint = current;
        }

        /// <summary>
        /// Termbarを動かしたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void TermBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging) { return; }

            UpdateEventReason = UpdateEventReasons.Drag;

            IsDragging = false;
            TermBar.ReleaseMouseCapture();

            var cell = GetDateCell(TermBar.Left);

            TermBar.Left = cell.GetLeftBasedOn(UserControlGrid);

            var term = (TaskInfo.End.Value - TaskInfo.Start.Value).Days;
            TaskInfo.Start = cell.Date;
            TaskInfo.End = GetNextCell(cell, term).Date;
        }

        #endregion<TermBar Move>

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            switch (UpdateEventReason)
            {
                case UpdateEventReasons.Loaded:
                case UpdateEventReasons.ChangeRange:
                    MoveTermBar();
                    break;
            }

            UpdateEventReason = UpdateEventReasons.None;
        }

        private void TaskTitle_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaskEditWindow taskEditWindow = new TaskEditWindow(TaskInfo, false);
            if (taskEditWindow.ShowDialog() == true)
            {
                TaskInfo = taskEditWindow.TaskInfo;
                MoveTermBar();
            }
        }
    }
}
