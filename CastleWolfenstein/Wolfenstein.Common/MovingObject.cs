namespace Wolfenstein.Common
{
    using System;
    using System.Drawing;

    public abstract class MovingObject : IMovable, IDrawable
    {
        public MovingObject(int x, int y, Bitmap sprite)
        {
            this.Position = new PointF(x, y);
            this.MapPosition = new Point((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y));
            this.Sprite = sprite;
            this.Bounds = new Size(this.Sprite.Width, this.Sprite.Height);
        }

        public Bitmap Sprite { get; private set; }

        public PointF Position { get; protected set; }

        public Point MapPosition { get; protected set; }

        public Size Bounds { get; private set; }

        public abstract void Move(GameTime gameTime, KeyboardState keyboardState);
 
        public virtual void Draw(GameTime gameTime, Graphics graphics)
        {
            graphics.DrawImage(this.Sprite, this.MapPosition.X, this.MapPosition.Y, this.Sprite.Width, this.Sprite.Height);
        }
    }
}