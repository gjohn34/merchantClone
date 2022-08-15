using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace merchantClone.Controls
{
    public class ScrollPane
    {
        enum SnapTo
        {
            Top,
            None,
            Bottom  
        };
        #region Fields
        private GraphicsDevice _graphics;
        private Vector2 _position;
        private Rectangle _rectangle;
        private Texture2D _texture;
        private TouchLocation _previousTouch;
        private TouchLocation _currentTouch;
        private Matrix _transform;
        private Texture2D _background;
        private int _rowHeight;
        private Texture2D _comp;
        private Button _button;
        private bool _transparent;
        private SnapTo _snapTo = SnapTo.Top;
        #endregion
        #region Properties
        public List<ComponentRow> ComponentGroups;
        #endregion
        public ScrollPane(Game game, List<ComponentRow> components, [Optional] Button button, Rectangle rectangle, Texture2D texture, Texture2D background = null)
        {
            _graphics = game.GraphicsDevice;
            ComponentGroups = components;
            _rectangle = rectangle;
            _texture = texture;
            _button = button;
            _transform = Matrix.CreateTranslation(new Vector3(new Vector2(_rectangle.X, _rectangle.Y), 0));
            if (_button != null)
            {
                _button.TouchRectangle= new Rectangle((int)_button.Position.X, (int)(_button.Position.Y + _transform.Translation.Y + _texture.Height), _texture.Width, _texture.Height);
            }

            if (background != null)
            {
                _background = background;
            }

            // TODO - Move this to ComponentGroup construfctor
            _rowHeight = 200;
            //_comp = new Texture2D(_graphics, _rectangle.Width, _rowHeight);
            //Color[] compData = new Color[_rectangle.Width * _rowHeight];
            //for (int i = 0; i < compData.Length; ++i)
            //{
            //    compData[i] = Color.Red;
            //}
            //_comp.SetData(compData);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Somehow, creating a clip outside the bounds of _rectangle
            RasterizerState rasterState = new RasterizerState();
            rasterState.CullMode = CullMode.None;
            var temp = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = _rectangle;
            rasterState.ScissorTestEnable = true;

            // Pushing panel down by x and y pixels. So they new "Zero" is at this position
            spriteBatch.Begin(SpriteSortMode.Immediate, rasterizerState: rasterState, transformMatrix: _transform);

            // Plain green fill rectangle background
            if (_background != null)
            {
                spriteBatch.Draw(_background, new Rectangle(0, (int)_position.Y, _rectangle.Width, _rectangle.Height), Color.White);
            }
            int margin = 50;

            foreach (ComponentRow componentGroup in ComponentGroups)
            {
                // TODO - Move into compGroup#draw
                //spriteBatch.Draw(_comp, componentGroup.Rectangle, Color.White);
                componentGroup.Draw(gameTime, spriteBatch);
            }
            if (_button != null)
            {
                _button.Draw(gameTime, spriteBatch);
            }


            spriteBatch.End();
            spriteBatch.GraphicsDevice.ScissorRectangle = temp;
        }

        public void Update(GameTime gameTime, List<ComponentRow> components)
        {
            ComponentGroups = components;
            _previousTouch = _currentTouch;
            int margin = 50;
            _currentTouch = ControlSettings.GetTouchLocation();
            if (ControlSettings.GetTouchRectangle().Intersects(_rectangle))
            {
                if (_currentTouch.State == TouchLocationState.Released && (_previousTouch.State == TouchLocationState.Moved))
                {
                    int containerSize = _rectangle.Height;
                    int rowsSize = (ComponentGroups.Count * (_rowHeight + margin));
                    int y = 0;

                    if (containerSize < rowsSize)
                    {
                        int difference = rowsSize - containerSize;
                        int scrollBy = (int)_position.Y + difference;
                        if (scrollBy < 0)
                        {
                            // scroll buffer passing the bottom
                            _snapTo = SnapTo.Bottom;
                            y = -difference - _rowHeight;
                        }
                        else
                        {
                            if (scrollBy < difference)
                            {
                                // scroll buffer between range
                                _snapTo = SnapTo.None;
                                y = (int)_position.Y;
                            }
                            else
                            {
                                // scroll buffer passing the top
                                _snapTo = SnapTo.Top;
                                y = 0;
                            }
                        }
                    //} else if (_background != null)
                    //{
                    //    if ((int)_position.Y < 0)
                    //    {
                    //        y = 0;
                    //    }
                    //    else if ((int)_position.Y > -_rectangle.Y)
                    //    {
                    //        y = -_rectangle.Y;
                    //    }
                    //    else
                    //    {
                    //        y = (int)_position.Y;
                    //    }
                    }
                    //else

                    _position.Y = y;
                    //if (reversed)
                    //{
                    //    _position.Y = rowsSize;
                    //}
                }
                else if (_currentTouch.State == TouchLocationState.Moved)
                {
                    // Updating the position of the _components based on how far the change in axis
                    _position.Y += (int)_currentTouch.Position.Y - (int)_previousTouch.Position.Y;
                }
            }

            int newRowY = ComponentGroups.Count * (_rowHeight + 50);
            if (_button != null)
            {
                _button.Position = new Vector2(0, _position.Y + newRowY);
                _button.TouchRectangle = new Rectangle(0, (int)_position.Y + newRowY + _texture.Height, _texture.Width, _texture.Height);
                _button.Update(gameTime);
            }

            foreach (ComponentRow component in ComponentGroups)
            {
                // Put this in yoffset
                int yOffset = component.GetYOffset();
                component.Rectangle = new Rectangle((int)_position.X, (int)_position.Y + yOffset, _rectangle.Width, _rowHeight);
                int newPos = component.Rectangle.Y + (_rowHeight - _texture.Height) / 2;
                component.UpdatePosition(gameTime, new Vector2(component.Rectangle.X, newPos));
                component.Update(gameTime);
            }

        }

        /// <summary>
        /// Unused code now that passing props down.
        /// </summary>
        /// <param name="component"></param>
        public void AddToList(ComponentRow component)
        {
            // copy of componentGroups array
            ComponentRow[] cc = new ComponentRow[ComponentGroups.Count];
            // taking array 
            ComponentGroups.CopyTo(cc);
            List<ComponentRow> t = cc.ToList();
            t.Add(component);
            ComponentGroups = t;
        }
    }
}