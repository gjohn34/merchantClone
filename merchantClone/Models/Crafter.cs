using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace merchantClone.Models
{
    public class Crafter : Person
    {
        private List<Recipe> _availableJobs = new List<Recipe>();
        private List<Recipe> _learnedJobs = new List<Recipe>();
        public Crafter() { }
        public Crafter(string name, Roles role, [Optional] Job job) {
            Name = name;
            Role = role;
            Task = job;
            _availableJobs = ItemDetails.GetRecipes().FindAll(recipe => recipe.BelongsTo == role);
            UpdateJobsList();
        }

        public List<Recipe> GetJobs()
        {
            return _learnedJobs;
        }

        public override void UpdateJobsList()
        {
            _learnedJobs = _availableJobs.FindAll(recipe => recipe.RequiredLevel <= Level);
        }

        public void StartJobsList()
        {
            _availableJobs = ItemDetails.GetRecipes().FindAll(recipe => recipe.BelongsTo == Role);
            _learnedJobs = _availableJobs.FindAll(recipe => recipe.RequiredLevel <= Level);
        }

        internal void AssignTask(Recipe recipe)
        {
            GameInfo.Instance.ReduceGold(recipe.Cost);
            GameInfo.Instance.ReduceInventory(recipe.RecipeItems);
            DateTime now = DateTime.Now;
            DateTime finish = now.AddSeconds(recipe.Time);
            Task = new Job(recipe.Name, finish, recipe);
            SaveFile.Save();
        }


        //public List<ComponentRow> ShowRecipes(Texture2D _texture, SpriteFont _font)
        //{

        //    List<ComponentRow> components = new List<ComponentRow>();
        //    Button one = new Button(_texture, _font) { Position = new Vector2(10, 10), Text = "Recipes1" };
        //    Button two = new Button(_texture, _font) { Position = new Vector2(10, 10), Text = "Recipes2" };
        //    Button three = new Button(_texture, _font) { Position = new Vector2(10, 10), Text = "Recipes3" };
        //    components.Add((ComponentRow)new RecipeGroup(
        //        new Component[3] { one, two, three },
        //        new Rectangle(0, 0, 400, 600)
        //    ));

        //    return components;
        //}
    }
}