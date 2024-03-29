﻿using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class ShowRecipeState : State
    {
        private Recipe _recipe;
        private List<StaticLabel> _recipeComponents = new List<StaticLabel>();
        private Crafter _crafter;
        private Rectangle _backgroundRectangle;

        public ShowRecipeState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Recipe recipe, State state, Crafter crafter) : base(game, graphicsDevice, content)
        {
            _background = content.Load<Texture2D>("forge");

            _backgroundRectangle = new Rectangle(0, _buttonTexture.Height, _vW, _vH - (2 * _buttonTexture.Height));
            int baseHeight = 50 + (int)(0.5 * _vH);
            _recipe = recipe;
            _crafter = crafter;

            // Components
            // Buttons
            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            backButton.Touch += BackButton_Click;

            Button createButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(_vW - _buttonTexture.Width, _vH - _buttonTexture.Height),
                Text = "Create",
                Disabled = false
            };
            if (!GameInfo.Instance.CanMake(recipe) || crafter.Job != null)
            {
                createButton.Disabled = true;
                createButton.PenColour = Color.White;
                createButton.Text = "Not\nEnough\nItems";
            }
            createButton.Touch += CreateButton_Click;

            // Labels
            StaticLabel goldLabel = new StaticLabel(_buttonTexture, _buttonFont, GameInfo.GetGold().ToString())
            { Position = new Vector2((int)(_vW * 0.5 - _buttonTexture.Width * 0.5), _vH - _buttonTexture.Height) };

            StaticLabel title = new StaticLabel(_buttonTexture, _buttonFont, recipe.Name)
            {
                //Position = new Vector2(0,0),
                Rectangle = new Rectangle(0, 0, _vW, _buttonTexture.Height)
            };


            _components = new List<Component>
            {
                backButton,
                createButton,
                goldLabel,
                title
            };
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
            spriteBatch.Draw(_background, _backgroundRectangle, Color.White);

            spriteBatch.FillRectangle(new RectangleF(0, _vH - _buttonTexture.Height, _vW, _buttonTexture.Height), Color.White);

            // Top
            //spriteBatch.Draw(_buttonTexture, new Rectangle(0, 0, _vW, _vH - 100 - _buttonTexture.Height), Color.White);
            // Bottom
            spriteBatch.Draw(_buttonTexture, new Rectangle(0, (int)(0.5 * _vH), _vW, (int)(0.5 * _vH) - _buttonTexture.Height), Color.White);
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch); 
            foreach (StaticLabel staticLabel in _recipeComponents)
                staticLabel.Draw(gameTime, spriteBatch);
            //spriteBatch.DrawString(_buttonFont, _recipe.Name, new Vector2((int)(_vW * 0.5), 200), Color.White);
            spriteBatch.DrawString(
                _buttonFont, "Cost: " + _recipe.Cost.ToString(), 
                new Vector2(
                    _vW - _buttonTexture.Width - _buttonFont.MeasureString("Cost: " + _recipe.Cost.ToString()).X, 
                    _vH - 0.5f * _buttonTexture.Height), 
                Color.Black);
            spriteBatch.End();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new JobsState(_game, _graphicsDevice, _content, _crafter));
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            GameInfo.Instance.ReduceInventory(_recipe.RecipeItems);
            _crafter.AssignTask(_recipe);
            _game.ChangeState(new CraftingMenuState(_game, _graphicsDevice, _content));
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