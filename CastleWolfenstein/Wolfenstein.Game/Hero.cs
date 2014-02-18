using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wolfenstein.Common;

namespace Wolfenstein.Game
{
    class Hero : HumanPlayer
    {
        // Animation speed in frames per second
        private const float AnimationSpeed = 12.0f;
        private float currentFrame = 0;

        public Hero(int x, int y, Bitmap sprite)
            : base(x, y, sprite)
        {
        }

        private void AnimateSprite(GameTime gameTime, Graphics mapGraphics, Bitmap sprite, int frames, bool flip)
        {
            currentFrame += (float)(AnimationSpeed * gameTime.ElapsedTime.TotalSeconds) * (flip ? -1 : 1);

            int frameWidth = sprite.Width / frames;
            int frameIndex = (int)Math.Round(currentFrame) % frames + (frames - 1) * (flip ? 1 : 0);

            Console.WriteLine(frameIndex);

            Rectangle frame = new Rectangle(frameIndex * frameWidth, 0, frameWidth, sprite.Height);
            Rectangle location = new Rectangle(this.MapPosition.X, this.MapPosition.Y, frame.Width, frame.Height);

            mapGraphics.DrawImage(sprite, location, frame, GraphicsUnit.Pixel);
        }
    }
}