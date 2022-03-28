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


        public MapState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            ComponentRow.ResetList();
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
                new Button(_buttonTexture, _buttonFont) { Text = "1" },
                new Rectangle(0, yOffset, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "2" },
                new Rectangle(100, yOffset - 250, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "3" },
                new Rectangle(200, yOffset - 500, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "4" },
                new Rectangle(300, yOffset - 750, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "5" },
                new Rectangle(0, yOffset - 1000, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "6" },
                new Rectangle(100, yOffset - 1250, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "7" },
                new Rectangle(200, yOffset - 1500, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "8" },
                new Rectangle(300, yOffset - 1750, _vW, _buttonTexture.Height),
                bg
                )
            );


            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "9" },
                new Rectangle(0, yOffset - 2000, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "10" },
                new Rectangle(100, yOffset - 2250, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "11" },
                new Rectangle(200, yOffset - 2500, _vW, _buttonTexture.Height),
                bg
                )
            );
            _scrollComponents.Add(new MapGroup(
                new Button(_buttonTexture, _buttonFont) { Text = "12" },
                new Rectangle(300, yOffset - 2750, _vW, _buttonTexture.Height),
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