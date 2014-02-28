namespace Wolfenstein.Common
{
    using System;

    /// <summary>
    /// Provides elapsed game time since the last update
    /// and total time since the start of the game.
    /// </summary>
    public class GameTime
    {
        /// <summary>
        /// Initializes a new instance of the GameTime class
        /// </summary>
        /// <param name="elapsedTime">Elapsed time</param>
        /// <param name="totalTime">Total time</param>
        public GameTime(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            this.ElapsedTime = elapsedTime;
            this.TotalTime = totalTime;
        }

        /// <summary>
        /// Gets the amount of elapsed game time since the last update.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }

        /// <summary>
        /// Gets the amount of game time since the start of the game.
        /// </summary>
        public TimeSpan TotalTime  { get; private set; }
    }
}