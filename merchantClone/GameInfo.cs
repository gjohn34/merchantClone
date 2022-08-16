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
        //private static List<InventoryItem> _items;
        private static List<Hero> _heroes;

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
            _saveFile.items = new List<InventoryItem>();

            foreach (InventoryItem item in items)
                _saveFile.items.Add(new InventoryItem(item.Id, item.Quantity));
        }

        internal void IncreaseInventory(List<RewardItem> rewardItems)
        {
            //Recipe recipe = ItemDetails.GetRecipe(recipeId);
            foreach (RewardItem rewardItem in rewardItems)
            {
                foreach (InventoryItem inventoryItem in _saveFile.items)
                {
                    if (inventoryItem.Item.Id == rewardItem.Id)
                    {
                        inventoryItem.Quantity += rewardItem.Quantity;
                        goto AlreadyAdded;
                    }
                }
                _saveFile.items.Add(new InventoryItem(rewardItem.Id, rewardItem.Quantity));
                AlreadyAdded: { }
            }
        }

        internal void ReduceInventory(List<RecipeItem> recipeItems)
        {
            foreach (RecipeItem recipeItem in recipeItems)
            {
                foreach (InventoryItem inventoryItem in _saveFile.items)
                {
                    if (inventoryItem.Item == recipeItem.Item)
                    {
                        inventoryItem.Quantity -= recipeItem.Quantity;
                        if (inventoryItem.Quantity <= 0)
                        {
                            _saveFile.items.Remove(inventoryItem);
                        }
                        break;
                    }
                }
            }
        }

        public static List<InventoryItem> GetInventory()
        {
            return _saveFile.items;
        }


        internal bool CanMake(Recipe recipe)
        {
            bool canBeMade = true;
            foreach (RecipeItem recipeItem in recipe.RecipeItems)
            {
                bool inInventory = false;
                foreach (InventoryItem inventoryItem in _saveFile.items)
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