using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace merchantClone.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Stacks { get; set; }
    }

    public class InventoryItem
    {
        private Item item;
        public int Id { get; set; }
        public Item Item
        {
            get
            {
                if (item != null)
                {
                    return item;
                }
                else
                {
                    item = ItemDetails.GetItem(Id);
                    return item;
                }
            }
        }
        public int Quantity { get; set; }
    }
    public class RecipeItem
    {
        private Item item;
        public int Id { get; set; }
        public Item Item
        {
            get
            {
                if (item != null)
                {
                    return item;
                } else
                {
                    item = ItemDetails.GetItem(Id);
                    return item;
                }
            }
        }
        public int Quantity { get; set; }
    }
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Item Item { get; set; }
        public int RequiredLevel { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] 
        public Roles BelongsTo { get; set; }
        public List<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
        public Recipe() { }
        public Recipe(Item item, int level, Roles belongsTo, List<RecipeItem> recipeItems)
        {
            Item = item;
            RequiredLevel = level;
            BelongsTo = belongsTo;
            RecipeItems = recipeItems;

        }

    }
}