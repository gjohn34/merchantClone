using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace merchantClone.Models
{
    [Serializable]
    public enum CrafterRole
    {
        Armorer,
        Blacksmith,
        Carpenter
    }
    public class Crafter : Person
    {
        private List<Recipe> _availableJobs = new List<Recipe>();
        private List<Recipe> _learnedJobs = new List<Recipe>();
        public CrafterRole Role { get; set; }
        public Crafter() { }
        public Crafter(string name, CrafterRole role, [Optional] Job job) {
            Name = name;
            Role = role;
            Job = job;
            _availableJobs = ItemDetails.GetRecipes().FindAll(recipe => recipe.BelongsTo == role);
            UpdateJobsList();
        }
        public override void FinishTask()
        {
            base.FinishTask();
        }

        public List<Recipe> GetJobs()
        {
            return _learnedJobs;
        }

        public override void HandleLevelUp()
        {
            base.HandleLevelUp();
            UpdateJobsList();
        }

        private void UpdateJobsList()
        {
            _learnedJobs = _availableJobs.FindAll(recipe => recipe.RequiredLevel <= Level);
        }

        public void StartJobsList()
        {
            _availableJobs = ItemDetails.GetRecipes().FindAll(recipe => recipe.BelongsTo == Role);
            UpdateJobsList();
        }

    }
}