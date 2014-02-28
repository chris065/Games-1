namespace Wolfenstein.Common.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// Defines a method to return whether a specified key is currently being pressed.
    /// </summary>
    public interface IControllerState
    {
        /// <summary>
        /// Returns whether a specified key is currently being pressed.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>true/false if there is a pressed key</returns>
        bool IsKeyDown(Keys key);

        /// <summary>
        /// When the key is released removed the key from the pressed keys
        /// </summary>
        /// <param name="key">A key</param>
        void OnKeyUp(Keys key);

        /// <summary>
        /// When the key is pressed adds the key to the pressed keys
        /// </summary>
        /// <param name="key">A key</param>
        void OnKeyDown(Keys key);
    }
}