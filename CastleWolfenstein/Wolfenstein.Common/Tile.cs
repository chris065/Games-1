namespace Wolfenstein.Common
{
    // Used to draw the tiles
    public enum TileType
    {
        EmptyTile,
        WallTile
    }

    /// <summary>
    /// Stores the texture and passability of a tile.
    /// </summary>
    public struct Tile
    {
        /// <summary>
        /// The type of the object that occupies a tile.
        /// </summary>
        public string Type;

        /// <summary>
        /// False if the tile does not allow the player to move through,
        /// True - if the player can pass.
        /// </summary>
        public bool IsPassable;

        public Tile(string type, bool isPassable)
        {
            this.Type = type;
            this.IsPassable = isPassable;
        }
    }
}