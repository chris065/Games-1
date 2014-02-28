namespace Wolfenstein.Game.Items
{
    using Common;
    using Common.Models;

    /// <summary>
    /// A class for the Bullet
    /// </summary>
    public class Bullet : Item
    {
        /// <summary>
        /// A predefined name for the armor
        /// </summary>
        private const string ItemBullet = "Bullet";

        /// <summary>
        /// Initializes a new instance of the Bullet class
        /// </summary>
        /// <param name="itemRarity">Item Rarity</param>
        public Bullet(ItemRarity itemRarity) : base(itemRarity)
        {
        }

        /// <summary>
        /// Gets the name of the bullet
        /// </summary>
        public override string Name
        {
            get { return ItemBullet; }
        }

        /// <summary>
        /// Clones a bullet
        /// </summary>
        /// <returns>A cloned bullet</returns>
        public override object Clone()
        {
            return new Bullet(this.Rarity);
        }
    }
}
