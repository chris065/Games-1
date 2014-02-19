namespace Wolfenstein.Common
{
    using System;
    using System.Drawing;

    public abstract class Sprite : IDrawable
    {
        public Sprite(int x, int y, Image image)
        {
            this.Position = new PointF(x, y);
            this.MapPosition = new Point((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y));
            this.Image = image;
            this.Bounds = new Size(this.Image.Width, this.Image.Height);
        }

        public Image Image { get; private set; }

        public PointF Position { get; protected set; }

        public Point MapPosition { get; protected set; }

        public Size Bounds { get; private set; }

        public virtual void Draw(GameTime gameTime, Graphics graphics)
        {
            graphics.DrawImage(this.Image, this.MapPosition.X, this.MapPosition.Y, this.Image.Width, this.Image.Height);
        }
    }
}