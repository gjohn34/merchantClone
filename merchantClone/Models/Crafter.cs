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