using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class ShowRecipeState : State
    {
        private Recipe _recipe;
        private int _vW;
        private int _vH;
        private Button _backButton;
        private State _backState;
        private List<StaticLabel> _recipeComponents = new List<StaticLabel>();
        private Button _createButton;

        public ShowRecipeState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Recipe recipe, State state) : base(game, graphicsDevice, content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            _recipe = recipe;
            _vW = _graphicsDevice.Viewport.Width;
            _vH = _graphicsDevice.Viewport.Height;
            _backState = state;
            _backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            _backButton.Touch += BackButton_Click;
            int baseHeight = 50 + (int)(0.5 * _vH);
            _createButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2((int)(0.5 * _vW - (0.5 * _buttonTexture.Width)), baseHeight + 500),
                Text = "Create",
                Disabled = false
            };
            _createButton.Disabled = !GameInfo.Instance.CanMake(recipe);
            _createButton.Touch += CreateButton_Click;
            int i = 0;
            foreach (RecipeItem recipeItem in recipe.RecipeItems)
            {
                Rectangle rectangle = new Rectangle(50,baseHeight,(int)(_vW * 0.5) -50 ,200);
                switch (i)
                {
                    case 1:
                        rectangle.X = (int)(_vW * 0.5);
                        break;
                    case 2:
                        rectangle.Y += 200;
                        break;
                    case 3:
                        rectangle.X = (int)(_vW * 0.5);
                        rectangle.Y += 200;
                        break;
                    default:
                        break;
                }
                _recipeComponents.Add(new StaticLabel(_buttonTexture, _buttonFont, recipeItem.Item.Name + " * " + recipeItem.Quantity ) { Rectangle = rectangle });
                i++;
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Top
            spriteBatch.Draw(_buttonTexture, new Rectangle(0,0, _vW, _vH - 100 - _backButton.Rectangle.Height), Color.White);
            // Bottom
            spriteBatch.Draw(_buttonTexture, new Rectangle(0,(int)(0.5 * _vH), _vW, (int)(0.5 * _vH) - _backButton.Rectangle.Height), Color.White);
            _backButton.Draw(gameTime, spriteBatch);
            _createButton.Draw(gameTime, spriteBatch);
            foreach (StaticLabel staticLabel in _recipeComponents)
                staticLabel.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(_buttonFont, _recipe.Name, new Vector2((int)(_vW * 0.5), 200), Color.White);
            spriteBatch.End();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(_backState);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            //_game.ChangeState(_backState);
            //_createButton.Disabled = !_createButton.Disabled;
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _backButton.Update(gameTime);
            _createButton.Update(gameTime);
        }
    }
}