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
    public class Recipe
    {
        public string Name { get; set; }

        public int RequiredLevel { get; set; }
        public Roles BelongsTo { get; }
        public Recipe(string name, int level, Roles belongsTo)
        {
            Name = name;
            RequiredLevel = level;
            BelongsTo = belongsTo;
        }
    }
}