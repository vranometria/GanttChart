using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using GanttChart.Class;

namespace GanttChart.Data
{
    public class AppDataManager
    {

        #region<singleton>

        private static AppDataManager SingletonInstance;

        public static AppDataManager Instance { get => SingletonInstance == null ? (SingletonInstance = InitInstance()) : SingletonInstance; }

        public static bool IsInitialized => SingletonInstance != null;

        #endregion<singleton>

        public const string APP_DATA_FILE = "app.json";

        public const double DEFAULT_WIDTH = 800;

        public const double DEFAULT_HEIGHT = 450;

        private AppData AppData;


        private AppDataManager(AppData appData) 
        {
            AppData = appData;
        }

        public List<TagInfo> GetTags()
        {
            return AppData.Tags;
        }

        public List<TagInfo> GetTags(TaskInfo taskInfo)
        {
            return GetTags().Where(x => x.RelatedIds.Contains(taskInfo.Id)).ToList();
        }

        private static AppDataManager InitInstance()
        {
            if (SingletonInstance == null)
            {
                AppData appData = File.Exists(APP_DATA_FILE) ? Load() : new AppData();
                return new AppDataManager(appData);
            }
            else
            {
                return SingletonInstance;
            }
        }


        public void AddTask(TaskInfo taskInfo)
        {
            AppData.TaskInfos.Add(taskInfo);
        }

        internal void DeleteTag(TagInfo tagInfo)
        {
            AppData.Tags.Remove(tagInfo);
        }

        internal TagInfo AddTag(string text)
        {
            int id = AppData.Tags.Count == 0 ? 0 : AppData.Tags.Max(x => x.Id) + 1;
            TagInfo tag = new TagInfo { Label = text, Id = id };
            AppData.Tags.Add(tag);
            return tag;
        }

        internal void UpdateTagRelation(TaskInfo taskInfo, List<TagInfo> selectedTags)
        {
            GetTags(taskInfo).ForEach(tag => tag.RelatedIds.Remove(taskInfo.Id));

            selectedTags.ForEach(tag => tag.RelatedIds.Add(taskInfo.Id));
        }

        public List<TaskInfo> GetTasks() { return AppData.TaskInfos; }

        public double WindowWidth { get => AppData.WindowWidth; set { AppData.WindowWidth = value; } }

        public double WindowHeight { get => AppData.WindowHeight; set => AppData.WindowHeight = value; }

        public DateTime? RangeStart { get => AppData.RangeStart; set => AppData.RangeStart = value; }

        public DateTime? RangeEnd { get => AppData.RangeEnd; set => AppData.RangeEnd = value; }

        public bool ExcludeHoliday { get => AppData.ExcludeHoliday; set => AppData.ExcludeHoliday = value; }

        public void Save()
        {
            var json = JsonSerializer.Serialize(AppData);

            File.WriteAllText(APP_DATA_FILE,json);
        }

        private static AppData Load()
        {
            var json = File.ReadAllText(APP_DATA_FILE);
            return JsonSerializer.Deserialize<AppData>(json);
        }
    }
}
