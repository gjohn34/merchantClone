using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace merchantClone.Controls
{
    public class ComponentGroup
    {

        #region Fields
        #endregion

        #region Properties
        private Component[] _components;
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

        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Component component in _components)
                component.Draw(gametime, spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
        #endregion
    }
}