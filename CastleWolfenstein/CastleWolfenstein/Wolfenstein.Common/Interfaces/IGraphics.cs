namespace Wolfenstein.Common.Interfaces
{
    using System.Drawing;

    /// <summary>
    /// Interface IGraphics
    /// </summary>
    public interface IGraphics
    {
        /// <summary>
        /// Draws the specified MapImage at the specified location and with the specified size.
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

        /// <summary>
        /// Draws the specified portion of the image at the specified location and with the specified size.
        /// </summary>
        void DrawImage(Image image, Rectangle location, Rectangle source, GraphicsUnit srcUnit);
    }
}