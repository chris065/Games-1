namespace Wolfenstein.Game.NPC
{
    using System.Drawing;
    using Items;

    /// <summary>
    /// A class for the guard
    /// </summary>
    public class Guard : Enemy
    {
        /// <summary>
        /// Guard damage
        /// </summary>
        private const int GuardDamage = 25;

        /// <summary>
        /// Initializes a new instance of the Guard class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="image">Image</param>
        /// <param name="company">Item delivery company</param>
        public Guard(int x, int y, Bitmap image, ItemDeliveryCompany company)
            : base(x, y, image, company, GuardDamage)
        {
            this.HasVest = false;
        }
    }
}