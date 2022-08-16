using merchantClone.Controls;
using merchantClone.Helpers;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace merchantClone.States
{
    public class QuestState : State
    {
        private Quest _quest;
        private Hero _hero;
        private List<Item> _rewards;
        private Rectangle _backgroundRectangle;
        private RectangleF _modalBackground;
        private string _text;
        private int _textHeight;

        public QuestState(Game1 game, GraphicsDevice graphics, ContentManager content, Quest quest, Hero hero) : base(game, graphics, content)
        {
            _background = content.Load<Texture2D>("clearing");
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
                Text = "Go"
            };
            startQuest.Touch += StartQuest_Click;

            _components = new List<Component>
            {
                backButton,
                mapName,
                startQuest
            };
            _rewards = quest.RewardItems.ConvertAll(item => ItemDetails.GetItem(item.Id));
            if (_rewards.Count == 0)
            {
                _rewards.Add(new Item() { Name = "Nothing" });
            }
            _backgroundRectangle = new Rectangle(0, _buttonTexture.Height, _vW, _vH - 2 * _buttonTexture.Height);
            _text = TextWrapper.WrapText(quest.Description, 0.4f * _vW, _buttonFont);
            _textHeight = (int)_buttonFont.MeasureString(_text).Y;
            int modalStart = _vH - (2 * _buttonTexture.Height) - ((_rewards.Count + 4) * _buttonFont.LineSpacing) - _textHeight;
            _modalBackground = new RectangleF(
                    0.2f * _vW,
                    modalStart,
                    0.6f * _vW,
                    _vH - modalStart - _buttonTexture.Height
            );

        }

        private void StartQuest_Click(object sender, EventArgs e)
        {
            _hero.AssignTask(_quest);
            _game.ChangeState(new HeroesMenuState(_game, _graphicsDevice, _content));
            //_hero.Ass
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MapState(_game, _graphicsDevice, _content, _hero));
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_background, _backgroundRectangle, Color.White);
            spriteBatch.FillRectangle(new RectangleF(0, _vH - _buttonTexture.Height, _vW, _buttonTexture.Height), Color.White);
            spriteBatch.FillRectangle(_modalBackground, Color.Black * 0.5f);
            spriteBatch.DrawString(_buttonFont, _text, new Vector2(0.3f * _vW, _modalBackground.Y + _buttonFont.LineSpacing), Color.White);
            spriteBatch.DrawString(_buttonFont, "Rewards: ", new Vector2(0.3f * _vW, _modalBackground.Y + _textHeight + 2 * _buttonFont.LineSpacing) ,Color.White);
            _rewards.ForEach(item => 
                spriteBatch.DrawString(
                    _buttonFont, 
                    item.Name, 
                    new Vector2(0.35f * _vW, _modalBackground.Y + _textHeight + ((_rewards.IndexOf(item) + 4) * _buttonFont.LineSpacing)), 
                    Color.White
                )
            );
            spriteBatch.DrawString(
                _buttonFont, "Cost: " + _quest.Cost,
                new Vector2(
                    _vW - _buttonTexture.Width - _buttonFont.MeasureString("Cost: " + _quest.Cost).X,
                    _vH - 0.5f * _buttonTexture.Height),
                Color.Black);
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
