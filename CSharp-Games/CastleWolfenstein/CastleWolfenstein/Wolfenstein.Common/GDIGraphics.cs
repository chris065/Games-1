namespace Wolfenstein.Common
{
    using System.Drawing;
    using Interfaces;

    /// <summary>
    /// Encapsulates a GDI+ drawing surface and enables a images to be drawn.
    /// </summary>
    public class GDIGraphics : IGraphics
    {
        /// <summary>
        /// Graphics
        /// </summary>
        private readonly Graphics screenGraphics;

        /// <summary>
        /// Creates a new GDIGraphics from the specified MapImage.
        /// </summary>
        /// <param name="image">Image</param>
        public GDIGraphics(Image image)
        {
            this.screenGraphics = Graphics.FromImage(image);
        }

        /// <summary>
        /// Draws the specified MapImage at the specified location and with the specified size.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            this.screenGraphics.DrawImage(image, x, y, width, height);
        }

        /// <summary>
        /// Clears the entire drawing surface and fills it with the specified background color.
        /// </summary>
        /// <param name="color"></param>
        public void Clear(Color color)
        {
            this.screenGraphics.Clear(color);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified Brush and Font objects.
        /// </summary>
        /// <param name="str">input a string</param>
        /// <param name="font">font style</param>
        /// <param name="brush">Brush</param>
        /// <param name="x">Coordinate x</param>
        /// <param name="y">Coordinate y</param>
        public void DrawString(string str, Font font, Brush brush, float x, float y)
        {
            this.screenGraphics.DrawString(str, font, brush, x, y);
        }

        /// <summary>
        /// Draws the specified portion of the image at the specified location and with the specified size.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="location">location</param>
        /// <param name="source">Source</param>
        /// <param name="srcUnit">Graphics</param>
        public void DrawImage(Image image, Rectangle location, Rectangle source, GraphicsUnit srcUnit)
        {
            this.screenGraphics.DrawImage(image, location, source, srcUnit);
        }
    }
}