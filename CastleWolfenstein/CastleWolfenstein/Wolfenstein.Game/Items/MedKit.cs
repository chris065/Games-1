namespace Wolfenstein.Game.Items
{
    using Common;
    using Common.Models;

    /// <summary>
    /// A class for the MedKit
    /// </summary>
    public class MedKit : Item
    {
        /// <summary>
        /// A predefined name for the medkit
        /// </summary>
        private const string ItemMedKit = "MedKit";

        /// <summary>
        /// Initializes a new instance of the MedKit class
        /// </summary>
        /// <param name="itemRarity">Item rarity</param>
        public MedKit(ItemRarity itemRarity)
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
                return ItemMedKit;
            }
        }

        /// <summary>
        /// Clones the MedKit
        /// </summary>
        /// <returns>A new MedKit</returns>
        public override object Clone()
        {
            return new MedKit(this.Rarity);
        }
    }
}
