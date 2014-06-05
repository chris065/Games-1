using Wolfenstein.Common.Structures;

namespace Wolfenstein.Game.Environment
{
    using Common;
    using Common.Interfaces;
    using Common.Models;
    using Items;
    using Properties;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// A class for the chest
    /// </summary>
    public class Chest : Sprite, ILootable
    {
        /// <summary>
        /// Open/close animation
        /// </summary>
        private AnimTexture openCloseAnimation;

        /// <summary>
        /// Animation sprite
        /// </summary>
        private Animation sprite;

        /// <summary>
        /// If chest is open
        /// </summary>
        private bool isOpen = false;

        /// <summary>
        /// Initializes a new instance of the Chest class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="image">Image of the chest</param>
        /// <param name="company">Delivery company for the inventory</param>
        public Chest(int x, int y, Image image, ItemDeliveryCompany company)
            : base(x, y, image)
        {
            this.Inventory = company.DeliverInventory();
            this.openCloseAnimation = new AnimTexture(Resources.Chest_Close_Open, 0.2f, true, 48);
        }

        /// <summary>
        /// Gets or sets the inventory
        /// </summary>
        private List<Item> Inventory { get; set; }

        /// <summary>
        /// The hero takes the inventory of the chest
        /// </summary>
        /// <returns>A list of items</returns>
        public List<Item> GetContent()
        {
            this.isOpen = true;
            var returnedInventory = new List<Item>();

            for (int i = 0; i < this.Inventory.Count; i++)
            {
                returnedInventory.Add(this.Inventory[i].Clone() as Item);
            }

            this.Inventory.Clear();

            return returnedInventory;
        }

        /// <summary>
        /// Draws the chest
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="gameTime">Game Time</param>
        public override void Draw(IGraphics graphics, GameTime gameTime)
        {
            int frame = this.isOpen ? 1 : 0;
            this.sprite.LoadAnimation(this.openCloseAnimation);
            this.sprite.DrawFrame(graphics, this.Position, frame);
        }
    }
}