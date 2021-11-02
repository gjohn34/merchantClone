using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using merchantClone.Models;
using System.Linq;
using Microsoft.Xna.Framework.Input.Touch;

namespace merchantClone.States
{
    public class ScrollPane : Component
    {
        #region Fields
        private GraphicsDevice _graphics;
        private Vector2 _position;
        private Rectangle _rectangle;
        private Texture2D _texture;
        private TouchLocation _previousTouch;
        private TouchLocation _currentTouch;
        private Matrix _transform;
        private Texture2D _rect;
        private int _compHeight;
        private Texture2D _comp;
        private Button _button;
        private int _count;
        #endregion
        #region Properties
        public List<ComponentGroup> ComponentGroups;
        #endregion
        public ScrollPane(Game game, List<ComponentGroup> components, Button button, Rectangle rectangle, Texture2D texture)
        {
            _count = 0;
            _graphics = game.GraphicsDevice;
            ComponentGroups = components;
            _rectangle = rectangle;
            _texture = texture;
            _button = button;
            _transform = Matrix.CreateTranslation(new Vector3(new Vector2(_rectangle.X, _rectangle.Y), 0));
            _button.TouchRectangle = new Rectangle((int)_button.Position.X, (int)(_button.Position.Y + _transform.Translation.Y + _texture.Height), _texture.Width, _texture.Height);

            _rect = new Texture2D(_graphics, _rectangle.Width, _rectangle.Height);
            Color[] data = new Color[_rectangle.Width * _rectangle.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Green;
            }
            _rect.SetData(data);

            // TODO - Move this to ComponentGroup construfctor


            _compHeight = 200;
            _comp = new Texture2D(_graphics, _rectangle.Width, _compHeight);
            Color[] compData = new Color[_rectangle.Width * _compHeight];
            for (int i = 0; i < compData.Length; ++i)
            {
                compData[i] = Color.Red;
            }
            _comp.SetData(compData);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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

            spriteBatch.Draw(_rect, new Rectangle(0, 0, _rectangle.Width, _rectangle.Height), Color.White);
            int margin = 50;

            foreach (ComponentGroup componentGroup in ComponentGroups)
            {
                // TODO - Move into compGroup#draw
                spriteBatch.Draw(_comp, new Rectangle((int)_position.X, (int)_position.Y + (ComponentGroups.IndexOf(componentGroup) * (_compHeight + margin)), _rectangle.Width, _compHeight), Color.White);
                componentGroup.Draw(gameTime, spriteBatch);
            }
            _button.Draw(gameTime, spriteBatch);
            //foreach (Texture2D square in _squares)
            //{
            //    spriteBatch.Draw(square, new Rectangle((int)_position.X, (int)_position.Y + (_squares.IndexOf(square) * (height + margin)), _graphics.Viewport.Width, height), Color.White);
            //}


            spriteBatch.End();
            spriteBatch.GraphicsDevice.ScissorRectangle = temp;
        }

        public override void Update(GameTime gameTime)
        {
            _count += 1;
            _previousTouch = _currentTouch;
            _currentTouch = ControlSettings.GetTouchLocation();
            if (ControlSettings.GetTouchRectangle().Intersects(_rectangle)) {
                if (_currentTouch.State == TouchLocationState.Released && (_previousTouch.State == TouchLocationState.Moved))
                {
                } else if (_currentTouch.State == TouchLocationState.Moved)
                {
                    // Updating the position of the _components based on how far the change in axis
                    _position.Y += (int)_currentTouch.Position.Y - (int)_previousTouch.Position.Y; ;
                }
            }

            int newRowY = ComponentGroups.Count * (_compHeight + 50);
            int margin = 50;

            _button.Position = new Vector2(0, _position.Y + newRowY);
            _button.TouchRectangle = new Rectangle(0, (int)_position.Y + newRowY + _texture.Height, _texture.Width, _texture.Height);

            _button.Update(gameTime);


            foreach (ComponentGroup component in ComponentGroups)
            {
                //component.Rectangle = new Rectangle((int)_position.X, (int)_position.Y + (ComponentGroups.IndexOf(component)), _rectangle.Width, _compHeight);
                component.UpdatePosition(gameTime, new Vector2(0, (int)_position.Y + (ComponentGroups.IndexOf(component) * (_compHeight + margin))));
                component.Update(gameTime);
            }
        }

        public void AddToList(ComponentGroup component)
        {
            // copy of componentGroups array
            ComponentGroup[] cc = new ComponentGroup[ComponentGroups.Count];
            // taking array 
            ComponentGroups.CopyTo(cc);
            List<ComponentGroup> t = cc.ToList();
            t.Add(component);
            ComponentGroups = t;
        }

        public override void UpdatePosition(GameTime gametime, Vector2 position)
        {
        }
    }
    public class CraftingMenuState : State
    {
        private List<Component> _components;
        private List<Person> _people;
        private Texture2D _texture;
        private SpriteFont _font;
        private int _margin;
        private int _deviceWidth;
        private Button _newPerson;
        private ScrollPane _scrollPane;


        public CraftingMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            _margin = 25;
            //_margin = (int)(0.05 * graphics.Viewport.Height);
            _deviceWidth = graphics.Viewport.Width;
            _texture = _content.Load<Texture2D>("controls/button_background2");
            _font = _content.Load<SpriteFont>("Fonts/font");
            _people = new List<Person>();

            Button backButton = new Button(_texture, _font)
            {
                Position = new Vector2(0, graphics.Viewport.Height - _texture.Height),
                Text = "Back",
            };
            _newPerson = new Button(_texture, _font)
            {
                Position = new Vector2(0, GeneratePosition()),
                Text = "New"
            };

            StaticLabel title = new StaticLabel(_texture, _font, "Crafters")
            {
                Position = new Vector2(0, 0),
                Rectangle = new Rectangle(0, 0, graphics.Viewport.Width, _texture.Height)
            };


            //Button test1 = new Button(_texture, _font)
            //{
            //    Position = new Vector2(0, 0),
            //    Text = "test1"
            //};
            //Button test2 = new Button(_texture, _font)
            //{
            //    Position = new Vector2(0, 0),
            //    Text = "test2"
            //};
            //Button test3 = new Button(_texture, _font)
            //{
            //    Position = new Vector2(0, 0),
            //    Text = "test3"
            //};
            //ComponentGroup cg1 = new ComponentGroup(new Component[3] { test1, test2, test3 });

            backButton.Touch += BackButton_Click;
            _newPerson.Touch += NewPerson_Click;
            _scrollPane = new ScrollPane(game, new List<ComponentGroup>(), _newPerson, new Rectangle(0, _texture.Height, _deviceWidth, graphics.Viewport.Height - ((_texture.Height + _margin) * 2)), _texture);
            //_scrollPane = new ScrollPane(game, new List<Component>() { _newPerson }, new Rectangle(0, _texture.Height + _margin, _deviceWidth, graphics.Viewport.Height - (_texture.Height + _margin)), _texture);

            _components = new List<Component>
            {
                backButton,
                //_newPerson,
                title
            };
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }
        private void NewPerson_Click(object sender, EventArgs e)
        {
            GeneratePersonComponent();
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _scrollPane.Draw(gameTime, spriteBatch);

            spriteBatch.Begin();
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public override void Update(GameTime gameTime)
        {
            _scrollPane.Update(gameTime);
            foreach (Component component in _components)
                component.Update(gameTime);
        }
        private void GeneratePersonComponent()
        {
            // Need to do this first becaues I'm an idiot.
            int position = GeneratePosition();
            _people.Add(new Person(PersonType.Crafter, "john"));
            Button test1 = new Button(_texture, _font) { Position = new Vector2(0,0), Text = "new test1"};
            Button test2 = new Button(_texture, _font) { Position = new Vector2(200, 0), Text = "new test2"};
            Button test3 = new Button(_texture, _font) { Position = new Vector2(400, 200), Text = "new test3" };
            _scrollPane.AddToList(new ComponentGroup(new Component[3] { test1, test2, test3 }));
            _newPerson.Position = new Vector2(0, position + (_texture.Height + _margin));
        }
        private int GeneratePosition()
        {
            return (_texture.Height + _margin) + (_people.Count * (_texture.Height + _margin));
            //return (_texture.Height + _margin) + (_people.Count * (_texture.Height + _margin));
        }
    }
}