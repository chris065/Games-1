namespace Wolfenstein.Common.Interfaces
{
    /// <summary>
    /// Interface IAlive for live creatures in the game
    /// </summary>
    public interface ILiving
    {
       /// <summary>
        /// Returns if the creature is alive.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Reduces the health of the creature by the amount of damage.
        /// </summary>
        /// <param name="damage"></param>
        void GetDamage(int damage);

        /// <summary>
        /// Method to update the character
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboardState"></param>
        void Update(GameTime gameTime, IControllerState keyboardState);
    }
}