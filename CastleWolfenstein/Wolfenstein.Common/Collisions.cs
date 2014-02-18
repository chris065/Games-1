namespace Wolfenstein.Common
{
    using System;
    using System.Drawing; 

    public static class Collisions
    {
        public static int tileSize;
        public static char[,] map;
        public static Size mapSize;

        public static bool ValidateXY(Rectangle bounds)
        {
            return
                bounds.Left >= 0 &&
                bounds.Right <= mapSize.Width &&
                bounds.Top >= 0 &&
                bounds.Bottom <= mapSize.Height;
        }

        public static bool CheckWallCollision(Rectangle bounds)
        {
            // Find the neighboring tiles from the bounding rectangle.
            int leftTile = bounds.Left / tileSize;
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / tileSize));
            int topTile = bounds.Top / tileSize;
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / tileSize));

            for (int row = topTile; row < bottomTile; row++)
            {
                for (int col = leftTile; col < rightTile; col++)
                {
                    if (map[row, col] == 'W')
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}