namespace Wolfenstein.Game.Items
{
    using Common;
    using Common.Models;

    /// <summary>
    /// A class for the armor
    /// </summary>
    public class Armor : Item
    {
        /// <summary>
        /// A predefined name for the armor
        /// </summary>
        private const string ItemArmor = "Armor";

        /// <summary>
        /// Initializes a new instance of the Armor class
        /// </summary>
        /// <param name="itemRarity">Rarity of the item</param>
        /// <param name="armorValue">Armor value</param>
        public Armor(ItemRarity itemRarity, int armorValue)
            : base(itemRarity)
        {
            this.ArmorValue = armorValue;
        }

        /// <summary>
        /// Gets the name
        /// </summary>
        public override string Name
        {
            get
            {
                return ItemArmor;
            }
        }

        /// <summary>
        /// Gets the armor value
        /// </summary>
        public int ArmorValue { get; private set; }

        /// <summary>
        /// Clones an armor
        /// </summary>
        /// <returns>A new armor</returns>
        public override object Clone()
        {
            return new Armor(this.Rarity, this.ArmorValue);
        }
    }
}
