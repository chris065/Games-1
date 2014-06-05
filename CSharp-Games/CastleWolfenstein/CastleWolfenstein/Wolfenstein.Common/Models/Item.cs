namespace Wolfenstein.Common.Models
{
    using System;

    /// <summary>
    /// A class for the item
    /// </summary>
    public abstract class Item : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the Item class
        /// </summary>
        /// <param name="itemRarity">Item Rarity</param>
        protected Item(ItemRarity itemRarity)
        {
            this.Rarity = itemRarity;
        }

        /// <summary>
        /// Gets the name of the item
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the item rarity
        /// </summary>
        protected ItemRarity Rarity { get; private set; }

        /// <summary>
        /// Clones the item
        /// </summary>
        /// <returns>A cloned item</returns>
        public abstract object Clone();
    }
}
