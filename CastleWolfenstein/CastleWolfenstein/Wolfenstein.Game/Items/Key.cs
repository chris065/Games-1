namespace Wolfenstein.Game.Items
{
    using Common;
    using Common.Models;

    /// <summary>
    /// A class for the key
    /// </summary>
    public class Key : Item
    {
        /// <summary>
        /// A predefined name for the key
        /// </summary>
        private const string ItemKey = "Key";

        /// <summary>
        /// Initializes a new instance of the Key class
        /// </summary>
        /// <param name="itemRarity">Item Rarity</param>
        public Key(ItemRarity itemRarity)
            : base(itemRarity)
        {
        }

        /// <summary>
        /// Gets the name
        /// </summary>
        public override string Name
        {
            get
            {
                return ItemKey;
            }
        }

        /// <summary>
        /// Clones the key
        /// </summary>
        /// <returns>A new key</returns>
        public override object Clone()
        {
            return new Key(this.Rarity);
        }
    }
}
