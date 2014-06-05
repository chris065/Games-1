namespace Wolfenstein.Common
{
    /// <summary>
    /// Enumeration for the direction of the characters
    /// </summary>
    public enum Direction
    {
        Up = 0,
        Left = 1,
        Down = 2,
        Right = 3,
        Still = 5
    }

    /// <summary>
    /// Enumeration for the rarity of the items
    /// </summary>
    public enum ItemRarity
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
    }


    /// <summary>
    /// Tiles
    /// </summary>
    public enum TileType
    {
        EmptyTile,
        WallTile,
        Next,
        Back,
        Door
    }

    /// <summary>
    /// Status: if level should be changed
    /// </summary>
    public enum LevelChangeStatus
    {
        Change,
        NoChange
    }

    /// <summary>
    /// Search pattern
    /// </summary>
    public enum SearchPattern
    {
        Touching,
        Nearby,
        Left,
        Right,
        Up,
        Down
    }

    /// <summary>
    /// Ranks of the hero
    /// </summary>
    public enum Ranks
    {
        Private, Corporal, Sergeant, Lieutenant, Captain, Colonel, General
    }
}
