namespace Wolfenstein.Common
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Builds a Bitmap image from a char[,] map and
    /// a Dictionary that contains the bitmap to use for each char.
    /// </summary>
    public class TileMapBuilder
    {
        /// <summary>
        /// Initializes a new instance of the TileMapBuilder class
        /// </summary>
        /// <param name="map">matrix of symbols</param>
        /// <param name="dictionary">dictionary with keys for the symbols</param>
        /// <param name="tileSize">Tile size</param>
        public TileMapBuilder(TileType[,] map, Dictionary<TileType, Bitmap> dictionary, int tileSize)
        {
            int mapRows = map.GetLength(0);
            int mapCols = map.GetLength(1);

            this.MapImage = new Bitmap(mapCols * tileSize, mapRows * tileSize);
            Graphics mapGraphics = Graphics.FromImage(this.MapImage);

            Bitmap tile;
            for (int row = 0; row < mapRows; row++)
            {
                for (int col = 0; col < mapCols; col++)
                {
                    tile = dictionary[map[row, col]];
                    int x = col * tileSize;
                    int y = row * tileSize;
                    mapGraphics.DrawImage(tile, x, y, tileSize, tileSize);
                }
            }
        }

        /// <summary>
        /// Gets the MapImage
        /// </summary>
        public Bitmap MapImage { get; private set; }
    }
}