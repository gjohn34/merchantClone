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

namespace merchantClone.Models
{
    public abstract class Job
    {
        public DateTime FinishTime { get; set; }
        public int ExperienceGain { get; set; }
        public Person BelongsTo { get; }
    }
}