namespace Wolfenstein.Game.Environment
{
    using Common;
    using Common.Interfaces;
    using Common.Models;
    using Items;
    using NPC;
    using PC;
    using Properties;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// A class for the level
    /// </summary>
    public class Level
    {
        private const int MapXoffset = 20;
        private const int MapYoffset = 20;
        private const int TextscreenHeight = 40;
        private const string DeadMessage = "GAME OVER";
        private const int TileSize = 16;

        private static readonly ItemFactory Factory = new ItemExtraordinaire();
        private static readonly Bitmap BmpTileBlack = Resources.Tile16x16Black;
        private static readonly Bitmap BmpTileWall = Resources.Tile16x16Wall_Red;

        private readonly Font textFont = new Font("Consolas", 12, FontStyle.Bold);
        private readonly SolidBrush textBrush = new SolidBrush(Color.FromArgb(255, 255, 85));
        private readonly ItemDeliveryCompany company = new ItemDeliveryCompany(Factory);

        private Dictionary<TileType, Bitmap> tileImages = new Dictionary<TileType, Bitmap> { { TileType.EmptyTile, BmpTileBlack }, { TileType.WallTile, BmpTileWall }, { TileType.Back, BmpTileBlack }, { TileType.Next, BmpTileBlack }, { TileType.Door, BmpTileBlack } };
        private Bitmap mapBuffer;
        private IGraphics mapGraphics;
        private SolidBrush statusTextBrush;
        private double fps = 0;
        private Bitmap mapImage;
        private TileType[,] wallMap; // map for the walls
        private List<Sprite> allObjects; // Holds all objects in the level (Player, enemies, chests etc.)
        private int levelIndex;
        private Door door;

        /// <summary>
        /// Initializes a new instance of the Level class
        /// </summary>
        public Level()
        {
            this.levelIndex = 0;
            this.LoadMapFromFile();
            this.BuildMapImage();
            this.InitializeCollisions();
            this.mapBuffer = new Bitmap(this.MapSize.Width, this.MapSize.Height);
            this.mapGraphics = new GDIGraphics(this.mapBuffer);
            GameIsRunning = true;
        }

        public static bool GameIsRunning { get; private set; }

        public Size ClientSize { get; private set; }

        public int LevelIndex
        {
            get
            {
                return this.levelIndex;
            }

            set
            {
                this.levelIndex = value;
            }
        }

        public Hero Player { get; private set; }

        private Size MapSize { get; set; }

        /// <summary>
        /// Updates the level
        /// </summary>
        public void UpdateLevel()
        {
            this.LoadMapFromFile();
            this.BuildMapImage();
            this.InitializeCollisions();
        }

        /// <summary>
        /// Updates all characters in the game
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="keyboardState">Keyboard state</param>
        public void Update(GameTime gameTime, IControllerState keyboardState)
        {
            if (this.Player.IsAlive)
            {
                Door doorToRemove = null;

                foreach (Sprite sprite in this.allObjects)
                {
                    if (sprite is ILiving)
                    {
                        (sprite as ILiving).Update(gameTime, keyboardState);
                    }
                    else if (sprite is Door)
                    {
                        var door = sprite as Door;
                        if (!door.IsLocked)
                        {
                            doorToRemove = door;
                        }
                    }
                }

                if (doorToRemove != null)
                {
                    this.allObjects.Remove(doorToRemove);
                }
            }
            else
            {
                GameIsRunning = false;
            }
        }

        /// <summary>
        /// Draws the sprites
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="renderer">Graphics</param>
        public void Draw(GameTime gameTime, IGraphics renderer)
        {
            // Draw the walls to the map buffer
            this.mapGraphics.DrawImage(this.mapImage, 0, 0, this.mapImage.Width, this.mapImage.Height);

            // Draw all objects to the map buffer
            foreach (Sprite sprite in this.allObjects)
            {
                sprite.Draw(this.mapGraphics, gameTime);
            }

            // Clear the screen background (because it is larger than the map)
            renderer.Clear(Color.Black);

            // Draw the map to the screen buffer
            renderer.DrawImage(this.mapBuffer, MapXoffset, MapYoffset, this.mapBuffer.Width, this.mapBuffer.Height);

            // Draw the text messages to the screen buffer
            this.DrawMessages(gameTime, renderer);
        }

        /// <summary>
        /// Loads a map from a file
        /// </summary>
        private void LoadMapFromFile()
        {
            // Load the level map.
            string levelPath = string.Format(@"Level\Level{0}.txt", this.levelIndex);

            // Load the map and ensure all of the lines are the same length.
            int width;
            var lines = new List<string>();

            var reader = new StreamReader(levelPath);

            using (reader)
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                    {
                        throw new GameException(string.Format("The length of line {0} is different from all preceding lines.", lines.Count));
                    }

                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            this.wallMap = new TileType[lines.Count, width];
            this.allObjects = new List<Sprite>();

            for (int row = 0; row < this.wallMap.GetLength(0); ++row)
            {
                for (int col = 0; col < this.wallMap.GetLength(1); ++col)
                {
                    char tileChar = lines[row][col];
                    if (tileChar == 'P')
                    {
                        if (this.Player == null)
                        {
                            this.Player = new Hero(col * TileSize, row * TileSize, Resources.Player48x48, this.company);
                        }
                        else
                        {
                            int currentXToChange = this.MapSize.Width - this.Player.Rectangle.Left;
                            int currentYToChange = this.Player.Rectangle.Top;

                            if (currentXToChange > this.MapSize.Width / 2)
                            {
                                currentXToChange -= this.Player.Rectangle.Width + 2;
                            }
                            else
                            {
                                currentXToChange -= this.Player.Rectangle.Width - 2;
                            }

                            this.Player.SetInitialPosition(currentXToChange, currentYToChange);
                        }

                        this.allObjects.Add(this.Player);
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == 'N')
                    {
                        this.wallMap[row, col] = TileType.Next;
                    }
                    else if (tileChar == 'B')
                    {
                        this.wallMap[row, col] = TileType.Back;
                    }
                    else if (tileChar == '.')
                    {
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == 'W')
                    {
                        this.wallMap[row, col] = TileType.WallTile;
                    }
                    else if (tileChar == 'C')
                    {
                        this.allObjects.Add(new Chest(col * TileSize, row * TileSize, Resources.Chest_Closed, this.company));
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == 'G')
                    {
                        this.allObjects.Add(new Guard(col * TileSize, row * TileSize, Resources.Guard48x48, this.company));
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == 'O')
                    {
                        this.allObjects.Add(new Officer(col * TileSize, row * TileSize, Resources.Officer48x48, this.company));
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == 'D')
                    {
                        this.allObjects.Add(new Door(col * TileSize, row * TileSize, Resources.Door));
                        this.wallMap[row, col] = TileType.Door;
                    }
                }
            }
        }

        /// <summary>
        /// Builds Map Image
        /// </summary>
        private void BuildMapImage()
        {
            this.mapImage = new TileMapBuilder(this.wallMap, this.tileImages, TileSize).MapImage;

            // Set the map size and main window client size (in pixels)
            this.MapSize = new Size(this.mapImage.Width, this.mapImage.Height);
            this.ClientSize = new Size(this.MapSize.Width + (2 * MapXoffset), this.MapSize.Height + MapYoffset + TextscreenHeight);
        }

        /// <summary>
        /// Initializes collisions
        /// </summary>
        private void InitializeCollisions()
        {
            Collisions.map = this.wallMap;
            Collisions.tileSize = TileSize;
            Collisions.mapSize = this.MapSize;
            Collisions.allObjects = this.allObjects;
            Collisions.player = this.Player;
        }

        /// <summary>
        /// Draws game messages on the screen
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="renderer">Renderer</param>
        private void DrawMessages(GameTime gameTime, IGraphics renderer)
        {
            // Draw the text messages (first line) to the screen buffer
            string line0 = string.Format("LEVEL {0}", this.levelIndex + 1);
            string line1;
            renderer.DrawString(line0, this.textFont, this.textBrush, this.CenterTextX(line0), 0);

            // Draw the text messages (second line) to the screen buffer
            // If Alive
            if (this.Player.IsAlive)
            {
                this.statusTextBrush = new SolidBrush(Color.Green);
                line1 = this.Player.Status;
            }
            else
            {
                this.statusTextBrush = new SolidBrush(Color.Red);
                line1 = DeadMessage;
            }

            renderer.DrawString(line1, this.textFont, this.statusTextBrush, this.CenterTextX(line1), MapYoffset + this.MapSize.Height);

            string line2 = string.Format("♥ {0}/{1} {2} Att: {3} Amm: {4} Med: {5} Keys: {6}", this.Player.CurrentHealth, this.Player.TotalHealth, this.Player.Rank, this.Player.Weapon.Damage, this.Player.Bullets, this.Player.MedKits, this.Player.AvailableKeys);
            renderer.DrawString(line2, this.textFont, this.textBrush, this.CenterTextX(line2), MapYoffset + this.MapSize.Height + 20);
        }

        /// <summary>
        /// Returns the X coordinate of the text so that it is centered on the screen
        /// </summary>
        private int CenterTextX(string text)
        {
            if (text.Length * 9 > this.ClientSize.Width)
            {
                return 0;
            }

            return (this.ClientSize.Width - (text.Length * 9)) / 2;
        }
    }
}