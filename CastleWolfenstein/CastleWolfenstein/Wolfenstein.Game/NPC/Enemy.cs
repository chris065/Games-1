namespace Wolfenstein.Game.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Common;
    using Common.Interfaces;
    using Common.Models;
    using Items;
    using PC;
    using Properties;
    using System.Media;
    using Environment;
    using Common.Structures;

    /// <summary>
    /// A class for the enemy
    /// </summary>
    public class Enemy : Character, ILootable
    {
        /// <summary>
        /// A speed of the enemies
        /// </summary>
        private const int Speed = 72;

        /// <summary>
        /// Gun range in tiles
        /// </summary>
        private const int GunRange = 9;

        /// <summary>
        /// Initial bullets
        /// </summary>
        private const int IniitialBullets = 4;

        /// <summary>
        /// Animations
        /// </summary>
        private AnimTexture walkLeftAnimation;

        /// <summary>
        /// Animation
        /// </summary>
        private AnimTexture walkRightAnimation;

        /// <summary>
        /// Animation
        /// </summary>
        private AnimTexture walkUpAnimation;

        /// <summary>
        /// Animation
        /// </summary>
        private AnimTexture walkDownAnimation;

        /// <summary>
        /// Animation
        /// </summary>
        private AnimTexture stillAnimation;

        /// <summary>
        /// Animation
        /// </summary>
        private AnimTexture hands;

        /// <summary>
        /// Animation
        /// </summary>
        private AnimTexture gunFlame;

        /// <summary>
        /// Animation
        /// </summary>
        private Animation spriteBody;

        /// <summary>
        /// Animation
        /// </summary>
        private Animation spriteHands;

        /// <summary>
        /// Animation
        /// </summary>
        private Animation spriteGunFlame;

        /// <summary>
        /// Animation
        /// </summary>
        private Bitmap vest;
    
        private bool isHunting;

        /// <summary>
        /// Initializes a new instance for the Enemy class
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="image">Image</param>
        protected Enemy(int x, int y, Bitmap image, ItemDeliveryCompany company, int gunDamage)
            : base(x, y, image)
        {
            this.WalkDirection = RandomGenerator.Direction();
            this.Inventory = company.DeliverInventory();
            this.Bullets = IniitialBullets;
            this.HasVest = true;
            this.GunDamage = gunDamage;
            this.LoadTextures();
        }

        /// <summary>
        ///  
        /// </summary>
        protected bool HasVest { get; set; }

        /// <summary>
        /// Gets or sets the inventory of the enemy.
        /// </summary>
        private List<Item> Inventory { get; set; }

        /// <summary>
        /// Gets or sets the gun damage
        /// </summary>
        private int GunDamage { get; set; }

        /// <summary>
        /// Gets or sets the bullets
        /// </summary>
        private int Bullets { get; set; }

        /// <summary>
        /// Updates the enemy
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        /// <param name="keyboardState">Keyboard state</param>
        public override void Update(GameTime gameTime, IControllerState keyboardState)
        {
            this.HolsterWeapon(gameTime);

            if (this.IsAlive)
            {
                if (Collisions.PlayerIsVisible(this, this.WalkDirection))
                {
                    double angleInDegrees = Collisions.GetBearing(this, Collisions.GetPlayer());
                    int tileRange = Collisions.GetTileRange(this, Collisions.GetPlayer());
                    Console.WriteLine("Player spotted: {0:F1}", angleInDegrees);
                    this.Chase(gameTime, angleInDegrees, tileRange);
                }
                else
                {
                    this.HolsterWeapon(gameTime);
                    this.isHunting = false;
                    this.MoveRandom(gameTime);
                }
            }
            else
            {
                this.WalkDirection = Direction.Still;
            }
        }

        /// <summary>
        /// Draws the enemy
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public override void Draw(IGraphics graphics, GameTime gameTime)
        {
            if (!Level.GameIsRunning)
            {
                this.WalkDirection = Direction.Still;
            }

            this.DrawBody(graphics, gameTime);
            if (this.IsAlive)
            {
                this.DrawHands(graphics);
            }
        }

        /// <summary>
        /// Transfers the current inventory to the hero
        /// </summary>
        /// <returns>a list of items</returns>
        public List<Item> GetContent()
        {
            var returnedInventory = new List<Item>();
            if (this.CurrentHealth <= 0)
            {
                for (int i = 0; i < this.Inventory.Count; i++)
                {
                    returnedInventory.Add(this.Inventory[i].Clone() as Item);
                }

                this.Inventory.Clear();
            }

            return returnedInventory;
        }

        /// <summary>
        /// Removes damage from enemy current health
        /// </summary>
        /// <param name="damage">amount of damage</param>
        public override void GetDamage(int damage)
        {
            base.GetDamage(damage);
            if (!this.IsAlive && this.WalkDirection != Direction.Still)
            {
                // SoundPlayer scream = new SoundPlayer(Resources.scream);
                // scream.Play();
            }
        }

        /// <summary>
        /// Tries to move
        /// </summary>
        /// <param name="direction">direction</param>
        /// <param name="move">move amount</param>
        /// <returns></returns>
        private bool TryMove(Direction direction, float move)
        {
            PointF exitingPosition = this.Position;
            Point existingMapPosition = this.MapPosition;

            switch (direction)
            {
                case Direction.Left:
                    this.Position = new PointF(this.Position.X - move, this.Position.Y);
                    break;
                case Direction.Right:
                    this.Position = new PointF(this.Position.X + move, this.Position.Y);
                    break;
                case Direction.Up:
                    this.Position = new PointF(this.Position.X, this.Position.Y - move);
                    break;
                case Direction.Down:
                    this.Position = new PointF(this.Position.X, this.Position.Y + move);
                    break;
            }

            // The position on the map is represented by two integers (pixel accuracy)
            this.MapPosition = new Point((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y));

            // Check if we can move on the next position
            // (not leaving the map and not colliding with walls or other objects)
            if (Collisions.IsNotCollding(this))
            {
                // Player was moved
                this.WalkDirection = direction;
                return true;
            }

            // Player cannot go here, return the old coords,
            this.Position = exitingPosition;
            this.MapPosition = existingMapPosition;

            return false;
        }

        /// <summary>
        /// Chases the hero
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="angleInDegrees">angles</param>
        /// <param name="tileRange">tile range</param>
        private void Chase(GameTime gameTime, double angleInDegrees, int tileRange)
        {
            if (angleInDegrees > -5 && angleInDegrees < 5)
            {
                this.TryDirectRoute(gameTime, angleInDegrees);
                this.ShootDirection = SearchPattern.Right;
                if (tileRange < GunRange)
                {
                    this.CheckTrigger(gameTime);
                }
            }
            else if (angleInDegrees > 85 && angleInDegrees < 95)
            {
                this.TryDirectRoute(gameTime, angleInDegrees);
                this.ShootDirection = SearchPattern.Up;
                if (tileRange < GunRange)
                {
                    this.CheckTrigger(gameTime);
                }
            }
            else if (angleInDegrees > -95 && angleInDegrees < -85)
            {
                this.TryDirectRoute(gameTime, angleInDegrees);
                this.ShootDirection = SearchPattern.Down;
                if (tileRange < GunRange)
                {
                    this.CheckTrigger(gameTime);
                }
            }
            else if (angleInDegrees > 175 || angleInDegrees < -175)
            {
                this.TryDirectRoute(gameTime, angleInDegrees);
                this.ShootDirection = SearchPattern.Left;
                if (tileRange < GunRange)
                {
                    this.CheckTrigger(gameTime);
                }
            }
            else
            {
                this.TrySidewaysRoute(gameTime, angleInDegrees);
            }
        }

        private void TryDirectRoute(GameTime gameTime, double angleInDegrees)
        {
            var move = (float)(Speed * gameTime.ElapsedTime.TotalSeconds);

            if (!this.TryMove(this.GetShortestDirection(angleInDegrees), move))
            {
                if (!this.TryMove(this.GetLongerDirection(angleInDegrees), move))
                {
                    this.WalkDirection = RandomGenerator.Direction();
                }
            }
        }

        private void TrySidewaysRoute(GameTime gameTime, double angleInDegrees)
        {
            var move = (float)(Speed * gameTime.ElapsedTime.TotalSeconds);

            if (!this.TryMove(this.GetLongerDirection(angleInDegrees), move))
            {
                if (!this.TryMove(this.GetShortestDirection(angleInDegrees), move))
                {
                    this.WalkDirection = RandomGenerator.Direction();
                }
            }
        }

        /// <summary>
        /// Directly towards the target
        /// </summary>
        private Direction GetShortestDirection(double angleInDegrees)
        {
            if (angleInDegrees >= -45 && angleInDegrees <= 45)
            {
                return Direction.Right;
            }
            else if (angleInDegrees > 45 && angleInDegrees <= 135)
            {
                return Direction.Up;
            }
            else if (angleInDegrees > 135 || angleInDegrees <= -135)
            {
                return Direction.Left;
            }
            else if (angleInDegrees > -135 && angleInDegrees < -45)
            {
                return Direction.Down;
            }
            else
            {
                throw new ApplicationException("You have missed a direction!");
            }
        }

        /// <summary>
        /// Sideways to get to the shooting position
        /// </summary>
        private Direction GetLongerDirection(double angleInDegrees)
        {
            if (angleInDegrees >= 0 && angleInDegrees <= 45 || angleInDegrees >= 135)
            {
                return Direction.Up;
            }
            else if ((angleInDegrees > 45 && angleInDegrees <= 90) || (angleInDegrees <= -45 && angleInDegrees >= -90))
            {
                return Direction.Right;
            }
            else if (angleInDegrees < 0 && angleInDegrees > -45 || angleInDegrees <= -135)
            {
                return Direction.Down;
            }
            else if ((angleInDegrees < 135 && angleInDegrees > 90) || (angleInDegrees > -135 && angleInDegrees < -90))
            {
                return Direction.Left;
            }
            else
            {
                throw new ApplicationException("You have missed a direction!");
            }
        }

        /// <summary>
        /// Loads the textures
        /// </summary>
        private void LoadTextures()
        {
            // Load animated textures.
            this.walkLeftAnimation = new AnimTexture(Resources.guard48x48_Walking_Left, 0.2f, true, 48);
            this.walkRightAnimation = new AnimTexture(Resources.guard48x48_Walking_Right, 0.2f, true, 48);
            this.walkUpAnimation = new AnimTexture(Resources.guard48x48_Walking_Up, 0.2f, true, 48);
            this.walkDownAnimation = new AnimTexture(Resources.guard48x48_Walking_Down, 0.2f, true, 48);
            this.stillAnimation = new AnimTexture(Resources.Guard48x96_AliveDead, 0.2f, true, 48);
            this.hands = new AnimTexture(Resources.player48x240_Hands, 0.2f, true, 48);
            this.gunFlame = new AnimTexture(Resources.Gun_Flame, 0.2f, true, 48);
            this.vest = Resources.Vest;
        }

        /// <summary>
        /// Moves random
        /// </summary>
        /// <param name="gameTime">game time</param>
        private void MoveRandom(GameTime gameTime)
        {
            PointF exitingPosition = this.Position;
            Point existingMapPosition = this.MapPosition;

            var move = (float)(Speed * gameTime.ElapsedTime.TotalSeconds);

            switch (this.WalkDirection)
            {
                case Direction.Left:
                    this.Position = new PointF(this.Position.X - move, this.Position.Y);
                    break;
                case Direction.Right:
                    this.Position = new PointF(this.Position.X + move, this.Position.Y);
                    break;
                case Direction.Up:
                    this.Position = new PointF(this.Position.X, this.Position.Y - move);
                    break;
                case Direction.Down:
                    this.Position = new PointF(this.Position.X, this.Position.Y + move);
                    break;
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

            // Player cannot go here, return the old coords,
            this.Position = exitingPosition;
            this.MapPosition = existingMapPosition;

            if (!this.isHunting)
            {
                this.WalkDirection = RandomGenerator.Direction();
            }
        }

        /// <summary>
        /// Check trigger
        /// </summary>
        /// <param name="gameTime">game time</param>
        private void CheckTrigger(GameTime gameTime)
        {
            if (this.Bullets > 0 && !this.GunOut)
            {
                this.Shoot(gameTime);
            }
        }

        /// <summary>
        /// Shoot
        /// </summary>
        /// <param name="gameTime">game time</param>
        private void Shoot(GameTime gameTime)
        {
            this.GunOut = true;
            this.ShootStartTime = gameTime.TotalTime.TotalSeconds;

            // Make the actual shot
            this.Bullets--;
            var shootSound = new SoundPlayer(Resources.barreta);
            shootSound.Play();

            IEnumerable<Sprite> targetList = Collisions.GetObjects(this, this.ShootDirection);
            foreach (var target in targetList)
            {
                if (target is Hero)
                {
                    (target as Hero).GetDamage(this.GunDamage);
                    Console.WriteLine("{0} killed!", target.GetType().Name);
                }
            }
        }

        /// <summary>
        /// Draws the  body
        /// </summary>
        /// <param name="graphics">graphics</param>
        /// <param name="gameTime">game time</param>
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

            if (this.WalkDirection == Direction.Still)
            {
                int frame = this.CurrentHealth > 0 ? 0 : 1;
                this.spriteBody.DrawFrame(graphics, this.Position, frame);
            }
            else
            {
                this.spriteBody.Draw(gameTime, graphics, this.Position, false);
            }

            if (this.CurrentHealth > 0 && this.HasVest)
            {
                graphics.DrawImage(this.vest, (int)this.Position.X, (int)this.Position.Y, this.vest.Width, this.vest.Height);
            }
        }

        /// <summary>
        /// Draws the hands
        /// </summary>
        /// <param name="graphics">graphics</param>
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
    }
}