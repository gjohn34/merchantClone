using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace merchantClone.Controls
{
    public class ProgressBar : Component
    {
        #region Fields
        private GraphicsDevice _graphics;
        private Texture2D _backgroundBar;
        private Texture2D _fillBar;
        private const int _barWidth = 400;
        private const int _barHeight = 40;
        private int _oldCurrent = 0;
        private SpriteFont _font;
#nullable enable
        private Job? _task = null;
#nullable disable
        private bool _done = false;
        #endregion

        #region Properties
        public Vector2 Position { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
        #endregion
        public ProgressBar(GraphicsDevice graphics, SpriteFont font, Job task)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, _barWidth, _barHeight);
            _graphics = graphics;
            _font = font;

            if (task != null)
            {
                _task = task;
                _done = task.IsDone();
                Total = task.Seconds;
            }
            if (!_done && _task != null)
            {
                DateTime now = DateTime.Now;
                Current = task.Seconds - (int)_task.FinishTime.Subtract(now).TotalSeconds;
                _oldCurrent = Current;
                _backgroundBar = new Texture2D(_graphics, _barWidth, _barHeight);
                Color[] data = new Color[_backgroundBar.Width * _backgroundBar.Height];
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = Color.Blue;
                }
                _backgroundBar.SetData(data);
                int fillProgress = (int)Current / Total;
                if (fillProgress > 0)
                {
                    _fillBar = new Texture2D(_graphics, (int)(fillProgress * _backgroundBar.Width), _barHeight);
                    Color[] compData = new Color[_fillBar.Width * _fillBar.Height];
                    for (int i = 0; i < compData.Length; ++i)
                    {
                        compData[i] = Color.Black;
                    }
                    _fillBar.SetData(compData);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            _done = _task == null ? false : _task.IsDone();
            if (!_done && _task != null)
            {
                DateTime now = DateTime.Now;
                Current = _task.Seconds - (int)_task.FinishTime.Subtract(now).TotalSeconds;
                if (_oldCurrent != Current)
                {
                    _oldCurrent = Current;
                    double fillProgress = (double)Current / Total;
                    _fillBar = new Texture2D(_graphics, (int)(fillProgress * _backgroundBar.Width), _barHeight);
                    Color[] compData = new Color[_fillBar.Width * _fillBar.Height];
                    for (int i = 0; i < compData.Length; ++i)
                    {
                        compData[i] = Color.Black;
                    }
                    _fillBar.SetData(compData);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_task != null)
            {
                if (!_done)
                {
                    spriteBatch.Draw(_backgroundBar, new Rectangle((int)Position.X, (int)Position.Y, _barWidth, _barHeight), Color.White);
                    if (_fillBar != null)
                    {
                        spriteBatch.Draw(_fillBar, new Rectangle((int)Position.X, (int)Position.Y, _fillBar.Width, _barHeight), Color.White);
                    }
                } else
                {
                    spriteBatch.DrawString(_font, "Job Done", Position, Color.Black);
                }
            }
        }
        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            Position = position;
        }

        public void ResetTask() {
            _task = null;
            _done = false;
        }
    }
}