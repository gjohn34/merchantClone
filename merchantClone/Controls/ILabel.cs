﻿using Microsoft.Xna.Framework;

namespace merchantClone.Controls
{
    public interface ILabel
    {
        Vector2 Position { get; set; }
        Rectangle Rectangle { get; }
        string Text { get; set; }
    }
}