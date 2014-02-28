namespace Wolfenstein.Game.Environment
{
    using System.Drawing;
    using Common.Models;

    /// <summary>
    /// A class for the Door
    /// </summary>
    public class Door : Sprite
    {
        /// <summary>
        /// Initial state of the door
        /// </summary>
        private const bool InitialLockState = true;

        /// <summary>
        /// Initializes a new instance of the Door class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="image">MapImage of the door</param>
        public Door(int x, int y, Image image) : base(x, y, image)
        {
            this.IsLocked = InitialLockState;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the state is locked or unlocked
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
