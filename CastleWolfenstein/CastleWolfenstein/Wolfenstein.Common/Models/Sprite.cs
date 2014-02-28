namespace Wolfenstein.Common.Models
{
    using System;
    using System.Drawing;
    using Interfaces;

    /// <summary>
    /// A class for the sprites
    /// </summary>
    public class Sprite : IDrawable
    {
        /// <summary>
        /// Initializes a new instance of the Sprite class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="image">MapImage for the sprite</param>
        protected Sprite(int x, int y, Image image)
        {
            this.SetInitialPosition(x, y);
            this.Image = image;
        }

        /// <summary>
        /// Gets a rectangle with location the upper-left corner of the sprite (in pixel coordinates)
        /// and size - the size of the sprite (in pixels).
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                var rectangle = new Rectangle(this.MapPosition, this.Size);
                return rectangle;
            }
        }

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        protected PointF Position { get; set; }

        /// <summary>
        /// Gets or sets the map position
        /// </summary>
        protected Point MapPosition { get; set; }

        /// <summary>
        /// Gets or sets the image
        /// </summary>
        private Image Image { get; set; }

        /// <summary>
        /// Gets the size of the sprite, generally this would be image.Width and image.Height,
        /// however for animated sprites, where the image could consist of many frames,
        /// it should be the size of the current frame
        /// </summary>
        private Size Size
        {
            get
            {
                var size = new Size(this.Image.Width, this.Image.Height);
                return size;
            }
        }

        /// <summary>
        /// Draws the image
        /// </summary>
        /// <param name="graphics">A graphics</param>
        /// <param name="gameTime">A game time</param>
        public virtual void Draw(IGraphics graphics, GameTime gameTime)
        {
            graphics.DrawImage(this.Image, this.MapPosition.X, this.MapPosition.Y, this.Image.Width, this.Image.Height);
        }

        /// <summary>
        /// Sets the initial position
        /// </summary>
        /// <param name="x">A coordinate X</param>
        /// <param name="y">A coordinate Y</param>
        public void SetInitialPosition(int x, int y)
        {
            this.Position = new PointF(x, y);
            this.MapPosition = new Point((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y));
        }
    }
}