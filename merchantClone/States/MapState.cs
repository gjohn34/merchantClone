using merchantClone.Controls;
using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace merchantClone.States
{
    public class MapState : State
    {
        private Texture2D _mapBackground;
        private Rectangle _r;
        private ScrollPane _scrollPane;
        private List<ComponentRow> _scrollComponents = new List<ComponentRow>();
        private Hero _hero;

        public MapState(Game1 game, GraphicsDevice graphics, ContentManager content, Hero hero) : base(game, graphics, content)
        {
            ComponentRow.ResetList();
            _hero = hero;
            #region Buttons
            Button backButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(0, _vH - _buttonTexture.Height),
                Text = "Back",
            };
            backButton.Touch += BackButton_Click;

            StaticLabel mapName = new StaticLabel(_buttonTexture, _buttonFont, "Deep Jungle")
            {
                Position = new Vector2(_vW - (_vW - _buttonTexture.Width), _vH - _buttonTexture.Height),
                Rectangle = new Rectangle(_vW - (_vW - _buttonTexture.Width), _vH - _buttonTexture.Height, _vW - _buttonTexture.Width, _buttonTexture.Height),
            };

            _components = new List<Component>
            {
                backButton,
                mapName
            };
            #endregion
            _mapBackground = content.Load<Texture2D>("sprites/maps/map1");
            //_r = new Rectangle(0, 0, _vW, _vH -_buttonTexture.Height);
            //_r = new Rectangle(0, 0 - _vH - _buttonTexture.Height, _vW, _vH * 2);
            _r = new Rectangle(0, 0 - (_mapBackground.Height - _vH)  - 143, _vW, _mapBackground.Height);
            //_r = new Rectangle(0, 0 - (_mapBackground.Height - _vH) - 143, _vW, _mapBackground.Height);

            Rectangle r2 = new Rectangle(0, 0 - _mapBackground.Height - 143, _vW, _mapBackground.Height);

            int yOffset = _r.Height - _buttonTexture.Height;
            // Hardcoded values for now, check scroll pane for originals
            int _rectanglewidth = 200;
            int _rowHeight = 143;
            Texture2D bg = new Texture2D(_graphicsDevice, _rectanglewidth, _rowHeight);
            Color[] compData = new Color[_rectanglewidth * _rowHeight];
            for (int i = 0; i < compData.Length; ++i)
            {
                compData[i] = Color.Red;
            }
            bg.SetData(compData);

            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont, (a,b) => QuestButton_Click(a,b, 1)) { Text = "1" },
                new Rectangle(400, yOffset - 100, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont, (a, b) => QuestButton_Click(a, b, 2)) { Text = "2" },
                new Rectangle(100, yOffset - 400, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont, (a, b) => QuestButton_Click(a, b, 3)) { Text = "3" },
                new Rectangle(250, yOffset - 800, _vW, _buttonTexture.Height),
                bg
                )
            );



            _scrollPane = new ScrollPane(
                game: game,
                components: _scrollComponents,
                rectangle: _r,
                texture: _buttonTexture,
                background: _mapBackground
                );
        }

        private void QuestButton_Click(object sender, EventArgs e, int id)
        {
            _game.ChangeState(new QuestState(_game, _graphicsDevice, _content, Quest.GetQuest(id), _hero));
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HeroesMenuState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(_mapBackground, _r, Color.White);
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