﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;


namespace merchantClone.Controls
{
    public class Button : Component, ILabel
    {
        #region Fields
        private TouchLocation _currentTouch;
        private TouchLocation _previousTouch;
        private bool _isPressed;
        private SpriteFont _font;
        private Texture2D _texture;
        private int _tick;
        #endregion

        #region Properties
        public int Id { get; set; }
        public event EventHandler Touch;
        public bool Disabled;
        public bool Clicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        private Rectangle _touchRectangle;
        private Rectangle _rectangle;

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
        public new Rectangle Rectangle
        {
            get
            {
                if (_rectangle.IsEmpty)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
                }
                return _rectangle;
            }
            set
            {
                _rectangle = value;
            }
        }   

        public string Text { get; set; }
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font, EventHandler e = null)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
            Disabled = false;
            if (e != null)
            {
                Touch = e;
            }
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (_isPressed)
            {
                colour = Color.Gray;
            }
            if (Disabled)
            {
                colour = Color.Black;
            }
            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Disabled) return;
            _previousTouch = _currentTouch;
            _isPressed = false;
            _currentTouch = ControlSettings.GetTouchLocation();
            Rectangle rect = ControlSettings.GetTouchRectangle();
            if (rect.Intersects(TouchRectangle))
            {
                _isPressed = true;
                if (
                    (_currentTouch.State == TouchLocationState.Released) 
                    && (_previousTouch.State == TouchLocationState.Pressed || _previousTouch.State == TouchLocationState.Moved))
                {
                    Touch?.Invoke(this, new EventArgs());
                }
            }
        }

        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            Position = position;
            TouchRectangle = new Rectangle((int)position.X, (int)position.Y + _texture.Height, _texture.Height, _texture.Width);
        }
        #endregion

    }
}