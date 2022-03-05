using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class ItemBox : Component
    {
        #region Fields
        private Texture2D _sprite;
        private SpriteFont _font;
        private Texture2D _borderTexture;
        private int _vPadding;
        #endregion
        #region Properties
        public string Text { get; set; }
        public Color PenColour { get; set; }
        #endregion
        public ItemBox(Texture2D sprite, SpriteFont font, int vPadding)
        {
            _sprite = sprite;
            _font = font;
            _vPadding = vPadding;   
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (_borderTexture == null)
            {
                _borderTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _borderTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Draw(_sprite, Rectangle, colour);


            // top left to left bottom
            spriteBatch.Draw(_borderTexture, new Rectangle(Rectangle.X, Rectangle.Y, 1, Rectangle.Height + (int)(0.5 * _vPadding)), Color.Black);
            
            // top left to top right
            spriteBatch.Draw(_borderTexture, new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width + 1, 1), Color.Black);
            
            // top right to bottom right
            spriteBatch.Draw(_borderTexture, new Rectangle(Rectangle.X + Rectangle.Width, Rectangle.Y, 1, Rectangle.Height + (int)(0.5 * _vPadding)), Color.Black);
            
            //bottom left to bottom right
            spriteBatch.Draw(_borderTexture, new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height + (int)(0.5 * _vPadding), Rectangle.Width + 1, 1), Color.Black);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + Rectangle.Width / 2) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + Rectangle.Height + 25) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
        }

    }
    public  class InventoryState : State
    {
        private List<ItemBox> _inventoryComponents = new List<ItemBox>();
        public InventoryState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

            int x = 20;
            int y = _buttonTexture.Height + 50;
            int objWidth = (int)Math.Ceiling((decimal)_vW / 5);
            int objHeight = (int)Math.Ceiling((decimal)_vH / 10);
            int remainingHSpace = _vW - (objWidth * 4);
            int remainingVSpace = _vH - (y * 2) - (objHeight * 7);
            int hPadding = (int)Math.Ceiling((decimal)remainingHSpace / 4);
            int vPadding = (int)Math.Ceiling((decimal)remainingVSpace / 7);

            List<InventoryItem> inv = GameInfo.GetInventory();
            foreach (InventoryItem item in inv)
            {
                string path = "sprites/" + item.Item.Sprite;
                Texture2D sprite = content.Load<Texture2D>(path);

                _inventoryComponents.Add(
                    new ItemBox(sprite, _buttonFont, vPadding) { Rectangle = new Rectangle(x, y, objWidth, objHeight), Text = item.Quantity.ToString() }
                );
                if (x < graphicsDevice.Viewport.Width - (objWidth + hPadding))
                {
                    x += objWidth + hPadding;
                } else
                {
                    x = 20;
                    y += (objHeight + vPadding + 50);
                }
            }
            StaticLabel goldLabel = new StaticLabel(_buttonTexture, _buttonFont, GameInfo.GetGold().ToString())
            { Position = new Vector2((int)(_vW * 0.5 - _buttonTexture.Width * 0.5), _vH - _buttonTexture.Height )};

            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            backButton.Touch += BackButton_Click;
            StaticLabel title = new StaticLabel(_buttonTexture, _buttonFont, "Inventory")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, _vW, _buttonTexture.Height)
            };
            _components = new List<Component>
            {
                backButton, title, goldLabel
            };

        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.DrawString(_buttonFont, "hello", Vector2.Zero, Color.White);
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
            foreach (ItemBox component in _inventoryComponents)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}