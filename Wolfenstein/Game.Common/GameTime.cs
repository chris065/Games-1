namespace Game
{
    using System;

    public class GameTime
    {
        public GameTime(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            this.ElapsedTime = elapsedTime;
            this.TotalTime = totalTime;
        }

        /// <summary>
        /// The amount of elapsed game time since the last update.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }


        /// <summary>
        /// The amount of game time since the start of the game.
        /// </summary>
        public TimeSpan TotalTime  { get; private set; }
    }
}