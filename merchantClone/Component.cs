using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace merchantClone
{
    public enum SwapWith
    {
        person,
        bar,
        label,
        button
    }
    public abstract class Component
    {
        public Rectangle Rectangle;
        public string Text;
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void UpdatePosition(GameTime gametime, Vector2 position);
    }

    public abstract class ComponentRow
    {
        public Rectangle Rectangle;
        public abstract Component[] Components();
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void UpdatePosition(GameTime gametime, Vector2 position);
        public abstract void Refresh();
        //public abstract void SwapWith(SwapWith swapWith, Component component);
    }

}