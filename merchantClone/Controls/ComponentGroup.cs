using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace merchantClone.Controls
{
    public class ComponentGroup : Component
    {

        #region Fields
        // TODO - Change this to private
        public Component[] _components;
        private int _compHeight;
        private Texture2D _comp;
        #endregion

        #region Properties
        public Vector2 Position;
        public Rectangle Rectangle;
        #endregion

        #region Methods
        public ComponentGroup(Component[] components)
        {
            if (components.Length != 3) 
            {
                throw new System.Exception();
            }

            _components = components;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Component component in _components)
                component.Draw(gametime, spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            _components[0].UpdatePosition(gametime, new Vector2(0, position.Y));
            _components[1].UpdatePosition(gametime, new Vector2(200, position.Y));
            _components[2].UpdatePosition(gametime, new Vector2(400, position.Y));
        }

        #endregion
    }
}