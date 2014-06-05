namespace Wolfenstein.Common
{
    using System.Drawing;

    /// <summary>
    /// Represents an animated texture.
    /// </summary>
    public class AnimTexture
    {
        /// <summary>
        /// Initializes a new instance of the AnimTexture class
        /// </summary>
        /// <param name="texture">Bitmap texture</param>
        /// <param name="frameTime">Frame time</param>
        /// <param name="isLooping">Is Looping</param>
        /// <param name="frameWidth">Frame width</param>
        public AnimTexture(Bitmap texture, float frameTime, bool isLooping, int frameWidth)
        {
            this.Texture = texture;
            this.FrameTime = frameTime;
            this.IsLooping = isLooping;
            this.FrameWidth = frameWidth;
        }

        /// <summary>
        /// Gets all frames in the animation arranged horizontally.
        /// </summary>
        public Bitmap Texture { get; private set; }

        /// <summary>
        /// Gets the duration of time to show each frame.
        /// </summary>
        public float FrameTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping { get; private set; }

        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return this.Texture.Width / this.FrameWidth; }
        }

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth { get; private set; }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return this.Texture.Height; }
        }
    }
}