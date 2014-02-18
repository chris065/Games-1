namespace Wolfenstein.Common
{
    using System.Drawing;

    public interface IDrawable
    {
        void Draw(GameTime gameTime, Graphics mapGraphics);
    }
}