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
    public class Quest : ITask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int RequiredLevel { get; set; }
        public int Time { get; set; }
        public List<Item> Reward { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Quest() { }
    }
}