﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace merchantClone.States
{
    public class CraftingMenuState : State
    {
        private List<Component> _components;
        public CraftingMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            var buttonTexture = _content.Load<Texture2D>("controls/button_background");
            var buttonFont = _content.Load<SpriteFont>("Fonts/font");

            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}