using Wolfenstein.Common.Structures;

namespace Wolfenstein.Common.Models
{
    using System.Drawing;
    using Interfaces;

    /// <summary>
    /// A character class for NPCs and PCs
    /// </summary>
    public abstract class Character : Sprite, ILiving
    {
        /// <summary>
        /// Initial total health of the characters
        /// </summary>
        private const int InitialTotalHealth = 100;

        /// <summary>
        /// Shoot message duration
        /// </summary>
        private const double ShootDurationSec = 0.5;

        /// <summary>
        /// Initial current health of the characters
        /// </summary>
        private const int InitialCurrentHealth = 100;

        /// <summary>
        /// Current health of the character
        /// </summary>
        private int currentHealth;

        /// <summary>
        /// Initializes a new instance of the Character class
        /// </summary>
        /// <param name="x">A coordinate X</param>
        /// <param name="y">A coordinate Y</param>
        /// <param name="image">An image</param>
        protected Character(int x, int y, Bitmap image)
            : base(x, y, image)
        {
            this.CurrentHealth = InitialCurrentHealth;
            this.TotalHealth = InitialTotalHealth;
        }

        /// <summary>
        /// Gets or set the total health of the character
        /// </summary>
        public int TotalHealth { get; protected set; }

        /// <summary>
        /// Gets the current health of a character
        /// </summary>
        public int CurrentHealth
        {
            get
            {
                return this.currentHealth;
            }

            protected set
            {
                if (value < 0)
                {
                    this.currentHealth = 0;
                }

                this.currentHealth = value;
            }
        }

        /// <summary>
        /// Is gun is out for image change
        /// </summary>
        protected bool GunOut { get; set; }

        /// <summary>
        /// Starting time of the shot
        /// </summary>
        protected double ShootStartTime { get; set; }

        /// <summary>
        /// Shot direction
        /// </summary>
        protected SearchPattern ShootDirection { get; set; }

        /// <summary>
        /// Walk Direction
        /// </summary>
        protected Direction WalkDirection { get; set; }

        /// <summary>
        /// Gets if the character is alive
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return this.CurrentHealth > 0;
            }
        }

        /// <summary>
        /// Removes part of the current health based on the damage
        /// </summary>
        /// <param name="damage"></param>
        public virtual void GetDamage(int damage)
        {
            this.CurrentHealth -= damage;
        }

        /// <summary>
        /// Method to update the character
        /// </summary>
        /// <param name="gameTime">A game time</param>
        /// <param name="keyboardState">A keyboard state</param>
        public abstract void Update(GameTime gameTime, IControllerState keyboardState);

        /// <summary>
        /// Holster weapon
        /// </summary>
        /// <param name="gameTime">Game time</param>
        protected void HolsterWeapon(GameTime gameTime)
        {
            // Check if enough time has passed to holster the weapon
            if (gameTime.TotalTime.TotalSeconds - this.ShootStartTime > ShootDurationSec)
            {
                this.GunOut = false;
            }
        }
    }
}