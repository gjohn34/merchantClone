using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace merchantClone.Controls
{
    public class ProgressBar : Component
    {
        private GraphicsDevice _graphics;
        private Texture2D _backgroundBar;
        private Texture2D _fillBar;
        private const int _barWidth = 400;
        private const int _barHeight = 40;
        private int _oldCurrent = 0;

        public Vector2 Position { get; set; }
        public Person Watching { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }

        public ProgressBar(GraphicsDevice graphics, Person person)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, _barWidth, _barHeight);
            _graphics = graphics;
            Watching = person;
            Total = person.TotalXp;
            Current = person.CurrentXp;
            _oldCurrent = Current;
            _backgroundBar = new Texture2D(_graphics, _barWidth, _barHeight);
            Color[] data = new Color[_backgroundBar.Width * _backgroundBar.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Blue;
            }
            _backgroundBar.SetData(data);
            double fillProgress = (double)Current / Total;
            _fillBar = new Texture2D(_graphics, (int)(fillProgress * _backgroundBar.Width), _barHeight);
            Color[] compData = new Color[_fillBar.Width * _fillBar.Height];
            for (int i = 0; i < compData.Length; ++i)
            {
                compData[i] = Color.Black;
            }
            _fillBar.SetData(compData);

        }
        public override void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, _barWidth, _barHeight);
            Current = Watching.CurrentXp;
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundBar, Rectangle, Color.White);
            spriteBatch.Draw(_fillBar, new Rectangle((int)Position.X, (int)Position.Y, _fillBar.Width, _barHeight), Color.White);
        }
        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
            Position = position;
        }
    }
}