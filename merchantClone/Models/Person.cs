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
    public abstract class Person : Role, IPerson
    {
        #region Fields
        protected int _currentExperience = 0;
        protected int _currentLevel = 1;
        protected Job _currentJob;

        #nullable enable
        Job? _task = null;
        #nullable disable
        #endregion

        #region Properties
        public int CurrentXp { get; set; } = 1;
        public int TotalXp { get; set; } = 100;
        public string Name { get; protected set; }
        public Roles Job { get; protected set; }
        public Role Role { get; set; }

        #endregion

        #region Methods
        public int GetLevel()
        {
            return _currentLevel;
        }
        public void FinishTask()
        {
            CurrentXp += 10;
            if (CurrentXp >= TotalXp)
            {
                _currentLevel += 1;
                CurrentXp -= TotalXp;
                TotalXp = NextTotal();
                UpdateJobsList();
                SaveFile.Save();
            }
        }

        private int NextTotal()
        {
            return 100;
        }
        public abstract void UpdateJobsList();
        #endregion
    }
}