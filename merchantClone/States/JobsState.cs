using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace merchantClone.States
{
    public class JobsState : State
    {
        private Crafter _person;
        private ScrollPane _scrollPane;
        private int _margin = 25;
        private List<ComponentRow> _scrollComponents = new List<ComponentRow>();

        public JobsState(Game1 game, GraphicsDevice graphics, ContentManager content, Crafter person) : base(game, graphics, content)
        {
            _person = person;

            // Buttons
            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back Button",
            };
            backButton.Touch += BackButton_Click;

            Button allRecipes = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(_vW - _buttonTexture.Width, _vH - _buttonTexture.Height),
                Text = "All"
            };
            allRecipes.Touch += AllRecipes_Click;

            // Labels
            StaticLabel title = new StaticLabel(_buttonTexture, _buttonFont, person.Name + " the " + person.Role + person.Level.ToString())
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, graphics.Viewport.Width, _buttonTexture.Height)
            };

            _components = new List<Component>
            {
                backButton,
                allRecipes,
                title
            };

            _scrollPane = new ScrollPane(
                game: game,
                components: _scrollComponents,
                rectangle: new Rectangle(0, _buttonTexture.Height, _vW, graphics.Viewport.Height - ((_buttonTexture.Height + _margin) * 2)),
                texture: _buttonTexture);

            foreach (Recipe recipe in _person.GetJobs())
            {
                if (person.Level >= recipe.RequiredLevel)
                {
                    LoadRecipeComponents(recipe);

                }
            }

        }

        private void AllRecipes_Click(object sender, EventArgs e)
        {
            _scrollComponents = new List<ComponentRow>();

            foreach (Recipe recipe in _person.GetJobs())
               LoadRecipeComponents(recipe, true);


        }

        private void LoadRecipeComponents(Recipe recipe, bool all = false )
        {
            bool canMake = GameInfo.Instance.CanMake(recipe);
            if (!all && !canMake) return;
            int rowHeight = 200;
            Button startJob = new Button(_buttonTexture, _buttonFont) { Position = new Vector2(400, 200), Text = "Start Job" };
            Button showRecipe = new Button(_buttonTexture, _buttonFont) { Position = new Vector2(10, 10), Text = recipe.Name };
            showRecipe.Touch += (object sender, EventArgs e) => ShowRecipe_Click(sender, e, recipe);

            startJob.Touch += (object sender, EventArgs e) => StartJob_Click(sender, e, recipe);
            startJob.Disabled = !canMake;
            _scrollComponents.Add(new RecipeGroup(
                    new Component[2] {
                        showRecipe,
                        startJob
                    },
                    new Rectangle(0, 0, _vH - ((_buttonTexture.Height + _margin) * 2), rowHeight)));
        }

        private void StartJob_Click(object sender, EventArgs e, Recipe recipe)
        {
            _person.AssignTask(recipe);
            _game.ChangeState(new CraftingMenuState(_game, _graphicsDevice, _content));
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CraftingMenuState(_game, _graphicsDevice, _content));
        }
        private void ShowRecipe_Click(object sender, EventArgs e, Recipe recipe)
        {
            _game.ChangeState(new ShowRecipeState(_game, _graphicsDevice, _content, recipe, this, _person));
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            _scrollPane.Draw(gameTime, spriteBatch);

            spriteBatch.Begin();
            
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
            
            spriteBatch.End();
        }
        public override void PostUpdate(GameTime gameTime)
        {
        }
        public override void Update(GameTime gameTime)
        {
            foreach (ComponentRow componentGroup in _scrollComponents)
                componentGroup.Update(gameTime);
            _scrollPane.Update(gameTime, _scrollComponents);
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}