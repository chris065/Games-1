namespace Wolfenstein.Common
{
    using System.Drawing;

    /// <summary>
    /// Encapsulates a GDI+ drawing surface and enables a images to be drawn.
    /// </summary>
    public class GDIGraphics : IGraphics
    {
        private readonly Graphics screenGraphics;

        /// <summary>
        /// Creates a new GDIGraphics from the specified Image.
        /// </summary>
        public GDIGraphics(Image image)
        {
            this.screenGraphics = Graphics.FromImage(image);
        }

        /// <summary>
        /// Draws the specified Image at the specified location and with the specified size.
        /// </summary>
        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            this.screenGraphics.DrawImage(image, x, y, width, height);
        }

        /// <summary>
        /// Clears the entire drawing surface and fills it with the specified background color.
        /// </summary>
        public void Clear(Color color)
        {
            this.screenGraphics.Clear(color);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified Brush and Font objects.
        /// </summary>
        public void DrawString(string str, Font font, Brush brush, float x, float y)
        {
            this.screenGraphics.DrawString(str, font, brush, x, y);
        }
    }
}