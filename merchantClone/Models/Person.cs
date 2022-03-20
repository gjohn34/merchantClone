using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using merchantClone.Controls;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace merchantClone.Models
{
    [Serializable]
    public abstract class Person : IPerson
    {
        #region Fields
        protected int _currentExperience = 0;
        protected Job _currentJob;
        #nullable enable
        #nullable disable
        #endregion

        #region Properties
        public Job Task { get; set; }
        public int CurrentXp { get; set; } = 1;
        public int TotalXp { get; set; } = 100;
        public string Name { get; protected set; }
        public int Level { get; set; } = 1;

        #endregion

        #region Methods
        public virtual void FinishTask()
        {
            Console.WriteLine("Finished Task");
            CurrentXp += Task.ExperienceGain;
            if (CurrentXp >= TotalXp)
            {

                HandleLevelUp();
            }
            Task = null;
            SaveFile.Save();
        }

        public virtual void HandleLevelUp()
        {
            Level += 1;
            CurrentXp -= TotalXp;
            TotalXp = NextTotal();
        }

        private int NextTotal()
        {
            return 100;
        }
        #endregion
    }
}