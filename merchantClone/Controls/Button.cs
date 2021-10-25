using Microsoft.Xna.Framework;
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
        #endregion

        #region Properties
        public int Id { get; set; }
        public Rectangle TouchArea { get; set; }
        public event EventHandler Touch;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {

            var colour = Color.White;
            if (_isPressed)
            {
                colour = Color.Gray;
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
            _previousTouch = _currentTouch;
            _isPressed = false;
            _currentTouch = ControlSettings.GetTouchLocation();
            if (ControlSettings.GetTouchRectangle().Intersects(Rectangle))
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
        #endregion

    }
}