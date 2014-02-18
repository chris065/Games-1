using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Wolfenstein.Game.Properties;
using Wolfenstein.Common;

namespace Wolfenstein.Game
{
    public class Level
    {
        // Level graphics
        private Bitmap mapBuffer;
        private Graphics mapGraphics;
        private readonly Font textFont = new Font("Consolas", 12, FontStyle.Bold);
        private readonly SolidBrush textBrush = new SolidBrush(Color.FromArgb(255, 255, 85));
        private double fps = 0;

        // Map
        private const int TILE_SIZE = 16;
        private const int MAP_WIDTH = 33;
        private const int MAP_HEIGHT = 19;
        private char[,] map;
        public Rectangle MapRectangle { get; private set; }
        public Size ClientSize { get; private set; }
        public int MapXoffset { get; private set; }
        public int MapYoffset { get; private set; }
        public int TextscreenHeight { get; private set; }

        // Resources
        private Bitmap bmpTileEmpty;
        private Bitmap bmpTileBlack;
        private Bitmap bmpTileWall;
        private Bitmap mapWalls;

        // The player
        private Player player;

        public Level()
        {
            // The offset of the map from the top-left corner of the window
            MapXoffset = 20;
            MapYoffset = 20;

            // The height of the bottom area where text messages are shown
            TextscreenHeight = 40;

            MapRectangle = new Rectangle(0, 0, MAP_WIDTH * TILE_SIZE, MAP_HEIGHT * TILE_SIZE);
            ClientSize = new Size(MapRectangle.Width + 2 * MapXoffset, MapRectangle.Height + MapYoffset + TextscreenHeight);

            // The Graphics device used to draw everything on the map (tiles, player, enemies etc.)
            mapBuffer = new Bitmap(MapRectangle.Width, MapRectangle.Height);
            mapGraphics = Graphics.FromImage(mapBuffer);

            map = LoadMapFromFile();
            LoadResources();

            Dictionary<char, Bitmap> tileImages = new Dictionary<char, Bitmap>();
            tileImages.Add('.', bmpTileBlack);
            tileImages.Add('W', bmpTileWall);
            mapWalls = new TileMapBuilder(map, tileImages, TILE_SIZE).ToImage();

            Collisions.map = map;
            Collisions.tileSize = TILE_SIZE;
            Collisions.mapRectangle = this.MapRectangle;

            player = new Player(TILE_SIZE, TILE_SIZE, Resources.Tiles28x46Player); // Put on empty tile!
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            player.Move(gameTime, keyboardState);
        }

        public void Draw(GameTime gameTime, Graphics screenGraphics)
        {
            // Draw the walls to the map buffer
            mapGraphics.DrawImage(mapWalls, 0, 0, mapWalls.Width, mapWalls.Height);

            // Draw the player to the map buffer
            player.Draw(gameTime, mapGraphics);

            // Clear the screen background (because it is larger than the map)
            screenGraphics.Clear(Color.Black);

            // Draw the map to the screenbuffer
            screenGraphics.DrawImage(mapBuffer, MapXoffset, MapYoffset, mapBuffer.Width, mapBuffer.Height);

            // Draw the text messages to the screenbuffer
            DrawMessages(gameTime, screenGraphics);
        }

        private char[,] LoadMapFromFile()
        {
            // Load the level map.
            int levelIndex = 0;
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
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            char[,] map = new char[lines.Count, width];
            for (int row = 0; row < map.GetLength(0); ++row)
            {
                for (int col = 0; col < map.GetLength(1); ++col)
                {
                    char tileType = lines[row][col];
                    map[row, col] = tileType;
                }
            }

            // Verify that the level has a beginning and an end.
            return map;
        }

        private void LoadResources()
        {
            bmpTileEmpty = Resources.Tile16x16Empty;
            bmpTileWall = Resources.Tile16x16Wall;
            bmpTileBlack = Resources.Tile16x16Black;
        }

        private void DrawMessages(GameTime gameTime, Graphics g)
        {
            // Draw the text messages (first line) to the screenbuffer
            string line0 = string.Format("LEVEL 1");
            g.DrawString(line0, textFont, textBrush, CenterTextX(line0), 0);

            // Draw the text messages (second line) to the screenbuffer
            string line1 = string.Format("Use the arrow keys to move around...");
            g.DrawString(line1, textFont, textBrush, CenterTextX(line1), MapYoffset + this.MapRectangle.Bottom);

            // Draw the text messages (third line) to the screenbuffer
            fps = (1000 / gameTime.ElapsedTime.TotalMilliseconds) * 0.10 + fps * 0.90;
            string line2 = string.Format("fps: {0,5:F1}", fps);
            g.DrawString(line2, textFont, textBrush, CenterTextX(line2), MapYoffset + this.MapRectangle.Bottom + 20);
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
            return (this.ClientSize.Width - text.Length * 9) / 2;
        }
    }
}