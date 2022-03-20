using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace merchantClone.Models
{
    public enum HeroRole
    {
        Warrior,
        Mage,
        Rogue
    }
    [Serializable]
    public class Hero : Person
    {
        public HeroRole Role { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public Hero() { }
        public Hero(string name, HeroRole role, [Optional] Job job)
        {
            Name = name;
            Role = role;
            Task = job;
        }
        public override void FinishTask()
        {
            base.FinishTask();
        }
        public override void HandleLevelUp()
        {
            base.HandleLevelUp();
            Strength += 1;
            Intelligence += 1;
            Dexterity += 1;
        }
    }
}