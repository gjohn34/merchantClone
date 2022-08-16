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
    public class HeroesMenuState : State
    {
        private ScrollPane _scrollPane;
        private List<ComponentRow> _scrollComponents = new List<ComponentRow>();
        private int _margin = 25;
        private Button _newPerson;
        private Rectangle _backgroundRectangle;

        public HeroesMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            ComponentRow.ResetList();
            #region Buttons
            var backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back"
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
            StaticLabel goldLabel = new StaticLabel(_buttonTexture, _buttonFont, GameInfo.GetGold().ToString())
            { Position = new Vector2((int)(_vW * 0.5 - _buttonTexture.Width * 0.5), _vH - _buttonTexture.Height) };


            StaticLabel title = new StaticLabel(_buttonTexture, _buttonFont, "Heroes")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, _vW, _buttonTexture.Height)
            };
            #endregion

            _components = new List<Component>
            {
                backButton,
                goldLabel,
                title

            };

            _background = content.Load<Texture2D>("barracks");
            _backgroundRectangle = new Rectangle(0, _buttonTexture.Height, _vW, _vH - (2 * _buttonTexture.Height));

            foreach (Hero hero in _saveData.heroes)
                HeroComponent(hero);

            _scrollPane = new ScrollPane(game, _scrollComponents, _newPerson, new Rectangle(0, _buttonTexture.Height, _vW, _vH - ((_buttonTexture.Height + _margin) * 2)), _buttonTexture);
        }

        private void NewPerson_Click(object sender, EventArgs e)
        {
            GeneratePersonComponent();
        }
        private int GeneratePosition()
        {
            return (_buttonTexture.Height + _margin) + (_scrollComponents.Count * (_buttonTexture.Height + _margin));
        }

        private void GeneratePersonComponent()
        {// Need to do this first becaues I'm an idiot.
            int position = GeneratePosition();
            string[] names = { "John", "Luke", "Paul" };
            HeroRole[] jobs = { HeroRole.Warrior, HeroRole.Mage, HeroRole.Rogue};
            string randName = names[new Random().Next(0, names.Length - 1)];
            HeroRole randJob = jobs[new Random().Next(0, jobs.Length - 1)];
            switch (_saveData.heroes.Count)
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
            //_people.Add(new Person(PersonType.Hero, randName));
            // DEBUG duplicate 
            int rowHeight = 200;
            Hero person = new Hero(randName, randJob);
            HeroComponent(person);
            //_scrollPane.AddToList(new ComponentGroup(new Component[3] { name, test2, recipes }));
            _newPerson.Position = new Vector2(0, position + (_buttonTexture.Height + _margin));
            _saveData.heroes.Add(person);
            SaveFile.Save();
        }

        private void HeroComponent(Hero hero)
        {
            Button heroDetail = new Button(_buttonTexture, _buttonFont) { Position = new Vector2(10, 10), Text = hero.Role.ToString() };
            ProgressBar bar = new ProgressBar(_graphicsDevice, _buttonFont, hero.Job);

            Button job = new Button(_buttonTexture, _buttonFont);
            //Button job = new Button(_buttonTexture, _buttonFont) { Position = new Vector2(400, 200), Text = "+10 exp" };
            if (hero.Job != null)
            {
                if (hero.Job.IsDone())
                {
                    job.Text = "Done";
                    job.Position = new Vector2(400, 200);
                }
                else
                {
                    job.Text = hero.Job.SecondsLeft().ToString();
                }
            } else
            {
                job.Text = "Go";
            }
            job.Touch += (object sender, EventArgs e) => Job_Touch(sender, e, hero);
            int rowHeight = 200;

            heroDetail.Touch += (object sender, EventArgs e) =>
                _game.ChangeState(new HeroDetailsState(_game, _graphicsDevice, _content, hero));


            _scrollComponents.Add(new PersonGroup(
                new Component[3] { heroDetail, bar, job },
                new Rectangle(0, 0, _vH - ((_buttonTexture.Height + _margin) * 2), rowHeight),
                hero)
            );
        }

        private void Job_Touch(object sender, EventArgs e, Hero hero)
        {
            if (hero.Job == null)
            {
                _game.ChangeState(new MapState(_game, _graphicsDevice, _content, hero));
            }
            else
            {
                if (hero.Job.IsDone())
                {
                    hero.FinishTask();
                    _scrollComponents.Find(x => x.Components()[2] == sender).Refresh();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            //spriteBatch.Draw(_background, new Rectangle(0, 143, _vW, _heightAfterButtons), Color.White);
            spriteBatch.Draw(_background, _backgroundRectangle, Color.White);

            spriteBatch.FillRectangle(new RectangleF(0, _vH - _buttonTexture.Height, _vW, _buttonTexture.Height), Color.White);

            spriteBatch.End();

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
        public void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }

    }
}