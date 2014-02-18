using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolfenstein.Common
{
    /// <summary>
    /// Holds the currently pressed key (if any).
    /// </summary>
    public class KeyboardState
    {
        private bool keyIsDown;
        private Keys keyPressed;

        public KeyboardState()
        {
            this.keyIsDown = false;
        }

        public void OnKeyDown(Keys key)
        {
            this.keyIsDown = true;
            this.keyPressed = key;
        }

        public void OnKeyUp(Keys key)
        {
            this.keyIsDown = false;
        }

        /// <summary>
        /// Returns whether a specified key is currently being pressed.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        public bool IsKeyDown(Keys key)
        {
            return this.keyIsDown == true && this.keyPressed == key;
        }
    }
}
