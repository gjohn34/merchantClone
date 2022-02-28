using merchantClone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace merchantClone
{
    public sealed class GameInfo
    {
        private static GameInfo instance = null;
        private static readonly object padlock = new object();
        private static List<InventoryItem> _items;

        GameInfo()
        {
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
                new InventoryItem() { Id = 2, Quantity = 2},
                new InventoryItem() { Id = 3, Quantity = 3},
                new InventoryItem() { Id = 1, Quantity = 10}
            };
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
    }
}