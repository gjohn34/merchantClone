using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace merchantClone.Controls
{
    public class StaticLabel : Component, ILabel
    {
        #region Fields
        private SpriteFont _font;
        private Texture2D _texture;
        private Color _penColour = Color.Black;
        private Rectangle _rectangle;
        #endregion

        #region Properties
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                if (_rectangle != null)
                {
                    return _rectangle;
                }
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
            set
            {
                _rectangle = value;
            }
        }
        #endregion

        #region Methods
        public StaticLabel(Texture2D texture, SpriteFont font, string text)
        {
            _texture = texture;
            _font = font;
            Text = text;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), _penColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
        #endregion
    }
}