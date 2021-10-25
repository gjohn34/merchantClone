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
    public enum PersonType
    {
        Crafter,
        Hero
    }
    public class Person
    {
        #region Fields
        private PersonType _personType;
        private string _name;
        private int _currentExperience = 0;
        private int _currentLevel;
        
        #nullable enable
        Job? _task = null;
        #nullable disable

        const int maxLevel = 10;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public Person(PersonType type, string name)
        {
            _personType = type;
            _name = name;
        }
        public void FinishJob()
        {
            try
            {
                _currentExperience += _task.ExperienceGain;
                _task = null;
            }
            catch
            {

            }
        }
        public static int GeneratePosition(List<Person> list, int margin, int rowHeight)
        {
            return list.Count * (rowHeight + margin) + margin; 
        }
        #endregion
    }
}