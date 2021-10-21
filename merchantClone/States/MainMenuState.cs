using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using merchantClone.Controls;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace merchantClone.States
{
    /// <summary>
    /// int gold
    /// string playerName
    /// List<string> heroNames
    /// </summary>
    [Serializable]
    public struct SaveGame
    {
        public int gold;
        public string playerName;
        public List<string> heroNames;
    }

    public class MainMenuState : State
    {
        private List<Component> _components;
        private Button _goldLabel;
        SaveGame SaveData = new SaveGame()
        {
            gold = 1,
            playerName = "john",
        };
        public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("controls/button_background");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/font");
            Button craftingButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "crafting"
            };
            Button heroesButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(600, 600),
                Text = "heroes"
            };
            Viewport viewport = graphicsDevice.Viewport;
            Button saveButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(viewport.Width - buttonTexture.Width, viewport.Height - buttonTexture.Height),
                Text = "SAVE"
            };
            Button loadButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(viewport.Width - (buttonTexture.Width * 2), viewport.Height - buttonTexture.Height),
                Text = "LOAD"
            };
            Button deleteButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(viewport.Width - (buttonTexture.Width * 3), viewport.Height - buttonTexture.Height),
                Text = "DELETE"
            };
            _goldLabel = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0, 0),
                Text = 0.ToString()
            };

            craftingButton.Touch += CraftingButton_Click;
            heroesButton.Touch += HeroesButton_Click;
            saveButton.Touch += SaveButton_Click;
            loadButton.Touch += LoadButton_Click;
            deleteButton.Touch += DeleteButton_Click;

            _components = new List<Component>
            {
                craftingButton,
                heroesButton,
                saveButton,
                loadButton,
                deleteButton,
                _goldLabel,
            };
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var dataFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null);
            if (dataFile.FileExists("file.sav"))
            {
                dataFile.DeleteFile("file.sav");
            }
            dataFile.Close();
            dataFile.Dispose();
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            var dataFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
            IsolatedStorageFileStream isolatedFileStream;
            if (!dataFile.FileExists("file.sav"))
            {
                Debug.WriteLine("File not found - start new game");
                return;
            }
            using (isolatedFileStream = dataFile.OpenFile("file.sav", FileMode.Open, FileAccess.ReadWrite))
            {
                // Store the deserialized data object.
                SaveGame SaveData = (SaveGame)serializer.Deserialize(isolatedFileStream);

                //Extract the save data
                _goldLabel.Text = SaveData.gold.ToString();
                //Loop through SaveData.ownedSoccerBalls and use the wrapper data to recreate the player's ownedSoccerBalls
            }
            dataFile.Close();
            isolatedFileStream.Close();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var dataFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
            IsolatedStorageFileStream isolatedFileStream;
            if (dataFile.FileExists("file.sav"))
            {
                Debug.WriteLine("File found - deleting it");
                dataFile.DeleteFile("file.sav");
            }
            using (isolatedFileStream = dataFile.CreateFile("file.sav"))
            {
                isolatedFileStream.Seek(0, SeekOrigin.Begin);

                serializer.Serialize(isolatedFileStream, SaveData);

                isolatedFileStream.SetLength(isolatedFileStream.Position);
            }
            dataFile.Close();
            isolatedFileStream.Dispose();
        }
        private void CraftingButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CraftingMenuState(_game, _graphicsDevice, _content));
        }
        private void HeroesButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HeroesMenuState(_game, _graphicsDevice, _content));
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}