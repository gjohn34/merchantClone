using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using merchantClone.Models;
using System.Linq;

namespace merchantClone.States
{
    public class CraftingMenuState : State
    {
        private List<Component> _components;
        private List<Person> _people;
        private Texture2D _texture;
        private SpriteFont _font;
        private int _margin;
        private int _deviceWidth;

        public CraftingMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            _margin = (int)(0.05 * graphics.Viewport.Height);
            _deviceWidth = graphics.Viewport.Width;
            _texture = _content.Load<Texture2D>("controls/button_background2");
            _font = _content.Load<SpriteFont>("Fonts/font");
            int rowHeight = _texture.Height;
            int margin = (int)(0.05 * graphics.Viewport.Height);
            _people = new List<Person>
            {

            };

            var backButton = new Button(_texture, _font)
            {
                Position = new Vector2(0, graphics.Viewport.Y),
                Text = "Back",
            };
            Button newPerson = new Button(_texture, _font)
            {
                Position = new Vector2(0, Person.GeneratePosition(_people, margin, rowHeight) + _texture.Height),
                Text = "New"
            };

            StaticLabel title = new StaticLabel(_texture, _font, "Crafters")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, graphics.Viewport.Width, _texture.Height)
            };

            backButton.Touch += BackButton_Click;
            newPerson.Touch += NewPerson_Click;

            _components = new List<Component>
            {
                backButton,
                title,
                newPerson
            };
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }
        private void NewPerson_Click(object sender, EventArgs e)
        {
            _people.Add(new Person(PersonType.Crafter, "John"));
            Component[] cc = new Component[_components.Count];
            _components.CopyTo(cc);
            List<Component> t = cc.ToList();

            t.Add(new ComponentGroup(_texture, new List<Component>(), new Rectangle(0, 400, _deviceWidth, 200)));
            _components = t;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}