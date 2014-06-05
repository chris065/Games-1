namespace Wolfenstein.Game
{
    using Common;
    using Common.Interfaces;
    using Environment;

    /// <summary>
    /// A class for the game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Level/Room in the game
        /// </summary>
        private Level level;

        /// <summary>
        /// Status if the level/room should be changed
        /// </summary>
        private LevelChangeStatus levelStat;

        /// <summary>
        /// Initializes a new instance of the Game class
        /// </summary>
        public Game()
        {
            this.CurrentLevel = new Level();
            this.levelStat = LevelChangeStatus.NoChange;
        }

        /// <summary>
        /// Gets the current level
        /// </summary>
        public Level CurrentLevel
        {
            get { return this.level; }
            private set { this.level = value; }
        }

        /// <summary>
        /// Run game logic here such as updating the world,
        /// checking for collisions, handling input and drawing the game.
        /// This method will be called as frequently as possible,
        /// when the Windows message queue is empty.
        /// Check GameTime to get the elapsed time since the last update.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="renderer">A Renderer</param>
        /// <param name="keyboardState">A keyboard state</param>
        public void Run(GameTime gameTime, IGraphics renderer, IControllerState keyboardState)
        {
            if (this.levelStat == LevelChangeStatus.NoChange)
            {
                if (Collisions.GetPlayerTile(this.level.Player) == TileType.Next)
                {
                    this.level.LevelIndex++;
                    this.levelStat = LevelChangeStatus.Change;
                }
                else if (Collisions.GetPlayerTile(this.level.Player) == TileType.Back)
                {
                    this.level.LevelIndex--;
                    this.levelStat = LevelChangeStatus.Change;
                }
                else
                {
                    this.level.Update(gameTime, keyboardState);
                    this.level.Draw(gameTime, renderer);
                }
            }
            else
            {
                this.level.UpdateLevel();
                this.levelStat = LevelChangeStatus.NoChange;
            }
        }
    }
}
