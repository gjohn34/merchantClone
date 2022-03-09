using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace merchantClone.Controls
{
    public class RecipeGroup : ComponentRow
    {
        #region Fields
        // TODO - Change this to private
        public Component[] _components;
        private Texture2D _comp;
        #endregion

        #region Properties
        public Vector2 Position;
        private Person _person;
        #endregion

        public RecipeGroup(Component[] components, Rectangle rectangle)
        {
            if (components.Length != 2)
            {
                throw new System.Exception();
            }
            _components = components;
            Rectangle = rectangle;
        }

        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            int hMargin = 25;
            int textureWidth = 143;
            int barHeight = 40;
            int index = 0;
            // button left
            _components[0].UpdatePosition(gametime, new Vector2(hMargin, position.Y));
            // progessbar
            // button right
            _components[1].UpdatePosition(gametime, new Vector2(Rectangle.Width - textureWidth - hMargin, position.Y));
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
    }
    public class PersonGroup : ComponentRow
    {

        #region Fields
        // TODO - Change this to private
        private Component[] _components;
        private Person _person;
        #nullable enable
        private Button? _left;
        private ProgressBar? _bar;
        private Button? _label;
        #nullable disable
        #endregion

        #region Properties
        public Vector2 Position;
        #endregion

        #region Methods
        public PersonGroup(Component[] components, Rectangle rectangle, Person person)
        {
            if (components.Length != 3)
            {
                throw new System.Exception();
            }
            _left = (Button)components[0];
            //_left = components[0] == null ? (Button)components[0] : null;
            _bar = (ProgressBar)components[1];
            _label = components[2] == null ? null : (Button)components[2];
            _components = components;
            _person = person;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Component component in _components)
                component.Draw(gametime, spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (_person.Task != null)
            {
                if (_person.Task.IsDone())
                {
                    _label.Text = "done";
                    _label.Touch += (object sender, EventArgs e) => { _person.FinishTask(); };
                } else
                {
                    _label.Text = _person.Task.SecondsLeft().ToString();
                }
            }
            _label.Update(gameTime);
            _left.Update(gameTime);
            _bar.Update(gameTime);
        }
        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            int hMargin = 25;
            int textureWidth = 143;
            int barHeight = 40;
            int index = 0;
            // button left
            _left.UpdatePosition(gametime, new Vector2(hMargin, position.Y));
            // progessbar
            _bar.UpdatePosition(gametime, new Vector2((int)(Rectangle.Width / 2 - (0.5 * _components[1].Rectangle.Width)), (int)((textureWidth - (0.5 * barHeight)) / 2) + position.Y));
            // button right
            _label.UpdatePosition(gametime, new Vector2(Rectangle.Width - textureWidth - hMargin, position.Y));
        }

        #endregion
    }
}