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
        private Texture2D _texture;
        private SpriteFont _font;
        private int _deviceWidth;
        private ScrollPane _scrollPane;
        private int _margin;
        private List<ComponentRow> _scrollComponents = new List<ComponentRow>();

        public JobsState(Game1 game, GraphicsDevice graphics, ContentManager content, Crafter person) : base(game, graphics, content)
        {
            //_person = person;
            _margin = 25;
            _texture = _content.Load<Texture2D>("controls/button_background2");
            _font = _content.Load<SpriteFont>("Fonts/font");
            _deviceWidth = graphics.Viewport.Width;
            _person = person;

            StaticLabel title = new StaticLabel(_texture, _font, person.Name + " the " + person.Job + person.Level.ToString())
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, graphics.Viewport.Width, _texture.Height)
            };

            Button backButton = new Button(_texture, _font)
            {
                Position = new Vector2(0, graphics.Viewport.Height - _texture.Height),
                Text = "Back Button",
            };

            backButton.Touch += BackButton_Click;


            _components = new List<Component>
            {
                backButton,
                //_newPerson,
                title
            };

            _scrollPane = new ScrollPane(
                game: game,
                components: _scrollComponents,
                //components: person.GetRecipes(),
                rectangle: new Rectangle(0, _texture.Height, _deviceWidth, graphics.Viewport.Height - ((_texture.Height + _margin) * 2)),
                texture: _texture);

            // Get the recipes of that person
            // inside each person has their recipes
            // simple 3 is enough
            // foreach recipe, new recipeGroup 
            int rowHeight = 200;

            List<Recipe> recipes = person.GetJobs();
            foreach (Recipe recipe in recipes)
            {
                Button startJob = new Button(_texture, _font) { Position = new Vector2(400, 200), Text = "Start Job" };
                Button showRecipe = new Button(_texture, _font) { Position = new Vector2(10, 10), Text = recipe.Name };
                showRecipe.Touch += (object sender, EventArgs e) => ShowRecipe_Click(sender, e, recipe);
                startJob.Touch += (object sender, EventArgs e) => ShowRecipe_Click(sender, e, recipe);
                startJob.Disabled = !GameInfo.Instance.CanMake(recipe);
                
                // ADDING EACH JOB TO THE LIST OF THE COMPONENTS
                _scrollComponents.Add(new RecipeGroup(
                    new Component[2] {
                        showRecipe,
                        startJob
                    },
                    new Rectangle(0, 0, _graphicsDevice.Viewport.Height - ((_texture.Height + _margin) * 2), rowHeight)));
            }

        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CraftingMenuState(_game, _graphicsDevice, _content));
        }
        private void ShowRecipe_Click(object sender, EventArgs e, Recipe recipe)
        {
            _game.ChangeState(new ShowRecipeState(_game, _graphicsDevice, _content, recipe, this));
        }
        private void StartJob_Click(object sender, EventArgs e, Recipe recipe)
        {
            _game.ChangeState(new ShowRecipeState(_game, _graphicsDevice, _content, recipe, this));
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

            //_tick += 1;
            //if (_tick % 240 == 0)
            //{
            //    Console.WriteLine("Button");
            //    Console.WriteLine(_back.Rectangle);
            //    Console.WriteLine(_back.TouchRectangle);
            //    Console.WriteLine("Touch Settings");
            //    Console.WriteLine(ControlSettings.GetTouchLocation());
            //    Console.WriteLine(ControlSettings.GetTouchRectangle());
            //    Console.WriteLine("--------------------------------------");
            //}
        }
    }
}