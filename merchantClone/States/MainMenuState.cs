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
using MonoGame.Extended;
using merchantClone.Helpers;

namespace merchantClone.States
{
    public class MainMenuState : State
    {
        private List<DynamicLabel> _labels;
        private Rectangle _backgroundRectangle;
        private string _text;
        private int _textHeight;
        private RectangleF _modalBackground;
        private bool _help = false;


        // TODO - Move load into game1, pass gold around


        public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = content.Load<Texture2D>("controls/button_background2");
            SpriteFont buttonFont = content.Load<SpriteFont>("Fonts/font");
            _background = content.Load<Texture2D>("background");
            _backgroundRectangle = new Rectangle(0, 0, _vW, _vH - _buttonTexture.Height);
            Button craftingButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0, _vH - buttonTexture.Height),
                Text = "Craft"
            };
            Button heroesButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(_vW - buttonTexture.Width, _vH - buttonTexture.Height),
                Text = "Quest"
            };
            #region DEBUG
            // Debug
            //Button saveButton = new Button(buttonTexture, buttonFont)
            //{
            //    Position = new Vector2(_vW - buttonTexture.Width, _vH - buttonTexture.Height),
            //    Text = "SAVE"
            //};
            //Button goldButton = new Button(buttonTexture, buttonFont)
            //{
            //    Position = new Vector2(_vW - (buttonTexture.Width * 3), _vH - buttonTexture.Height),
            //    Text = "Add Gold"
            //};
            //StaticLabel goldLabel = new StaticLabel(buttonTexture, buttonFont, _saveData.gold.ToString())
            //{
            //    Position = new Vector2(0.5f * _vW - 0.5f * buttonTexture.Width),
            //    Text = _saveData.gold.ToString()
            //};
            #endregion
            Button resetButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(_vW - (buttonTexture.Width * 2), _vH - buttonTexture.Height),
                Text = "reset"
            };

            Button inventoryButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0.5f * _vW - 0.5f * buttonTexture.Width, _vH - buttonTexture.Height),
                Text = "Items"
            };

            Button helpButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(buttonTexture.Width, _vH - buttonTexture.Height),
                Text = "Help"
            };

            craftingButton.Touch += (a,b) => _game.ChangeState(new CraftingMenuState(_game, _graphicsDevice, _content));
            heroesButton.Touch += (a,b) => _game.ChangeState(new HeroesMenuState(_game, _graphicsDevice, _content));
            //saveButton.Touch += (a,b) => SaveFile.Save();
            //goldButton.Touch += GoldButton_Click;
            resetButton.Touch += ResetButton_Click;
            inventoryButton.Touch += (a,b) => _game.ChangeState(new InventoryState(_game, _graphicsDevice, _content));
            helpButton.Touch += (a,b) => _help = !_help;

            StaticLabel title = new StaticLabel(buttonTexture, buttonFont, "Main")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, _vW, buttonTexture.Height)
            };
            _components = new List<Component>
            {
                craftingButton,
                heroesButton,
                //saveButton,
                //goldButton,
                resetButton,
                inventoryButton,
                title,
                helpButton
                //goldLabel
            };
            _backgroundRectangle = new Rectangle(0, _buttonTexture.Height, _vW, _vH - 2 * _buttonTexture.Height);
            _text = TextWrapper.WrapText("Yo!\nRight now, you have no items, no crafters and no heroes. But you do have gold.\nStart by touching 'Quest' and hiring a hero. Send that hero out on a quest. They'll bring back items which can be seen by touching the 'Items' button.\nThose items are used to craft equipment which can be equipped to your heroes (NOT IMPLEMENTED) to send them out on stronger quests (NOT IMPLEMENTED)\nFor now, its super basic and timers are static but will gradually increase the further you go (NOT IMPLEMENTED)", 0.5f * _vW, _buttonFont);
            _textHeight = (int)_buttonFont.MeasureString(_text).Y;
            _modalBackground = new RectangleF(
                    0.2f * _vW,
                    0.2f * _vH,
                    0.6f * _vW,
                    0.5f * _vH
            );
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            SaveFile.Reset();
            GameInfo.Instance.ResetData(SaveFile.Load());

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(_background, _backgroundRectangle, Color.White);
            if (_help)
            {
            spriteBatch.FillRectangle(_modalBackground, Color.Black * 0.5f);
            spriteBatch.DrawString(_buttonFont, _text, new Vector2(_modalBackground.X + 0.1f * _modalBackground.Width, _modalBackground.Y + 0.1f * _modalBackground.Height), Color.White);

            }

            spriteBatch.FillRectangle(new RectangleF(0,_vH - _buttonTexture.Height,_vW,_buttonTexture.Height), Color.White);

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