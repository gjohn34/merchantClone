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
        public string Sprite { get; set; } = "scrap";
    }

    public class InventoryItem
    {
        private Item _item;
        public int Id { get; set; }
        public Item Item
        {
            get
            {
                if (_item != null)
                {
                    return _item;
                }
                else
                {
                    _item = ItemDetails.GetItem(Id);
                    return _item;
                }
            }
        }
        public int Quantity { get; set; }
        public InventoryItem(int id, int qty)
        {
            Id = id;
            Quantity = qty;
            _item = ItemDetails.GetItem(id);
        }
        public InventoryItem() { }
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
        public int Cost { get; set; }
        public int ItemId { get; set; }
        public int RequiredLevel { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] 
        public Roles BelongsTo { get; set; }
        public List<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
        public int Time { get; set; }

        public Recipe() { }
        public Recipe(Item item, int level, Roles belongsTo, List<RecipeItem> recipeItems)
        {
            ItemId = item.Id;
            RequiredLevel = level;
            BelongsTo = belongsTo;
            RecipeItems = recipeItems;

        }

    }
}