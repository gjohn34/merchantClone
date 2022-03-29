using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace merchantClone.Models
{
    [Serializable]
    public class Job
    {
        public string Name { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public int Seconds { get; set; }
        public int ExperienceGain { get; set; }
        public Person BelongsTo { get; }
        public int TaskId { get; set; }
        [XmlIgnore]
        public ITask Task { get; set; }
        public Job() { }
        public Job(string name, DateTime finishTime, ITask task) {
            Name = name;
            FinishTime = finishTime;
            TaskId = task.Id;
            Seconds = task.Time;
            Task = task;
            //Seconds = (int)finishTime.Subtract(StartTime).TotalSeconds;
        }

        internal bool IsDone()
        {
            DateTime now = DateTime.Now;
            if (FinishTime <= now)
            {
                return true;
            }
            return false;
        }
        internal int SecondsLeft()
        {
            DateTime now = DateTime.Now;
            return (int)FinishTime.Subtract(now).TotalSeconds;
        }
    }
}