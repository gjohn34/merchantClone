using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class HeroesMenuState : State
    {
        private List<Component> _components;
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
        public HeroesMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {

            var buttonTexture = _content.Load<Texture2D>("controls/button_background");
            var buttonFont = _content.Load<SpriteFont>("Fonts/font");

            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0, graphics.Viewport.Y),
                Text = "Back"
            };

            backButton.Touch += BackButton_Click;

            _components = new List<Component>
            {
                backButton
            };

        }
        public void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }

    }
}