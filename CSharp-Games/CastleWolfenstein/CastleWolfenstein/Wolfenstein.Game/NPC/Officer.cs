namespace Wolfenstein.Game.NPC
{
    using System.Drawing;
    using Items;

    /// <summary>
    /// A class for the officer
    /// </summary>
    public class Officer : Enemy
    {
        /// <summary>
        /// Officer damage
        /// </summary>
        private const int OfficerDamage = 40;

        /// <summary>
        /// Initializes a new instance of the Officer class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="image">Image</param>
        /// <param name="company">Item delivery company</param>
        public Officer(int x, int y, Bitmap image, ItemDeliveryCompany company)
            : base(x, y, image, company, OfficerDamage)
        {
        }
    }
}