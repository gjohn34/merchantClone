using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class QuestState : State
    {
        private Quest _quest;
        private Hero _hero;

        public QuestState(Game1 game, GraphicsDevice graphics, ContentManager content, Quest quest, Hero hero) : base(game, graphics, content)
        {
            _quest = quest;
            _hero = hero;
            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            backButton.Touch += BackButton_Click;

            StaticLabel mapName = new StaticLabel(_buttonTexture, _buttonFont, _quest.Name)
            {
                Position = new Vector2(0,0),
                Rectangle = new Rectangle(0,0, _vW, _buttonTexture.Height),
            };
            Button startQuest = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(_vW - _buttonTexture.Width, _vH - _buttonTexture.Height),
                Text = ""
            };

            _components = new List<Component>
            {
                backButton,
                mapName,
                startQuest
            };
        }

        private void StartQuest_Click()
        {
            //_hero.Ass
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MapState(_game, _graphicsDevice, _content, _hero));
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            //spriteBatch.Draw(_mapBackground, _r, Color.White);
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {

            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}
