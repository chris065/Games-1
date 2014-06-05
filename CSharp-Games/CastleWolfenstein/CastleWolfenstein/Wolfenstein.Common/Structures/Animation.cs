namespace Wolfenstein.Common.Structures
{
    using System;
    using System.Drawing;
    using Interfaces;

    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    public struct Animation
    {
        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;

        /// <summary>
        /// Gets or sets the animation which is currently playing.
        /// </summary>
        private AnimTexture AnimTexture { get; set; }

        /// <summary>
        /// Gets or sets the index of the current frame in the animation.
        /// </summary>
        private int FrameIndex { get; set; }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        /// <param name="animTexture">An animation texture</param>
        public void PlayAnimation(AnimTexture animTexture)
        {
            // If this animation is already running, do not restart it.
            if (this.AnimTexture == animTexture)
            {
                return;
            }

            // Start the new animation.
            this.AnimTexture = animTexture;
            this.FrameIndex = 0;
            this.time = 0.0f;
        }

        /// <summary>
        /// Loads an animation from a texture
        /// </summary>
        /// <param name="animTexture">Animation texture</param>
        public void LoadAnimation(AnimTexture animTexture)
        {
            // Start the new animation.
            this.AnimTexture = animTexture;
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        /// <param name="gameTime">A game time</param>
        /// <param name="spriteBatch">A graphics</param>
        /// <param name="position">A position</param>
        /// <param name="spriteEffects">To flip an image</param> NOT USED
        public void Draw(GameTime gameTime, IGraphics spriteBatch, PointF position, bool spriteEffects)
        {
           if (AnimTexture == null)
           {
               throw new NotSupportedException("No animation is currently playing.");
           }

            // Process passing time.
            this.time += (float)gameTime.ElapsedTime.TotalSeconds;

            while (this.time > AnimTexture.FrameTime)
            {
                this.time -= AnimTexture.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (AnimTexture.IsLooping)
                {
                    this.FrameIndex = (this.FrameIndex + 1) % AnimTexture.FrameCount;
                }
                else
                {
                    this.FrameIndex = Math.Min(this.FrameIndex + 1, AnimTexture.FrameCount - 1);
                }
            }

            // Draw the current frame.
            this.DrawFrame(spriteBatch, position, this.FrameIndex);
        }

        /// <summary>
        /// Draws a frame
        /// </summary>
        /// <param name="spriteBatch">A graphics</param>
        /// <param name="position">A position</param>
        /// <param name="frameIndex">A frame index</param>
        public void DrawFrame(IGraphics spriteBatch, PointF position, int frameIndex)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(frameIndex * AnimTexture.FrameWidth, 0, AnimTexture.FrameWidth, AnimTexture.FrameHeight);
            Rectangle location = new Rectangle((int)position.X, (int)position.Y, source.Width, source.Height);

            // Draw the current frame.
            spriteBatch.DrawImage(AnimTexture.Texture, location, source, GraphicsUnit.Pixel);
        }
    }
}