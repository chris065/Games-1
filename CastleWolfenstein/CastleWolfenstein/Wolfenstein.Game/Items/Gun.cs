namespace Wolfenstein.Game.Items
{
    using Common;
    using Common.Models;

    /// <summary>
    /// A class for the gun
    /// </summary>
    public class Gun : Item
    {
        /// <summary>
        /// A predefined name for the gun
        /// </summary>
        private const string ItemGun = "Gun";

        /// <summary>
        /// Initializes a new instance of the Gun class
        /// </summary>
        /// <param name="itemRarity">Item Rarity</param>
        /// <param name="damage">Damage</param>
        public Gun(ItemRarity itemRarity, int damage)
            : base(itemRarity)
        {
            this.Damage = damage;
        }

        /// <summary>
        /// Gets the name
        /// </summary>
        public override string Name
        {
            get
            {
                return ItemGun;
            }
        }

        /// <summary>
        /// Gets the damage
        /// </summary>
        public int Damage { get; private set; }

        /// <summary>
        /// A clone method
        /// </summary>
        /// <returns>A new gun</returns>
        public override object Clone()
        {
            return new Gun(this.Rarity, this.Damage);
        }
    }
}
