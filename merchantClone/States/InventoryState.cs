using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended;
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
        private Texture2D _fontBackground;
        private int _vPadding;
        private TouchLocation _currentTouch;
        private TouchLocation _previousTouch;
        private bool _isPressed;
        private Rectangle _touchRectangle;
        private Rectangle _rectangle;


        #endregion
        #region Properties
        public Color PenColour { get; set; }
        public Rectangle TouchRectangle
        {
            get
            {
                if (_touchRectangle.IsEmpty)
                {
                    _touchRectangle = Rectangle;
                }
                return _touchRectangle;
            }
            set
            {
                _touchRectangle = value;
            }

        }
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
            spriteBatch.FillRectangle(new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height),Color.White * 0.3f);

            spriteBatch.Draw(_sprite, Rectangle, Color.White);

            if (_isPressed)
            {
                int width = (int)_font.MeasureString(Text).X + 20;
                int height = (int)_font.LineSpacing;
                int y = (Rectangle.Y + Rectangle.Height / 2);
                spriteBatch.FillRectangle(new RectangleF(Rectangle.X, y, width, height), new Color(255, 255, 255, 0.5f * 255));
                spriteBatch.DrawString(_font, Text, new Vector2(Rectangle.X, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousTouch = _currentTouch;
            _isPressed = false;
            _currentTouch = ControlSettings.GetTouchLocation();
            Rectangle rect = ControlSettings.GetTouchRectangle();

            if (rect.Intersects(TouchRectangle))
            {
                _isPressed = true;
            }
        }

        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
        }

    }
    public  class InventoryState : State
    {
        private List<ItemBox> _inventoryComponents = new List<ItemBox>();
        private Rectangle _backgroundRectangle;

        public InventoryState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _background = content.Load<Texture2D>("storeroom");
            _backgroundRectangle = new Rectangle(0, _buttonTexture.Height, _vW, _vH - 2 * _buttonTexture.Height);
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
                    new ItemBox(sprite, _buttonFont, vPadding) { 
                        Rectangle = new Rectangle(x, y, objWidth, objHeight), 
                        Text = item.Item.Name}
                );
                if (x < _vW - (objWidth + hPadding))
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
            spriteBatch.Draw(_background, _backgroundRectangle, Color.White);
            spriteBatch.FillRectangle(new RectangleF(0, _vH - _buttonTexture.Height, _vW, _buttonTexture.Height), Color.White);

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
            foreach (ItemBox itemBox in _inventoryComponents)
                itemBox.Update(gameTime);
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}