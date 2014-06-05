namespace Wolfenstein.Game.Items
{
    using System.Collections.Generic;
    using Common;
    using Common.Models;

    /// <summary>
    /// A class for item delivery company
    /// </summary>
    public class ItemDeliveryCompany
    {
        /// <summary>
        /// Min Amount of items that could be received
        /// </summary>
        private const int MinAmountOfItems = 2;

        /// <summary>
        /// Max amount of items that could be received
        /// </summary>
        private const int MaxAmountOfItems = 6;

        /// <summary>
        /// Item factory to create items.
        /// </summary>
        private ItemFactory factory;

        /// <summary>
        /// Initializes a new instance of the ItemDeliveryCompany class
        /// </summary>
        /// <param name="itemFactory">Item factory</param>
        public ItemDeliveryCompany(ItemFactory itemFactory)
        {
            this.factory = itemFactory;
        }

        /// <summary>
        /// Deliver a list of random items - an inventory
        /// </summary>
        /// <returns>A list of random items</returns>
        public List<Item> DeliverInventory()
        {
            int number = RandomGenerator.Integer(MinAmountOfItems, MaxAmountOfItems + 1);
            var items = new List<Item>();

            for (int i = 0; i < number; i++)
            {
                Item item = this.DeliverRandomItem();
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Deliver a bullet
        /// </summary>
        /// <returns>A bullet</returns>
        private Bullet DeliverBullet()
        {
            return this.factory.MakeBullet();
        }

        /// <summary>
        /// Deliver a random armor
        /// </summary>
        /// <returns>A random armor</returns>
        private Armor DeliverArmor()
        {
            return this.factory.MakeArmor();
        }

        /// <summary>
        /// Deliver a gun
        /// </summary>
        /// <returns>A random gun</returns>
        private Gun DeliverGun()
        {
            return this.factory.MakeGun();
        }

        /// <summary>
        /// Deliver a key
        /// </summary>
        /// <returns>A random key</returns>
        private Key DeliverKey()
        {
            return this.factory.MakeKey();
        }

        /// <summary>
        /// Deliver a MedKit
        /// </summary>
        /// <returns>A MedKit</returns>
        private MedKit DeliverMedKit()
        {
            return this.factory.MakeMedKit();
        }

        /// <summary>
        /// Deliver a random item
        /// </summary>
        /// <returns>A random item</returns>
        private Item DeliverRandomItem()
        {
            double chance = RandomGenerator.Percent();

            if (chance <= 10)
            {
                return this.DeliverArmor();
            }

            if (chance > 10 && chance <= 25)
            {
                return this.DeliverGun();
            }

            if (chance > 25 && chance <= 40)
            {
                return this.DeliverMedKit();
            }

            if (chance > 40 && chance <= 55)
            {
                return this.DeliverKey();
            }

            return this.DeliverBullet();
        }


        public Armor DeliverBaseArmor()
        {
            return this.factory.MakeBaseArmor();
        }

        public Gun DeliverBaseGun()
        {
            return this.factory.MakeBaseGun();
        }
    }
}
