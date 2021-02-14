using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using GanttChart.Data;
using System.Runtime.CompilerServices;

namespace GanttChart.Component
{
    public class TaskDetailModel : INotifyPropertyChanged
    {
        public TaskInfo TaskInfo { get; set; }

        public bool IsEditMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEnabled 
        {
            get => IsEditMode;
            set
            {
                IsEditMode = value;
                NotifyPropertyChanged();
            }
        }

        public TaskDetailModel(TaskInfo taskInfo)
        {
            TaskInfo = taskInfo;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "IsEditMode")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
