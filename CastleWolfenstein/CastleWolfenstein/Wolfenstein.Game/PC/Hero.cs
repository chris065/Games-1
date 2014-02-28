namespace Wolfenstein.Game.PC
{
    using Common;
    using Common.Interfaces;
    using Common.Models;
    using Common.Structures;
    using Environment;
    using Items;
    using NPC;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;

    /// <summary>
    /// A class for the hero
    /// </summary>
    public class Hero : Character
    {
        /// <summary>
        /// Hero speed
        /// </summary>
        private const int PlayerSpeed = 100;

        /// <summary>
        /// Initial keys
        /// </summary>
        private const int InitialKeys = 0;

        /// <summary>
        /// Initial status for the hero
        /// </summary>
        private const string InitialStatus = "Kill 'em all!";

        /// <summary>
        /// Initial med kits
        /// </summary>
        private const int InitialMedKits = 5;

        /// <summary>
        /// Min heal points
        /// </summary>
        private const int MedKitMinHealPoints = 8;

        /// <summary>
        /// Max heal points
        /// </summary>
        private const int MedKitMaxHealPoints = 21;

        /// <summary>
        /// Health increase when the hero increases the rank
        /// </summary>
        private const int RankUpHealth = 10;

        /// <summary>
        /// Percent damage taken
        /// </summary>
        private const float PercentTakenDamage = 0.1f;

        /// <summary>
        /// Initial bullets
        /// </summary>
        private const int InitialGunBullets = 10;

        /// <summary>
        /// Initials kills to increase the rank
        /// </summary>
        private const int InitialKillsToRankUp = 2;

        /// <summary>
        /// Initial kills
        /// </summary>
        private const int InitialKills = 0;

        /// <summary>
        /// Removes the message/status after this seconds
        /// </summary>
        private const int StatusDurationSec = 4;

        /// <summary>
        /// Removes the medkit after this seconds
        /// </summary>
        private const double MedKitUsingDurationSec = 0.5;

        /// <summary>
        /// Shot sound
        /// </summary>
        private readonly SoundPlayer ShootSound = new SoundPlayer(Resources.barreta);

        /// <summary>
        /// Rank
        /// </summary>
        private Ranks rank;

        /// <summary>
        /// Walk left animation
        /// </summary>
        private AnimTexture walkLeftAnimation;

        /// <summary>
        /// Walk right animation
        /// </summary>
        private AnimTexture walkRightAnimation;

        /// <summary>
        /// Walk up animation
        /// </summary>
        private AnimTexture walkUpAnimation;

        /// <summary>
        /// Walk down animation
        /// </summary>
        private AnimTexture walkDownAnimation;

        /// <summary>
        /// Still animation
        /// </summary>
        private AnimTexture stillAnimation;

        /// <summary>
        /// Hands animation
        /// </summary>
        private AnimTexture hands;

        /// <summary>
        /// Sprite body animation
        /// </summary>
        private Animation spriteBody;

        /// <summary>
        /// Sprite hands animation
        /// </summary>
        private Animation spriteHands;

        /// <summary>
        /// Status/Messages time
        /// </summary>
        private double statusStartTime;

        /// <summary>
        /// Gun flame animation
        /// </summary>
        private AnimTexture gunFlame;

        /// <summary>
        /// Sprite gun flame animation
        /// </summary>
        private Animation spriteGunFlame;

        /// <summary>
        /// Healing start time
        /// </summary>
        private double healingStartTime;

        /// <summary>
        /// If the medkit is used
        /// </summary>
        private bool isMedKitUsed;

        /// <summary>
        /// Initializes a new instance of the Hero class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="sprite">Image</param>
        /// <param name="comnay">Delivery company for the items</param>
        public Hero(int x, int y, Bitmap sprite, ItemDeliveryCompany company)
            : base(x, y, sprite)
        {
            this.Rank = Ranks.Private;
            this.AvailableKeys = InitialKeys;
            this.Armor = company.DeliverBaseArmor();
            this.MedKits = InitialMedKits;
            this.Weapon = company.DeliverBaseGun();
            this.Bullets = InitialGunBullets;
            this.Kills = InitialKills;
            this.KillsToRankUp = InitialKillsToRankUp;
            this.Status = InitialStatus;
            this.LoadTextures();
            this.AvailableKeys = 1;
        }

        /// <summary>
        /// Gets the weapon
        /// </summary>
        public Gun Weapon { get; private set; }

        /// <summary>
        /// Gets the current potions that the hero has
        /// </summary>
        public int MedKits { get; private set; }

        /// <summary>
        /// Gets the keys
        /// </summary>
        public int AvailableKeys { get; private set; }

        /// <summary>
        /// Gets or sets the current experience that the hero has
        /// </summary>
        public Ranks Rank
        {
            get
            {
                return this.rank;
            }

            private set
            {
                if (value != Ranks.Private)
                {
                    this.TotalHealth += RankUpHealth;
                }

                this.rank = value;
            }
        }

        /// <summary>
        /// Gets Status/Message
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// Gets the current bullets that the hero has
        /// </summary>
        public int Bullets { get; private set; }

        /// <summary>
        /// Gets or sets the current kills that the hero has made
        /// </summary>
        private int Kills { get; set; }

        /// <summary>
        /// Gets or sets the kills to next rank
        /// </summary>
        private int KillsToRankUp { get; set; }

        /// <summary>
        /// Gets or sets the chest armor
        /// </summary>
        private Armor Armor { get; set; }

        /// <summary>
        /// Updates the hero state
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="keyboardState">Keyboard state</param>
        public override void Update(GameTime gameTime, IControllerState keyboardState)
        {
            this.Move(gameTime, keyboardState);
            this.OnSpacebar(keyboardState, gameTime);
            this.OnDigitOne(keyboardState, gameTime);
            this.RankUp();
            this.CheckTrigger(gameTime, keyboardState);
            this.SetInitialStatus(gameTime);
        }

        /// <summary>
        /// Draws the hero
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="gameTime">Game time</param>
        public override void Draw(IGraphics graphics, GameTime gameTime)
        {
            this.DrawBody(graphics, gameTime);
            this.DrawHands(graphics);
        }

        /// <summary>
        /// Gets the damage
        /// </summary>
        /// <param name="damage">damage</param>
        public override void GetDamage(int damage)
        {
            damage = damage - (int)(this.Armor.ArmorValue * PercentTakenDamage);
            base.GetDamage(damage);
        }

        /// <summary>
        /// Receives an item 
        /// </summary>
        /// <param name="item">Item</param>
        private void ReceiveItem(Item item)
        {
            if (item is MedKit)
            {
                this.MedKits++;
            }
            else if (item is Gun)
            {
                Gun gun = item as Gun;
                if (gun.Damage > this.Weapon.Damage)
                {
                    this.Weapon = gun;
                }
            }
            else if (item is Armor)
            {
                Armor armor = item as Armor;
                if (armor.ArmorValue > this.Armor.ArmorValue)
                {
                    this.Armor = armor;
                }
            }
            else if (item is Key)
            {
                this.AvailableKeys++;
            }
            else if (item is Bullet)
            {
                this.Bullets++;
            }
        }

        /// <summary>
        /// Inspects objects when space is pressed
        /// </summary>
        /// <param name="keyboardState">key board state</param>
        /// <param name="gameTime">game time</param>
        private void OnSpacebar(IControllerState keyboardState, GameTime gameTime)
        {
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                this.InspectNearbyObjects(gameTime);
            }
        }

        /// <summary>
        /// heals the hero when "1" is pressed
        /// </summary>
        /// <param name="keyboardState">keyboard state</param>
        /// <param name="gameTime">game time</param>
        private void OnDigitOne(IControllerState keyboardState, GameTime gameTime)
        {
            if (keyboardState.IsKeyDown(Keys.D1))
            {
                if (this.MedKits > 0 && !this.isMedKitUsed)
                {
                    this.Heal(gameTime);
                }
            }
            else
            {
                this.PrepareMedKit(gameTime);
            }
        }

        /// <summary>
        /// Heals the character 
        /// </summary>
        private void Heal(GameTime gameTime)
        {
            this.healingStartTime = gameTime.TotalTime.TotalSeconds;
            this.isMedKitUsed = true;
            this.MedKits--;

            var medKitHealPoints = RandomGenerator.Integer(MedKitMinHealPoints, MedKitMaxHealPoints);

            if (this.CurrentHealth + medKitHealPoints > this.TotalHealth)
            {
                this.CurrentHealth = this.TotalHealth;
            }
            else
            {
                this.CurrentHealth += medKitHealPoints;
            }
        }

        /// <summary>
        /// Retrieves a list of objects surrounding the character.
        /// </summary>
        private void InspectNearbyObjects(GameTime gameTime)
        {
            IEnumerable<Sprite> nearbyObjects = Collisions.GetObjects(this, SearchPattern.Touching);

            foreach (var obj in nearbyObjects)
            {
                var door = obj as Door;
                var receivedItemsNames = new HashSet<string>();

                // check for door
                if ((object)door != null)
                {
                    string doorStatus = string.Empty;

                    if (this.AvailableKeys > 0)
                    {
                        door.IsLocked = false;
                        this.AvailableKeys--;
                    }
                    else
                    {
                        doorStatus = "You don't have a key!";
                        this.ChangeStatus(gameTime, doorStatus);
                    }
                }
                else
                {
                    // check for lootable objects
                    var lootedObject = obj as ILootable;

                    if ((object)lootedObject != null)
                    {
                        var receivedItems = lootedObject.GetContent();

                        if (receivedItems.Count > 0)
                        {
                            for (int i = 0; i < receivedItems.Count; i++)
                            {
                                this.ReceiveItem(receivedItems[i]);

                                receivedItemsNames.Add(receivedItems[i].Name);
                            }
                        }
                    }
                }

                if (receivedItemsNames.Count > 0)
                {
                    this.ChangeStatus(gameTime, string.Format("You found a {0}!", string.Join(", ", receivedItemsNames)));
                }
            }
        }

        /// <summary>
        /// Checks the trigger
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="keyboardState">Keyboard state</param>
        private void CheckTrigger(GameTime gameTime, IControllerState keyboardState)
        {
            if (this.Bullets > 0 && !this.GunOut)
            {
                if (this.ShootKeyPressed(keyboardState))
                {
                    this.Shoot(gameTime);
                }
            }
            else
            {
                this.HolsterWeapon(gameTime);
            }
        }

        /// <summary>
        /// Checks if the key for shooting is pressed
        /// </summary>
        /// <param name="keyboardState">Keyboard state</param>
        /// <returns></returns>
        private bool ShootKeyPressed(IControllerState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                this.ShootDirection = SearchPattern.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                this.ShootDirection = SearchPattern.Right;
            }
            else if (keyboardState.IsKeyDown(Keys.W))
            {
                this.ShootDirection = SearchPattern.Up;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                this.ShootDirection = SearchPattern.Down;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to shoot
        /// </summary>
        /// <param name="gameTime">Game time</param>
        private void Shoot(GameTime gameTime)
        {
            this.GunOut = true;
            this.ShootStartTime = gameTime.TotalTime.TotalSeconds;

            // Make the actual shot
            this.Bullets--;
            this.ShootSound.Play();

            IEnumerable<Sprite> targetList = Collisions.GetObjects(this, this.ShootDirection);
            foreach (var obj in targetList)
            {
                var target = obj as Enemy;

                if (target != null)
                {
                    target.GetDamage(this.Weapon.Damage);

                    if (!target.IsAlive)
                    {
                        // The scream sound is played in the target GetDamage method
                        this.ChangeStatus(gameTime, "Die Nazi!");
                        this.Kills++;
                    }
                }
            }
        }

        /// <summary>
        /// Increase the rank
        /// </summary>
        private void RankUp()
        {
            if (this.Kills == this.KillsToRankUp && this.Rank != Ranks.General)
            {
                this.Rank++;
                this.KillsToRankUp++;
                this.Kills = InitialKills;
            }
        }

        /// <summary>
        /// Move the Player
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="keyboardState">Keyboard state</param>
        private void Move(GameTime gameTime, IControllerState keyboardState)
        {
            PointF existingPosition = this.Position;
            Point existingMapPosition = this.MapPosition;

            var move = (float)(PlayerSpeed * gameTime.ElapsedTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.Position = new PointF(this.Position.X - move, this.Position.Y);
                this.WalkDirection = Direction.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.Position = new PointF(this.Position.X + move, this.Position.Y);
                this.WalkDirection = Direction.Right;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.Position = new PointF(this.Position.X, this.Position.Y - move);
                this.WalkDirection = Direction.Up;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.Position = new PointF(this.Position.X, this.Position.Y + move);
                this.WalkDirection = Direction.Down;
            }
            else
            {
                this.WalkDirection = Direction.Still;
                return;
            }

            // The position on the map is represented by two integers (pixel accuracy)
            this.MapPosition = new Point((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y));

            // Check if we can move on the next position
            // (not leaving the map and not colliding with walls or other objects)
            if (Collisions.IsNotCollding(this))
            {
                // Player was moved
                return;
            }

            // Player cannot go here, return the old coords
            this.Position = existingPosition;
            this.MapPosition = existingMapPosition;
        }

        /// <summary>
        /// Load textures
        /// </summary>
        private void LoadTextures()
        {
            this.walkLeftAnimation = new AnimTexture(Resources.player48x48_Walking_Left, 0.2f, true, 48);
            this.walkRightAnimation = new AnimTexture(Resources.player48x48_Walking_Right, 0.2f, true, 48);
            this.walkUpAnimation = new AnimTexture(Resources.player48x48_Walking_Up, 0.2f, true, 48);
            this.walkDownAnimation = new AnimTexture(Resources.player48x48_Walking_Down, 0.2f, true, 48);
            this.stillAnimation = new AnimTexture(Resources.player48x48_Idle, 0.2f, true, 48);
            this.hands = new AnimTexture(Resources.player48x240_Hands, 0.2f, true, 48);
            this.gunFlame = new AnimTexture(Resources.Gun_Flame, 0.2f, true, 48);
        }

        /// <summary>
        /// Draws the body
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="gameTime">Game time</param>
        private void DrawBody(IGraphics graphics, GameTime gameTime)
        {
            switch (this.WalkDirection)
            {
                case Direction.Up:
                    this.spriteBody.PlayAnimation(this.walkUpAnimation);
                    break;
                case Direction.Left:
                    this.spriteBody.PlayAnimation(this.walkLeftAnimation);
                    break;
                case Direction.Down:
                    this.spriteBody.PlayAnimation(this.walkDownAnimation);
                    break;
                case Direction.Right:
                    this.spriteBody.PlayAnimation(this.walkRightAnimation);
                    break;
                case Direction.Still:
                    this.spriteBody.PlayAnimation(this.stillAnimation);
                    break;
                default:
                    throw new ApplicationException("Direction not supported!");
            }

            this.spriteBody.Draw(gameTime, graphics, this.Position, false);
        }

        /// <summary>
        /// Draw hands
        /// </summary>
        /// <param name="graphics">Graphics</param>
        private void DrawHands(IGraphics graphics)
        {
            this.spriteHands.PlayAnimation(this.hands);
            this.spriteGunFlame.PlayAnimation(this.gunFlame);
            if (this.GunOut)
            {
                PointF flamePosition;
                switch (this.ShootDirection)
                {
                    case SearchPattern.Left:
                        this.spriteHands.DrawFrame(graphics, this.Position, 1);
                        flamePosition = new PointF(this.Position.X - 16, this.Position.Y + 15);
                        break;
                    case SearchPattern.Right:
                        this.spriteHands.DrawFrame(graphics, this.Position, 2);
                        flamePosition = new PointF(this.Position.X + 48, this.Position.Y + 15);
                        break;
                    case SearchPattern.Up:
                        this.spriteHands.DrawFrame(graphics, this.Position, 3);
                        flamePosition = new PointF(this.Position.X + 32, this.Position.Y - 1);
                        break;
                    case SearchPattern.Down:
                        this.spriteHands.DrawFrame(graphics, this.Position, 4);
                        flamePosition = new PointF(this.Position.X + 32, this.Position.Y + 31);
                        break;
                    default:
                        throw new ApplicationException("Shoot direction not supported!");
                }

                this.spriteGunFlame.DrawFrame(graphics, flamePosition, 0);
            }
            else
            {
                this.spriteHands.DrawFrame(graphics, this.Position, 0);
            }
        }

        /// <summary>
        /// Set Initial message/status
        /// </summary>
        /// <param name="gameTime">Game time</param>
        private void SetInitialStatus(GameTime gameTime)
        {
            if (gameTime.TotalTime.TotalSeconds - this.statusStartTime > StatusDurationSec)
            {
                this.Status = string.Empty;
            }
        }

        /// <summary>
        /// Changes the status/message
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="newStatus">New status/message</param>
        private void ChangeStatus(GameTime gameTime, string newStatus)
        {
            this.Status = newStatus;
            this.statusStartTime = gameTime.TotalTime.TotalSeconds;
        }

        /// <summary>
        /// Prepares the med kit
        /// </summary>
        /// <param name="gameTime">Game time</param>
        private void PrepareMedKit(GameTime gameTime)
        {
            if (this.isMedKitUsed &&
                gameTime.TotalTime.TotalSeconds - this.healingStartTime > MedKitUsingDurationSec)
            {
                this.isMedKitUsed = false;
            }
        }
    }
}