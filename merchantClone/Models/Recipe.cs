﻿using Android.App;
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
    [Serializable]
    public class RewardItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public RewardItem() { }
    }

    [Serializable]
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
    public class Recipe : ITask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int ItemId { get; set; }
        public int RequiredLevel { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] 
        public CrafterRole BelongsTo { get; set; }
        public List<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
        public List<RewardItem> RewardItems { get; set; } = new List<RewardItem>();
        public int Time { get; set; }
        public int ExperienceGain { get; set; }

        public Recipe() { }
        public Recipe(Item item, int level, CrafterRole belongsTo, List<RecipeItem> recipeItems)
        {
            ItemId = item.Id;
            RequiredLevel = level;
            BelongsTo = belongsTo;
            RecipeItems = recipeItems;

        }

    }
}