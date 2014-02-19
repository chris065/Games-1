using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfenstein.Common;

namespace Wolfenstein.Game
{
    class Game
    {
        public Level level;

        public Game()
        {
            // Create level
            this.level = new Level();
        }

        /// <summary>
        /// Run game logic here such as updating the world,
        /// checking for collisions, handling input and drawing the game.
        /// This method will be called as frequently as possible,
        /// when the Windows message queue is empty.
        /// Check GameTime to get the elapsed time since the last update.
        /// </summary>
        internal void Run(GameTime gameTime, IGraphics renderer, KeyboardState keyboardState)
        {
            // This is the main game loop
            level.Update(gameTime, keyboardState);
            level.Draw(gameTime, renderer);
        }
    }
}
