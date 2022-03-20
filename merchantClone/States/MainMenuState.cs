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
using static merchantClone.SaveFile;
using merchantClone.Models;
using merchantClone.Data;

namespace merchantClone.States
{
    public class MainMenuState : State
    {
        private List<DynamicLabel> _labels;
        // TODO - Move load into game1, pass gold around


        public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("controls/button_background2");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/font");
            Button craftingButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0, graphicsDevice.Viewport.Height-buttonTexture.Height),
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
            Button goldButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(viewport.Width - (buttonTexture.Width * 3), viewport.Height - buttonTexture.Height),
                Text = "Add Gold"
            };
            Button resetButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(viewport.Width - (buttonTexture.Width * 2), viewport.Height - buttonTexture.Height),
                Text = "reset"
            };

            Button inventoryButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(buttonTexture.Width * 3, viewport.Height - buttonTexture.Height),
                Text = "inventory"
            };
            StaticLabel goldLabel = new StaticLabel(buttonTexture, buttonFont, _saveData.gold.ToString())
            {
                Position = new Vector2((viewport.Width / 2) - (buttonTexture.Width / 2)),
                Text = _saveData.gold.ToString()
            };

            craftingButton.Touch += CraftingButton_Click;
            heroesButton.Touch += HeroesButton_Click;
            saveButton.Touch += SaveButton_Click;
            //goldButton.Touch += GoldButton_Click;
            resetButton.Touch += ResetButton_Click;
            inventoryButton.Touch += InventoryButton_Click;

            StaticLabel title = new StaticLabel(buttonTexture, buttonFont, "Main")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, graphicsDevice.Viewport.Width, buttonTexture.Height)
            };
            _components = new List<Component>
            {
                craftingButton,
                heroesButton,
                saveButton,
                //goldButton,
                resetButton,
                inventoryButton,
                title,
                goldLabel
            };

            //_labels = new List<S>
            //{
            //    goldLabel
            //};


        }

        //private void GoldButton_Click(object sender, EventArgs e)
        //{
        //    _saveData.gold += 1;
        //    foreach (DynamicLabel label in _labels)
        //    {
        //        label.Changed = true;
        //    }
        //    SaveFile.UpdateSaveData(_saveData);
        //}
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFile.Save();
        }
        private void ResetButton_Click(object sender, EventArgs e)
        {
            SaveFile.Reset();
            GameInfo.Instance.ResetData(SaveFile.Load());

        }
        private void InventoryButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new InventoryState(_game, _graphicsDevice, _content));
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

            spriteBatch.Begin();
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //_goldLabel.Text = _saveData.gold.ToString();
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}