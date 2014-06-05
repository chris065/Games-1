namespace Wolfenstein.Common
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Interfaces;

    /// <summary>
    /// Holds the currently pressed key (if any).
    /// </summary>
    public class KeyboardState : IControllerState
    {
        /// <summary>
        /// A list with current pressed keys
        /// </summary>
        private List<Keys> pressedKeyList;

        /// <summary>
        /// Initializes a new instance of the KeyboardState class
        /// </summary>
        public KeyboardState()
        {
            this.pressedKeyList = new List<Keys>();
        }

        /// <summary>
        /// Add a key to the pressed keys
        /// </summary>
        /// <param name="key">A key</param>
        public void OnKeyDown(Keys key)
        {
            if (!this.pressedKeyList.Contains(key))
            {
                this.pressedKeyList.Add(key);
            }
        }

        /// <summary>
        /// Removes the key from the pressed keys
        /// </summary>
        /// <param name="key">A key</param>
        public void OnKeyUp(Keys key)
        {
            this.pressedKeyList.Remove(key);
        }

        /// <summary>
        /// Returns whether a specified key is currently being pressed.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>true or false if the key is pressed</returns>
        public bool IsKeyDown(Keys key)
        {
            return this.pressedKeyList.Contains(key);
        }
    }
}