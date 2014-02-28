using Wolfenstein.Common.Models;

namespace Wolfenstein.Game.Items
{
    using Common;

    /// <summary>
    /// Item Factory. Generates a random item or list of items
    /// </summary>
    public abstract class ItemFactory
    {
        /// <summary>
        /// Makes a bullet
        /// </summary>
        /// <returns>a bullet item</returns>
        public abstract Bullet MakeBullet();

        /// <summary>
        /// Makes an armor
        /// </summary>
        /// <returns>an armor</returns>
        public abstract Armor MakeArmor();

        /// <summary>
        /// Makes a key
        /// </summary>
        /// <returns>Key</returns>
        public abstract Key MakeKey();

        /// <summary>
        /// Makes a Medkit
        /// </summary>
        /// <returns>MedKit</returns>
        public abstract MedKit MakeMedKit();

        /// <summary>
        /// Makes a gun
        /// </summary>
        /// <returns>Gun</returns>
        public abstract Gun MakeGun();

        /// <summary>
        /// Creates a random rarity
        /// </summary>
        /// <returns>RollRarity</returns>
        protected ItemRarity RollRarity()
        {
            double number = RandomGenerator.Percent();

            if (number >= 50)
            {
                return ItemRarity.Common;
            }
            else if (number < 50 && number > 10)
            {
                return ItemRarity.Uncommon;
            }

            return ItemRarity.Rare;
        }

        /// <summary>
        /// Makes a base armor
        /// </summary>
        /// <returns>a base armor</returns>
        public abstract Armor MakeBaseArmor();

        /// <summary>
        /// Makes a base gun
        /// </summary>
        /// <returns>A base gun</returns>
        public abstract Gun MakeBaseGun();
    }
}
