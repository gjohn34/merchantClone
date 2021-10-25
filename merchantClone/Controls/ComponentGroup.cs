using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace merchantClone.Controls
{
    public class ComponentGroup : Component
    {
        private List<Component> _components;

        #region Fields
        private Texture2D _texture;
        private Rectangle _rectangle;
        private Vector2 _position;

        #endregion

        #region Properties
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
        public ComponentGroup(Texture2D texture, List<Component> components, Rectangle rectangle)
        {
            _texture = texture;
            _components = components;
            _rectangle = rectangle;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);
            foreach (Component component in _components)
                component.Draw(gametime, spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
        #endregion
    }
}