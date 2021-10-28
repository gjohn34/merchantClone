using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
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

        public static void UpdateSaveData(SaveGame saveData)
        {
            _saveData = saveData;
            Save();
        }

        //internal static void UpdateSave(SaveGame saveGame)
        //{
        //    _saveGame = saveGame;
        //}


        public static SaveGame Load()
        {
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
            } catch
            {
                saveData.gold = 100;
                saveData.playerName = "new player";
            }
            return saveData;
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

                serializer.Serialize(_isolatedFileStream, _saveData);

                _isolatedFileStream.SetLength(_isolatedFileStream.Position);
            }
            _dataFile.Close();
            _isolatedFileStream.Dispose();
        }
    }
}