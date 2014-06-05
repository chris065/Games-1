using Wolfenstein.Common.Models;

namespace Wolfenstein.Game.Items
{
    using Common;

    /// <summary>
    /// A class for Extraordinary factory
    /// </summary>
    public class ItemExtraordinaire : ItemFactory
    {
        /// <summary>
        /// Armor value for a common armor
        /// </summary>
        private const int CommonArmor = 35;

        /// <summary>
        /// Armor value for a uncommon armor.
        /// </summary>
        private const int UncommonArmor = 55;

        /// <summary>
        /// Armor value for a rare armor.
        /// </summary>
        private const int RareArmor = 75;

        /// <summary>
        /// Damage value for a common gun.
        /// </summary>
        private const int CommonGun = 35;

        /// <summary>
        /// Damage value for an uncommon gun.
        /// </summary>
        private const int UncommonGun = 55;

        /// <summary>
        /// Damage value for a rare gun.
        /// </summary>
        private const int RareGun = 75;

        /// <summary>
        /// Make a bullet
        /// </summary>
        /// <returns>A bullet</returns>
        public override Bullet MakeBullet()
        {
            return new Bullet(ItemRarity.Common);
        }

        /// <summary>
        /// Makes an armor
        /// </summary>
        /// <returns>A random armor</returns>
        public override Armor MakeArmor()
        {
            ItemRarity itemRarity = RollRarity();

            if (itemRarity == ItemRarity.Uncommon)
            {
                return new Armor(itemRarity, UncommonArmor);
            }
            else if (itemRarity == ItemRarity.Rare)
            {
                return new Armor(itemRarity, RareArmor);
            }

            return new Armor(itemRarity, CommonArmor);
        }

        /// <summary>
        /// Makes a key
        /// </summary>
        /// <returns>a key</returns>
        public override Key MakeKey()
        {
            return new Key(ItemRarity.Common);
        }

        /// <summary>
        /// Makes a medKit
        /// </summary>
        /// <returns>a med kit</returns>
        public override MedKit MakeMedKit()
        {
            return new MedKit(ItemRarity.Common);
        }

        /// <summary>
        /// Makes a gun
        /// </summary>
        /// <returns>A random gun</returns>
        public override Gun MakeGun()
        {
            ItemRarity itemRarity = RollRarity();

            if (itemRarity == ItemRarity.Uncommon)
            {
                return new Gun(itemRarity, UncommonGun);
            }
            
            if (itemRarity == ItemRarity.Rare)
            {
                return new Gun(itemRarity, RareGun);
            }

            return new Gun(itemRarity, CommonGun);
        }

        public override Armor MakeBaseArmor()
        {
            return new Armor(ItemRarity.Common, CommonArmor);
        }

        public override Gun MakeBaseGun()
        {
            return new Gun(ItemRarity.Common, CommonGun);
        }
    }
}
