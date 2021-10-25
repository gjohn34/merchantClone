using merchantClone.Controls;
using merchantClone.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Text;
using static merchantClone.SaveFile;

namespace merchantClone
{
    public class Game1 : Game
    {
        private SaveGame _saveFile = SaveFile.Instance.GetSave();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private State _currentState;
        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //ControlSettings.UpdateSave(SaveFile.Load());
            _currentState = new CraftingMenuState(this, GraphicsDevice, Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count != 0)
            {
                Rectangle touchRectangle = new Rectangle((int)tc[0].Position.X, (int)tc[0].Position.Y, 1, 1);
                ControlSettings.UpdateTouch(touchRectangle, tc[0]);
            } else
            {
                ControlSettings.UpdateTouch(new Rectangle(), new TouchLocation());
            }
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }
            _currentState.Update(gameTime);
            //_currentState.PostUpdate(gameTime);
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // TODO: Add your drawing code here

            _currentState.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            //_currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);

        }
    }
}
