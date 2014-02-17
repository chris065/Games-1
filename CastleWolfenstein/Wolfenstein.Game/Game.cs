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

        internal void Run(GameTime gameTime, Graphics screenGraphics)
        {
            // This is the main game loop
            level.Update(gameTime);
            level.Draw(gameTime, screenGraphics);
        }
    }
}
