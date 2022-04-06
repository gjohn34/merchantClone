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
            _items = new List<InventoryItem>();
            //_items = items;
            foreach (InventoryItem item in items)
                _items.Add(new InventoryItem(item.Id, item.Quantity));
            //_items = new List<InventoryItem>{
            //    new InventoryItem() { Id = 1, Quantity = 100},
            //    new InventoryItem() { Id = 2, Quantity = 100},
            //    new InventoryItem() { Id = 3, Quantity = 100},
            //    new InventoryItem() { Id = 4, Quantity = 100},
            //    new InventoryItem() { Id = 5, Quantity = 100},
            //    new InventoryItem() { Id = 6, Quantity = 1},
            //};
        }

        internal void IncreaseInventory(List<RewardItem> rewardItems)
        {
            //Recipe recipe = ItemDetails.GetRecipe(recipeId);
            foreach (RewardItem rewardItem in rewardItems)
            {
                foreach (InventoryItem inventoryItem in _items)
                {
                    if (inventoryItem.Item.Id == rewardItem.Id)
                    {
                        inventoryItem.Quantity += rewardItem.Quantity;
                        break;
                    }
                }
                _items.Add(new InventoryItem(rewardItem.Id, rewardItem.Quantity));
            }
        }

        internal void ReduceInventory(List<RecipeItem> recipeItems)
        {
            foreach (RecipeItem recipeItem in recipeItems)
            {
                foreach (InventoryItem inventoryItem in _items)
                {
                    if (inventoryItem.Item == recipeItem.Item)
                    {
                        inventoryItem.Quantity -= recipeItem.Quantity;
                        break;
                    }
                }
            }
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

        public void ResetData(SaveGame saveData)
        {
            _saveFile = saveData;
        }
        // TODO - Add Person
         internal static void InitializeTimers(List<Person> people)
        {
            DateTime now = DateTime.Now;
            foreach (Person person in people)
            {
                if (person.Job != null)
                {
                    person.Job.Seconds = (int)person.Job.FinishTime.Subtract(now).TotalSeconds;
                }
            }
        }

    }
}