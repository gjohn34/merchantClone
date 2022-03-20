using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using merchantClone.Models;

namespace merchantClone.States
{
    public class CraftingMenuState : State
    {
        private List<ComponentRow> _scrollComponents = new List<ComponentRow>();
        private int _margin = 25;
        private Button _newPerson;
        private ScrollPane _scrollPane;

        public CraftingMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            #region Buttons
            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            backButton.Touch += BackButton_Click;
            _newPerson = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, GeneratePosition()),
                Text = "New"
            };
            _newPerson.Touch += NewPerson_Click;
            #endregion
            #region Labels
            // Labels
            StaticLabel goldLabel = new StaticLabel(_buttonTexture, _buttonFont, GameInfo.GetGold().ToString())
            { Position = new Vector2((int)(_vW * 0.5 - _buttonTexture.Width * 0.5), _vH - _buttonTexture.Height) };


            StaticLabel title = new StaticLabel(_buttonTexture, _buttonFont, "Crafters")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, _vW, _buttonTexture.Height)
            };
            #endregion


            _components = new List<Component>
            {
                backButton,
                title,
                goldLabel
            };

            foreach (Crafter crafter in _saveData.crafters)
                CrafterComponent(crafter);

            _scrollPane = new ScrollPane(game, _scrollComponents, _newPerson, new Rectangle(0, _buttonTexture.Height, _vW, _vH - ((_buttonTexture.Height + _margin) * 2)), _buttonTexture);

        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }
        private void NewPerson_Click(object sender, EventArgs e)
        {
            GeneratePersonComponent();
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
            throw new NotImplementedException();
        }
        public override void Update(GameTime gameTime)
        {
            foreach (ComponentRow componentGroup in _scrollComponents)
                componentGroup.Update(gameTime);
            _scrollPane.Update(gameTime, _scrollComponents);
            foreach (Component component in _components)
                component.Update(gameTime);
        }
        private void GeneratePersonComponent()
        {
            // Need to do this first becaues I'm an idiot.
            int position = GeneratePosition();
            string[] names = { "John", "Luke", "Paul" };
            CrafterRole[] jobs = { CrafterRole.Armorer, CrafterRole.Blacksmith, CrafterRole.Carpenter};
            string randName = names[new Random().Next(0, names.Length - 1)];
            CrafterRole randJob = jobs[new Random().Next(0, jobs.Length - 1)];
            switch (_saveData.crafters.Count)
            {
                case 0:
                    randJob = jobs[0];
                    break;
                case 1:
                    randJob = jobs[1];
                    break;
                case 2:
                    randJob = jobs[2];
                    break;
                default:
                    break;
            }
            //_people.Add(new Person(PersonType.Crafter, randName));
            // DEBUG duplicate 
            int rowHeight = 200;
            Crafter person = new Crafter(randName, randJob);
            CrafterComponent(person);
            //_scrollPane.AddToList(new ComponentGroup(new Component[3] { name, test2, recipes }));
            _newPerson.Position = new Vector2(0, position + (_buttonTexture.Height + _margin));
            _saveData.crafters.Add(person);
            SaveFile.Save();
        }

        private void CrafterComponent(Crafter crafter)
        {
            Button recipes = new Button(_buttonTexture, _buttonFont) { Position = new Vector2(10, 10), Text = crafter.Role.ToString() + " " + crafter.Level };
            ProgressBar bar = new ProgressBar(_graphicsDevice, _buttonFont, crafter.Task);

            Button job = new Button(_buttonTexture, _buttonFont);
            //Button job = new Button(_buttonTexture, _buttonFont) { Position = new Vector2(400, 200), Text = "+10 exp" };
            if (crafter.Task != null)
            {
                if (crafter.Task.IsDone())
                {
                    job.Text = "Done";
                    job.Position = new Vector2(400, 200);
                } else
                {
                    job.Text = crafter.Task.SecondsLeft().ToString();
                }
            } else
            {
                job.Text = "rdy";
            }
            job.Touch += (object sender, EventArgs e) => Job_Touch(sender, e, crafter);

            int rowHeight = 200;

            recipes.Touch += (object sender, EventArgs e) =>
                _game.ChangeState(new JobsState(_game, _graphicsDevice, _content, crafter));

            _scrollComponents.Add(new PersonGroup(
                new Component[3] { recipes, bar, job },
                new Rectangle(0, 0, _vH - ((_buttonTexture.Height + _margin) * 2), rowHeight),
                crafter)
            );
        }

        private void Job_Touch(object sender, EventArgs e, Person crafter)
        {
            if (crafter.Task != null && crafter.Task.IsDone())
            {
                GameInfo.Instance.IncreaseInventory(crafter.Task.RecipeId);
                crafter.FinishTask();
                _scrollComponents.Find(x => x.Components()[2] == sender).Refresh();
            } else
            {
                _game.ChangeState(new JobsState(_game, _graphicsDevice, _content, (Crafter)crafter));
            }
        }

        private int GeneratePosition()
        {
            return (_buttonTexture.Height + _margin) + (_scrollComponents.Count * (_buttonTexture.Height + _margin));
            //return (_texture.Height + _margin) + (_people.Count * (_texture.Height + _margin));
        }
    }
}