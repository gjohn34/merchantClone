using merchantClone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using merchantClone.Models;
using System.Linq;
using Microsoft.Xna.Framework.Input.Touch;
using System.Runtime.InteropServices;

namespace merchantClone.States
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
        private Texture2D _rect;
        private int _rowHeight;
        private Texture2D _comp;
        private Button _button;
        private SnapTo _snapTo = SnapTo.Top;
        #endregion
        #region Properties
        public List<ComponentRow> ComponentGroups;
        #endregion
        public ScrollPane(Game game, List<ComponentRow> components, [Optional] Button button, Rectangle rectangle, Texture2D texture)
        {
            _graphics = game.GraphicsDevice;
            ComponentGroups = components;
            _rectangle = rectangle;
            _texture = texture;
            _button = button;
            _transform = Matrix.CreateTranslation(new Vector3(new Vector2(_rectangle.X, _rectangle.Y), 0));
            if (_button != null)
            {
                _button.TouchRectangle = new Rectangle((int)_button.Position.X, (int)(_button.Position.Y + _transform.Translation.Y + _texture.Height), _texture.Width, _texture.Height);
            } 

            _rect = new Texture2D(_graphics, _rectangle.Width, _rectangle.Height);
            Color[] data = new Color[_rectangle.Width * _rectangle.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Green;
            }
            _rect.SetData(data);

            // TODO - Move this to ComponentGroup construfctor
            _rowHeight = 200;
            _comp = new Texture2D(_graphics, _rectangle.Width, _rowHeight);
            Color[] compData = new Color[_rectangle.Width * _rowHeight];
            for (int i = 0; i < compData.Length; ++i)
            {
                compData[i] = Color.Red;
            }
            _comp.SetData(compData);

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

            spriteBatch.Draw(_rect, new Rectangle(0, 0, _rectangle.Width, _rectangle.Height), Color.White);
            int margin = 50;

            foreach (ComponentRow componentGroup in ComponentGroups)
            {
                // TODO - Move into compGroup#draw
                spriteBatch.Draw(_comp, componentGroup.Rectangle, Color.White);
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
            if (ControlSettings.GetTouchRectangle().Intersects(_rectangle)) {
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
                        } else
                        {
                            if (scrollBy < difference)
                            {
                                // scroll buffer between range
                                _snapTo = SnapTo.None;
                                y = (int)_position.Y;
                            } else
                            {
                                // scroll buffer passing the top
                                _snapTo = SnapTo.Top;
                                y = 0;
                            }
                        }
                    }
                    _position.Y = y;
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
                component.Rectangle = new Rectangle((int)_position.X, (int)_position.Y + (ComponentGroups.IndexOf(component) * (_rowHeight + margin)), _rectangle.Width, _rowHeight);
                int newPos = component.Rectangle.Y + (_comp.Height - _texture.Height) / 2;
                component.UpdatePosition(gameTime, new Vector2(component.Rectangle.X, newPos) );
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
    public class CraftingMenuState : State
    {
        private List<ComponentRow> _scrollComponents;
        private Texture2D _texture;
        private SpriteFont _font;
        private int _margin;
        private int _deviceWidth;
        private Button _newPerson;
        private ScrollPane _scrollPane;

        public CraftingMenuState(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            _margin = 25;
            _scrollComponents = new List<ComponentRow>();
            //_margin = (int)(0.05 * graphics.Viewport.Height);
            _deviceWidth = graphics.Viewport.Width;
            _texture = _content.Load<Texture2D>("controls/button_background2");
            _font = _content.Load<SpriteFont>("Fonts/font");

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
            _scrollPane = new ScrollPane(game, new List<ComponentRow>(), _newPerson, new Rectangle(0, _texture.Height, _deviceWidth, graphics.Viewport.Height - ((_texture.Height + _margin) * 2)), _texture);
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
            foreach (ComponentRow componentGroup in _scrollComponents)
                componentGroup.Update(gameTime);
            _scrollPane.Update(gameTime, _scrollComponents);
            foreach (Component component in _components)
                component.Update(gameTime);
        }
        private void GeneratePersonComponent()
        {
            // Need to do this first becaues I'm an idiot.
            int position = GeneratePosition();
            
            string[] names = { "John", "Luke", "Paul" };
            Roles[] jobs = { Roles.Carpenter, Roles.Armorer, Roles.Blacksmith };
            string randName = names[new Random().Next(0, names.Length - 1)];
            Roles randJob = jobs[new Random().Next(0, jobs.Length - 1)];
            //_people.Add(new Person(PersonType.Crafter, randName));
            // DEBUG duplicate 
            int rowHeight = 200;
            Crafter person = new Crafter(randName, randJob);
            Button recipes = new Button(_texture, _font) { Position = new Vector2(10, 10), Text = person.Job.ToString()};
            
            //StaticLabel name = new StaticLabel(_texture, _font, person.Name) { Position = new Vector2(10, 10) };
            //StaticLabel time = new StaticLabel(_texture, _font, DateTime.Now.ToString());
            ProgressBar bar = new ProgressBar(_graphicsDevice, person) { Position = new Vector2(200, 0) };
            //DynamicLabel test2 = new Button(_texture, _font) { Position = new Vector2(200, 0), Text = "new test2"};
            Button job = new Button(_texture, _font) { Position = new Vector2(400, 200), Text = "+10 exp" };

            job.Touch += (object sender, EventArgs e) =>
                person.FinishTask();

            recipes.Touch += (object sender, EventArgs e) =>
            {
                _game.ChangeState(new JobsState(_game, _graphicsDevice, _content, person));
                //    Button backButton = new Button(_texture, _font)
                //    {
                //        Position = new Vector2(0, _graphicsDevice.Viewport.Height - _texture.Height),
                //        Text = "Back",
                //    };

                //    _scrollPane = new ScrollPane(
                //        _game,
                //        new List<ComponentGroup>(),
                //        //person.ShowJobs(_texture, _font),
                //        backButton,
                //        new Rectangle(0, _texture.Height, _deviceWidth, _graphicsDevice.Viewport.Height - ((_texture.Height + _margin) * 2)),
                //        _texture
                //    );
            };

            //Button recipes = new Button(_texture, _font) { Position = new Vector2(400, 200), Text = $"{randName}'s Recipes" };
            _scrollComponents.Add(new PersonGroup(
                new Component[3] { recipes, bar, job }, 
                new Rectangle(0,0, _graphicsDevice.Viewport.Height - ((_texture.Height + _margin) * 2), rowHeight))
            );
            //_scrollPane.AddToList(new ComponentGroup(new Component[3] { name, test2, recipes }));
            _newPerson.Position = new Vector2(0, position + (_texture.Height + _margin));
        }
        private int GeneratePosition()
        {
            return (_texture.Height + _margin) + (_scrollComponents.Count * (_texture.Height + _margin));
            //return (_texture.Height + _margin) + (_people.Count * (_texture.Height + _margin));
        }
    }
}