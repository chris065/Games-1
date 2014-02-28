namespace Wolfenstein.Common.Interfaces
{
    /// <summary>
    /// IDrawable interface
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws graphics
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="gameTime">Game time</param>
        void Draw(IGraphics graphics, GameTime gameTime);
    }
}