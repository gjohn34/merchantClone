using merchantClone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using static merchantClone.SaveFile;

namespace merchantClone
{
    public sealed class GameInfo
    {
        private static GameInfo instance = null;
        private static readonly object padlock = new object();
        private static SaveGame _saveFile;
        private static List<InventoryItem> _items;

        GameInfo()
        {
            _saveFile = SaveFile.Instance.GetSave();
        }

        public static GameInfo Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameInfo();
                    }
                    return instance;
                }
            }
        }
        internal static void InitializeInventory(List<InventoryItem> items)
        {
            //_items = items;
            _items = new List<InventoryItem>{
                new InventoryItem() { Id = 1, Quantity = 10},
                new InventoryItem() { Id = 2, Quantity = 2},
                new InventoryItem() { Id = 3, Quantity = 3},
                new InventoryItem() { Id = 4, Quantity = 1},
                new InventoryItem() { Id = 5, Quantity = 3},
                new InventoryItem() { Id = 6, Quantity = 1},
            };
        }

        public static List<InventoryItem> GetInventory()
        {
            return _items;
        }

        internal bool CanMake(Recipe recipe)
        {
            bool canBeMade = true;
            foreach (RecipeItem recipeItem in recipe.RecipeItems)
            {
                bool inInventory = false;
                foreach (InventoryItem inventoryItem in _items)
                {
                    if (inventoryItem.Item == recipeItem.Item && inventoryItem.Quantity >= recipeItem.Quantity)
                    {
                        inInventory = true;
                        break;
                    }
                }
                if (!inInventory)
                {
                    canBeMade = false;
                    break;
                }
            }
            return canBeMade;
        }

        internal static void InitializeGold(int gold)
        {
            _saveFile.gold = gold;
        }

        internal static int GetGold()
        {
            return _saveFile.gold;
        }

        internal void ReduceGold(int cost)
        {
            _saveFile.gold -= cost;
        }
        public SaveGame GetGameData()
        {
            return _saveFile;
        }

        internal static void InitializeTimers(List<Crafter> crafters)
        {
            DateTime now = DateTime.Now;
            foreach (Crafter crafter in crafters)
            {
                if (crafter.Task != null)
                {
                    crafter.Task.Seconds = (int)crafter.Task.FinishTime.Subtract(now).TotalSeconds;
                }
            }
        }
    }
}