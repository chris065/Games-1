namespace Wolfenstein.Common
{
    using System;
    using System.Drawing;

    public abstract class MovingObject : Sprite, IMovable
    {
        public MovingObject(int x, int y, Bitmap sprite)
            : base(x, y, sprite)
        {
        }

        public abstract void Move(GameTime gameTime, KeyboardState keyboardState);
    }
}