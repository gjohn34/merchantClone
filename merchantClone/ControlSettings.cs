using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using static merchantClone.SaveFile;

namespace merchantClone
{
    public sealed class ControlSettings
    {
        private static ControlSettings instance = null;
        private static readonly object padlock = new object();

        ControlSettings()
        {
            // TODO Move save from control settings to save file and make singleton
            //_saveGame = Load();
        }

        public static ControlSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ControlSettings();
                    }
                    return instance;
                }
            }
        }
        private static Rectangle _touchRectangle;
        private static TouchLocation _currentTouch;
        private static TouchLocation _lastTouch;
        private static SaveGame _saveGame;
        public static void UpdateTouch(Rectangle touchRectangle, TouchLocation currentTouch)
        {
            _touchRectangle = touchRectangle;
            _lastTouch = _currentTouch;
            _currentTouch = currentTouch;
        }

        public static Rectangle GetTouchRectangle()
        {
            return _touchRectangle;
        }
        public static TouchLocation GetTouchLocation()
        {
            return _currentTouch;
        }
        public static TouchLocation GetLastTouchLocation()
        {
            return _lastTouch;
        }

    }
}