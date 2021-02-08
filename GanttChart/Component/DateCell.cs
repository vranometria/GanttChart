using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GanttChart.Component
{
    public class DateCell:Border
    {
        public DateTime Date { get; set; }

        private bool IsHeader { get; set; }

        public double Left { get => TranslatePoint(Util.PointZero, BaseContainer == null ? Parent as UIElement : BaseContainer).X; }

        public double Right { get => Left + Width; }

        /// <summary>
        /// 位置情報を返すときに基準とする親コントロール
        /// </summary>
        public UIElement BaseContainer { get; set; } = null;

        public bool IsHeaderMode 
        {
            get => IsHeader;
            set
            { 
                IsHeader = value;
                ChangeDateViewVisible();
            }
        }

        private StackPanel DateView { get; set; }

        public DateCell(DateTime date, bool isHeader) 
        {
            Date = date;
            IsHeader = isHeader;

            DrawBasic();
        }

        private void DrawBasic()
        {
            BorderBrush = Brushes.LightGray;
            BorderThickness = new Thickness(1);
            Width = 25;
            Height = 70;
            Child = DateView = CreateDateView(Date, IsHeader);
            
        }

        private static StackPanel CreateDateView(DateTime date, bool isHeader) 
        {
            StackPanel dateView = new StackPanel { Visibility = isHeader ? Visibility.Visible : Visibility.Hidden };
            Label month = new Label { Content = date.Month };
            Label day = new Label { Content = date.Day };
            dateView.Children.Add(month);
            dateView.Children.Add(day);

            return dateView;
        }

        private void ChangeDateViewVisible()
        {
            DateView.Visibility = IsHeader ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// 指定した位置にいるのが自分か判定する
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool IsOn(double x) => Left <= x && x <= Right;

        public double GetLeftBasedOn(UIElement uIElement) => TranslatePoint(Util.PointZero, uIElement).X;


        public double GetRightBasedOn(UIElement uIElement) => GetLeftBasedOn(uIElement) + Width;

    }
}
