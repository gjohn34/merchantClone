using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static merchantClone.SaveFile;

namespace merchantClone.States
{
    public abstract class State
    {
        #region Fields
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        protected List<Component> _components;
        protected SaveGame _saveData;
        protected GameInfo _gameInfo;
        protected Texture2D _buttonTexture;
        protected SpriteFont _buttonFont;
        protected int _vW;
        protected int _heightAfterButtons;
        protected Texture2D _background;
        protected int _vH;
        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            _saveData = GameInfo.Instance.GetGameData();
            _buttonTexture = content.Load<Texture2D>("controls/button_background2");
            _buttonFont = content.Load<SpriteFont>("Fonts/font");
            _vH = graphicsDevice.Viewport.Height;
            _vW = graphicsDevice.Viewport.Width;
            _heightAfterButtons = _vH - (_buttonTexture.Height * 2);
            _background = new Texture2D(_graphicsDevice, _vW, _vH);
            Color[] data = new Color[_vW * _vH];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Green;
            }
            _background.SetData(data);
        }


        public abstract void Update(GameTime gameTime);
        #endregion
    }
}