using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttChart.Component
{
    public class TermBer:Canvas
    {
        private Thickness Position;

        /// <summary>１日より小さくした場合に１日分の幅にするための物</summary>
        private static readonly DateCell SampleDateCell = new DateCell(DateTime.Now,false);

        /// <summary>位置を返すための基準とする親コントロール</summary>
        public UIElement BasicParent { get; set; }

        public double Top { get => TranslatePoint(Util.PointZero, BasicParent).Y; }

        public double Left 
        {
            get => TranslatePoint(Util.PointZero, BasicParent).X;
            set
            {
                Position.Left = value;
                Position.Top = Margin.Top;
                Margin = Position;
            }
        }

        public double Right 
        {
            get => Left + Width;
        }



    }
}
