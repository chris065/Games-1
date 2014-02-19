using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Wolfenstein.Common
{
    /// <summary>
    /// Holds the currently pressed key (if any).
    /// </summary>
    public class KeyboardState
    {
        private List<Keys> pressedKeyList;

        public KeyboardState()
        {
            this.pressedKeyList = new List<Keys>();
        }

        public void OnKeyDown(Keys key)
        {
            if (!this.pressedKeyList.Contains(key))
            {
                this.pressedKeyList.Add(key);
            }
        }

        public void OnKeyUp(Keys key)
        {
            this.pressedKeyList.Remove(key);
        }

        /// <summary>
        /// Returns whether a specified key is currently being pressed.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        public bool IsKeyDown(Keys key)
        {
            return this.pressedKeyList.Contains(key);
        }
    }
}