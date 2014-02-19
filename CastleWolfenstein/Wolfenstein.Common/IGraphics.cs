namespace Wolfenstein.Common
{
    using System.Drawing;

    /// <summary>
    /// Defines a method to draw an image at a specified location and with specified size.
    /// </summary>
    public interface IGraphics
    {
        /// <summary>
        /// Draws the specified Image at the specified location and with the specified size.
        /// </summary>
        void DrawImage(Image image, int x, int y, int width, int height);

        /// <summary>
        /// Clears the entire drawing surface and fills it with the specified background color.
        /// </summary>
        void Clear(Color color);

        /// <summary>
        /// Draws the specified text string at the specified location with the specified Brush and Font objects.
        /// </summary>
        void DrawString(string str, Font font, Brush brush, float x, float y);
    }
}