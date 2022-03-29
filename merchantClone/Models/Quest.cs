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
        public Quest(int id, string name, int cost, int reqLev, int time)
        {
            Id = id;
            Name = name;
            Cost = cost;
            RequiredLevel = reqLev;
            Time = time;
        }

        private static List<Quest> _quests = new List<Quest>
        {
            new Quest(1, "first q", 1, 1, 10),
            new Quest(2, "2nd q", 1, 1, 10),
            new Quest(3, "third q", 1, 1, 10),
        };

        public static Quest GetQuest(int id)
        {
            return _quests.Find(x => x.Id == id);
        }
    }
}