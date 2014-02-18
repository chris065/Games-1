namespace Wolfenstein.Game
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Wolfenstein.Common;
    using Wolfenstein.Game.Properties;

    public class Level
    {
        // The offset of the map from the top-left corner of the window
        private const int MapXoffset = 20;
        private const int MapYoffset = 20;

        // The height of the bottom area where text messages are shown
        private const int TextscreenHeight = 40;

        // Level graphics
        private Bitmap mapBuffer;
        private Graphics mapGraphics;
        private readonly Font textFont = new Font("Consolas", 12, FontStyle.Bold);
        private readonly SolidBrush textBrush = new SolidBrush(Color.FromArgb(255, 255, 85));
        private double fps = 0;

        // Map
        private const int TileSize = 16;
        public Size MapSize { get; private set; }
        public Size ClientSize { get; private set; }
        private Bitmap mapImage;
        private TileType[,] wallMap; // map for the walls
        private string[,] mapRegister; // map register for all objects

        // The player
        private Player player;

        public Level()
        {
            // Build the map
            this.LoadMapFromFile(0);
            this.BuildMapImage();

            // The Graphics device used to draw everything on the map (tiles, player, enemies etc.)
            this.mapBuffer = new Bitmap(MapSize.Width, MapSize.Height);
            this.mapGraphics = Graphics.FromImage(mapBuffer);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            player.Move(gameTime, keyboardState);
        }

        public void Draw(GameTime gameTime, Graphics screenGraphics)
        {
            // Draw the walls to the map buffer
            mapGraphics.DrawImage(mapImage, 0, 0, mapImage.Width, mapImage.Height);

            // Draw the player to the map buffer
            player.Draw(gameTime, mapGraphics);

            // Clear the screen background (because it is larger than the map)
            screenGraphics.Clear(Color.Black);

            // Draw the map to the screenbuffer
            screenGraphics.DrawImage(mapBuffer, MapXoffset, MapYoffset, mapBuffer.Width, mapBuffer.Height);

            // Draw the text messages to the screenbuffer
            DrawMessages(gameTime, screenGraphics);
        }

        private void LoadMapFromFile(int levelIndex)
        {
            // Load the level map.
            string levelPath = string.Format(@"...\...\Level{0}.txt", levelIndex);

            // Load the map and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(levelPath))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                    {
                        throw new Exception(string.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    }

                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid and fill the map register.
            this.wallMap = new TileType[lines.Count, width];
            this.mapRegister = new string[lines.Count, width];
            for (int row = 0; row < wallMap.GetLength(0); ++row)
            {
                for (int col = 0; col < wallMap.GetLength(1); ++col)
                {
                    char tileChar = lines[row][col];
                    if (tileChar == 'P')
                    {
                        this.player = new Player(col * TileSize, row * TileSize, Resources.Tiles28x46Player);
                        this.mapRegister[row, col] = this.player.GetType().Name;
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == '.')
                    {
                        this.wallMap[row, col] = TileType.EmptyTile;
                    }
                    else if (tileChar == 'W')
                    {
                        this.wallMap[row, col] = TileType.WallTile;
                    }
                }
            }
        }

        private void BuildMapImage()
        {
            // Load the resources
            Bitmap bmpTileBlack = Resources.Tile16x16Black;
            Bitmap bmpTileEmpty = Resources.Tile16x16Empty;
            Bitmap bmpTileWall = Resources.Tile16x16Wall;

            // Build a Bitmap image from the char[,] map using the resources
            var tileImages = new Dictionary<TileType, Bitmap>();
            tileImages.Add(TileType.EmptyTile, bmpTileBlack);
            tileImages.Add(TileType.WallTile, bmpTileWall);
            mapImage = new TileMapBuilder(wallMap, tileImages, TileSize).ToImage();

            // Set the map size and main window client size (in pixels)
            this.MapSize = new Size(mapImage.Width, mapImage.Height);
            this.ClientSize = new Size(MapSize.Width + (2 * MapXoffset), MapSize.Height + MapYoffset + TextscreenHeight);

            // Pass the map to the Collisions class
            Collisions.map = wallMap;
            Collisions.tileSize = TileSize;
            Collisions.mapSize = this.MapSize;
        }

        private void DrawMessages(GameTime gameTime, Graphics g)
        {
            // Draw the text messages (first line) to the screenbuffer
            string line0 = string.Format("LEVEL 1");
            g.DrawString(line0, textFont, textBrush, CenterTextX(line0), 0);

            // Draw the text messages (second line) to the screenbuffer
            string line1 = string.Format("Use the arrow keys to move around...");
            g.DrawString(line1, textFont, textBrush, CenterTextX(line1), MapYoffset + this.MapSize.Height);

            // Draw the text messages (third line) to the screenbuffer
            fps = ((1000 / gameTime.ElapsedTime.TotalMilliseconds) * 0.10) + (fps * 0.90);
            string line2 = string.Format("fps: {0,5:F1}", fps);
            g.DrawString(line2, textFont, textBrush, CenterTextX(line2), MapYoffset + this.MapSize.Height + 20);
        }

        /// <summary>
        /// Returns the X coord of the text so that it is centered on the screen
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