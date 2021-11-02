using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using static merchantClone.SaveFile;

namespace merchantClone.Controls
{
    public enum DataValue
    {
        Gold,
        Hero
    }
    public class DynamicLabel : Component, ILabel
    {
        #region Fields
        private SpriteFont _font;
        private Texture2D _texture;
        private Color _penColour = Color.Black;
        private DataValue _type;
        #endregion

        #region Properties
        public string Text { get; set; }
        public bool Changed { get; set; } = false;
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        #endregion

        #region Methods
        public DynamicLabel(Texture2D texture, SpriteFont font, string text, DataValue type)
        {
            _texture = texture;
            _font = font;
            Text = text;
            _type = type;
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
            if (Changed == true)
            {
                Changed = false; 
                SaveGame saveGame = SaveFile.Instance.GetSave();
                switch (_type)
                {
                    case DataValue.Gold:
                        Text = saveGame.gold.ToString();
                        break;
                    case DataValue.Hero:
                        Text = "not implemented";
                        break;
                    default:
                        break;
                }

            }
        }
        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            Position = position;
        }

        #endregion
    }
}