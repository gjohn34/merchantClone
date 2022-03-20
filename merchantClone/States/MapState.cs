    using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Button = merchantClone.Controls.Button;

namespace merchantClone.States
{
    public class MapState : State
    {
        private Texture2D _mapBackground;
        private ScrollPane _scrollPane;
        private List<ComponentRow> _scrollComponents = new List<ComponentRow>();


        public MapState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            #region Buttons
            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            backButton.Touch += BackButton_Click;

            _components = new List<Component>
            {
                backButton,
            };
            #endregion
            _mapBackground = content.Load<Texture2D>("sprites/maps/map1");
            Rectangle r = new Rectangle(0, 0 - _vH * 2, _vW, _vH * 3);
            _scrollPane = new ScrollPane(
                game: game,
                components: _scrollComponents,
                rectangle: r,
                texture: _buttonTexture,
                transparent: true
                );
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HeroesMenuState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_mapBackground, new Rectangle(0, 0 - _vH * 2, _vW, _vH * 3), Color.White);
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            _scrollPane.Draw(gameTime, spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
            _scrollPane.Update(gameTime, _scrollComponents);
        }
    }
}