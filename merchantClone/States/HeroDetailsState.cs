using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class HeroDetailsState : State
    {
        private Rectangle _backgroundRectangle;

        public HeroDetailsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Hero hero) : base(game, graphicsDevice, content)
        {
            #region Buttons

            var backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back"
            };
            backButton.Touch += BackButton_Click;
            #endregion
            #region Labels
            StaticLabel title = new StaticLabel(_buttonTexture, _buttonFont, $"{hero.Name} the {hero.Role}\nLevel: {hero.Level}")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, _vW, _buttonTexture.Height)
            };

            _components = new List<Component>
            {
                backButton,
                title,

            };

            #region Other
            int index = 0;
            string[] text = new string[]
            {
                 "HP: " + hero.CurrentHp + "/" + hero.MaxHp,
                 "Exp: " + hero.CurrentXp + "/" + hero.TotalXp,
            };
            foreach (string s in text)
            {
                _components.Add(new StaticLabel(_buttonTexture, _buttonFont, s)
                {
                    Position = new Vector2(0, 0),
                    Rectangle = new Rectangle(100, (index * _buttonFont.LineSpacing) + (int)(0.5 * _vH) + 100, 250, 50)
                });
                index += 1;
            };

            index = 0;
            text = new string[]
            {
                 "Strength: ", hero.Strength.ToString(),
                 "Intelligence: ", hero.Intelligence.ToString(),
                 "Dexterity: ", hero.Dexterity.ToString()
            };
            index = text.Length;
            foreach (string s in text)
            {
                _components.Add(new StaticLabel(_buttonTexture, _buttonFont, s)
                {
                    Position = new Vector2(0, 0),
                    Rectangle = new Rectangle(100, (index * _buttonFont.LineSpacing) + (int)(0.5 * _vH) + 100, 250, 50)
                });
                index += 1;
            };
            #endregion


            #endregion
            _background = content.Load<Texture2D>("barracks");
            _backgroundRectangle = new Rectangle(0, _buttonTexture.Height, _vW, _vH - (2 * _buttonTexture.Height));
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HeroesMenuState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_background, _backgroundRectangle, Color.White);
            spriteBatch.FillRectangle(new RectangleF(0, _vH - _buttonTexture.Height, _vW, _buttonTexture.Height), Color.White);

            // Top
            //spriteBatch.Draw(_buttonTexture, new Rectangle(0, 0, _vW, _vH - 100 - _buttonTexture.Height), Color.White);
            // Bottom
            spriteBatch.Draw(_buttonTexture, new Rectangle(0, (int)(0.5 * _vH), _vW, (int)(0.5 * _vH) - _buttonTexture.Height), Color.White);
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}