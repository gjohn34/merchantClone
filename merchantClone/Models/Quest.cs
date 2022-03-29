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
        private static List<Quest> Quests { get; set; } = new List<Quest>();
        public List<RewardItem> RewardItems { get; set; } = new List<RewardItem>();
        public Quest() {
            Quests.Add(this);
        }
        public Quest(int id, string name, int cost, int reqLev, int time)
        {
            Id = id;
            Name = name;
            Cost = cost;
            RequiredLevel = reqLev;
            Time = time;
        }

        
        public static Quest GetQuest(int id)
        {
            return Quests.Find(x => x.Id == id);
        }
        public static List<Quest> GetQuests()
        {
            return Quests;
        }
    }
}