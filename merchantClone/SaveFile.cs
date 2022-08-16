using merchantClone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Serialization;

namespace merchantClone
{
    public sealed class SaveFile
    {
        private static SaveFile instance = null;
        private static readonly object padlock = new object();
        private static SaveGame _saveData;

        [Serializable]
        public struct SaveGame
        {
            public int gold;
            public string playerName;
            public List<Crafter> crafters;
            public List<Hero> heroes;
            public List<InventoryItem> items;
        }

        SaveFile()
        {
            _saveData = Load();
        }

        public static SaveFile Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SaveFile();
                    }
                    return instance;
                }
            }
        }
        private static IsolatedStorageFileStream _isolatedFileStream;
        private static IsolatedStorageFile _dataFile;

        public SaveGame GetSave()
        {
            return _saveData;
        }


        public static SaveGame Load()
        {
            List<Person> f = new List<Person>();

            _dataFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
            SaveGame saveData = new SaveGame();
            if (!_dataFile.FileExists("file.sav"))
            {
                Debug.WriteLine("File not found - start new game");
            }
            try
            {
                using (_isolatedFileStream = _dataFile.OpenFile("file.sav", FileMode.Open, FileAccess.ReadWrite))
                {
                    saveData = (SaveGame)serializer.Deserialize(_isolatedFileStream);
                    // Loop through nested Lists
                    _dataFile.Close();
                    _isolatedFileStream.Close();
                }
                foreach (Crafter crafter in saveData.crafters)
                {
                    if (crafter.Job != null)
                        crafter.Job.Task = ItemDetails.GetRecipe(crafter.Job.TaskId);
                    
                    crafter.StartJobsList();
                }
                foreach (Hero hero in saveData.heroes)
                {
                    if (hero.Job != null)
                        hero.Job.Task = Quest.GetQuest(hero.Job.TaskId);
                }
            } catch
            {
                saveData.gold = 100;
                saveData.playerName = "new player";
                saveData.crafters = new List<Crafter>();
                saveData.heroes = new List<Hero>();
                saveData.items = new List<InventoryItem>();
            }
            GameInfo.InitializeInventory(saveData.items);
            GameInfo.InitializeGold(saveData.gold);
            //GameInfo.InitializeTimers(f);
            return saveData;
        }

        public static void Reset()
        {
            _dataFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
            if (_dataFile.FileExists("file.sav"))
            {
                Debug.WriteLine("File found - deleting it");
                _dataFile.DeleteFile("file.sav");
            }
            _dataFile.Close();
        }

        public static void Save()
        {
            _dataFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
            if (_dataFile.FileExists("file.sav"))
            {
                Debug.WriteLine("File found - deleting it");
                _dataFile.DeleteFile("file.sav");
            }
            using (_isolatedFileStream = _dataFile.CreateFile("file.sav"))
            {
                _isolatedFileStream.Seek(0, SeekOrigin.Begin);

                SaveGame gameData = GameInfo.Instance.GetGameData();

                serializer.Serialize(_isolatedFileStream, gameData);

                _isolatedFileStream.SetLength(_isolatedFileStream.Position);
            }
            _dataFile.Close();
            _isolatedFileStream.Dispose();
        }
    }
}