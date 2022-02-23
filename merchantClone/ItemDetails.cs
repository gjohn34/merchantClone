using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using static merchantClone.SaveFile;

namespace merchantClone
{
    public sealed class ItemDetails
    {
        private static ItemDetails instance = null;
        private static readonly object padlock = new object();
        ItemDetails()
        {
            _itemList = new List<Item>();
            _recipeList = new List<Recipe>();
            // TODO Move save from control settings to save file and make singleton
            //_saveGame = Load();
            //_itemList = 
        }

        public static ItemDetails Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ItemDetails();
                    }
                    return instance;
                }
            }
        }
        private static List<Item> _itemList;
        private static List<Recipe> _recipeList;

        public static List<Item> GetItems()
        {
            return _itemList;
        }

        public static List<Recipe> GetRecipes()
        {
            return _recipeList;
        }
        public static Recipe GetRecipe(int id)
        {
            return _recipeList.Find(item => item.Id == id);

        }
        public static Item GetItem(int id)
        {
            return _itemList.Find(item => item.Id == id);
        }
        public void LoadContent(List<Item> items, List<Recipe> recipes)
        {
            _itemList = items;
            _recipeList = recipes;
        }
    }
}