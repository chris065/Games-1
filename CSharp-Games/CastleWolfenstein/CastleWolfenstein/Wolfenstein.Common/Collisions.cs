namespace Wolfenstein.Common
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Models;
    using Interfaces;

    /// <summary>
    /// A class for the collisions
    /// </summary>
    public static class Collisions
    {
        public static int tileSize;
        public static TileType[,] map;
        public static Size mapSize;
        public static List<Sprite> allObjects;
        public static Sprite player;

        /// <summary>
        /// Check if the Sprite is not leaving the map and not colliding with walls or other objects.
        /// </summary>
        public static bool IsNotCollding(Sprite origin)
        {
            Rectangle bounds = origin.Rectangle;
            return IsInsideMap(bounds) && IsAwayFromWalls(bounds) && IsAwayFromObjects(origin);
        }

        private static bool IsInsideMap(Rectangle bounds)
        {
            return
                bounds.Left >= 0 &&
                bounds.Right <= mapSize.Width &&
                bounds.Top >= 0 &&
                bounds.Bottom <= mapSize.Height;
        }

        private static bool IsAwayFromWalls(Rectangle bounds)
        {
            Rectangle tileBounds = GetSpriteTileRectangle(bounds);

            // Check for collision with walls
            for (int row = tileBounds.Top; row < tileBounds.Bottom; row++)
            {
                for (int col = tileBounds.Left; col < tileBounds.Right; col++)
                {
                    // Only empty tiles are passable
                    if (map[row, col] == TileType.WallTile)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static TileType IsAtDoor(Rectangle bounds)
        {
            Rectangle tileBounds = GetSpriteTileRectangle(bounds);

            // Check for collision with doors
            for (int row = tileBounds.Top; row < tileBounds.Bottom; row++)
            {
                for (int col = tileBounds.Left; col < tileBounds.Right; col++)
                {
                    // Checks if a door is met
                    if (map[row, col] == TileType.Next)
                    {
                        return TileType.Next;
                    }
                    else if (map[row, col] == TileType.Back)
                    {
                        return TileType.Back;
                    }
                }
            }

            return TileType.EmptyTile;
        }

        private static Rectangle GetSpriteTileRectangle(Rectangle bounds)
        {
            // Find the neighboring tiles from the bounding rectangle.
            int leftTile = bounds.Left / tileSize;
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / tileSize));
            int topTile = bounds.Top / tileSize;
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / tileSize));

            Rectangle tileRect = new Rectangle(leftTile, topTile, rightTile - leftTile, bottomTile - topTile);
            return tileRect;
        }

        private static bool IsAwayFromObjects(Sprite origin)
        {
            foreach (Sprite obj in allObjects)
            {
                if (obj is ILiving)
                {
                    if (!(obj as ILiving).IsAlive)
                    {
                        continue;
                    }
                }

                if (obj.GetType().Name != origin.GetType().Name)
                {
                    Point overlap = OverLap(origin.Rectangle, obj.Rectangle);
                    if (overlap.X <= 0 && overlap.Y <= 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a list of object in an area specified by the search pattern.
        /// SearchPattern.Touching -> you are in direct contact with an object 
        /// </summary>
        public static IEnumerable<Sprite> GetObjects(Sprite origin, SearchPattern pattern)
        {
            var objectsFound = new List<Sprite>();
            foreach (var obj in allObjects)
            {
                // Exclude the searching object from the list of found objects
                if (obj.GetType().Name != origin.GetType().Name)
                {
                    Point overlap = OverLap(origin.Rectangle, obj.Rectangle);
                    if (pattern == SearchPattern.Touching)
                    {
                        if (overlap.X <= tileSize / 4 && overlap.Y <= tileSize / 4)
                        {
                            objectsFound.Add(obj);
                        }
                    }
                    else if (pattern == SearchPattern.Right)
                    {
                        if (overlap.Y <= 0 && origin.Rectangle.Right < obj.Rectangle.Left && NoWallsBetween(origin, obj))
                        {
                            objectsFound.Add(obj);
                        }
                    }
                    else if (pattern == SearchPattern.Left && NoWallsBetween(origin, obj))
                    {
                        if (overlap.Y <= 0 && origin.Rectangle.Left > obj.Rectangle.Right && NoWallsBetween(origin, obj))
                        {
                            objectsFound.Add(obj);
                        }
                    }
                    else if (pattern == SearchPattern.Up)
                    {
                        if (overlap.X <= 0 && origin.Rectangle.Top > obj.Rectangle.Bottom && NoWallsBetween(origin, obj))
                        {
                            objectsFound.Add(obj);
                        }
                    }
                    else if (pattern == SearchPattern.Down)
                    {
                        if (overlap.X <= 0 && origin.Rectangle.Bottom < obj.Rectangle.Top && NoWallsBetween(origin, obj))
                        {
                            objectsFound.Add(obj);
                        }
                    }
                }
            }

            return objectsFound;
        }

        private static bool NoWallsBetween(Sprite spriteA, Sprite spriteB)
        {
            int X1 = spriteA.Rectangle.Left;
            int X2 = spriteB.Rectangle.Left;
            int Y1 = spriteA.Rectangle.Top;
            int Y2 = spriteB.Rectangle.Top;

            if (Math.Abs(X1 - X2) <= tileSize)
            {
                int minRow = Math.Min(Y1, Y2) / tileSize;
                int maxRow = Math.Max(Y1, Y2) / tileSize;

                int minCol = Math.Min(X1, X2) / tileSize;
                int maxCol = Math.Max(X1, X2) / tileSize;

                for (int col = minCol; col <= maxCol; col++)
                {
                    for (int row = minRow; row < maxRow; row++)
                    {
                        if (map[row, col] == TileType.WallTile)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                int minX = Math.Min(X1, X2);
                int maxX = Math.Max(X1, X2);
                for (int x = minX; x < maxX; x++)
                {
                    float y = Y1 + (Y2 - Y1) * (x - X1) / (float)(X2 - X1);

                    int row = (int)y / tileSize;
                    int col = x / tileSize;

                    if (map[row, col] == TileType.WallTile)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static Point OverLap(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            int halfWidthA = rectA.Width / 2;
            int halfHeightA = rectA.Height / 2;
            int halfWidthB = rectB.Width / 2;
            int halfHeightB = rectB.Height / 2;

            // Calculate centers.
            Point centerA = new Point(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Point centerB = new Point(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            int distanceX = centerA.X - centerB.X;
            int distanceY = centerA.Y - centerB.Y;
            int minDistanceX = halfWidthA + halfWidthB;
            int minDistanceY = halfHeightA + halfHeightB;

            // Return intersection depths.
            // If the rectangles are not intersecting, the component should be positive.
            return new Point(Math.Abs(distanceX) - minDistanceX, Math.Abs(distanceY) - minDistanceY);
        }

        public static TileType GetPlayerTile(Sprite player)
        {
            return IsAtDoor(player.Rectangle);
        }

        public static bool PlayerIsVisible(Sprite observer, Direction viewDirection)
        {
            return IsInfront(observer, Collisions.player, viewDirection) && NoWallsBetween(Collisions.player, observer);
        }

        private static bool IsInfront(Sprite observer, Sprite target, Direction observerDirection)
        {
            Point overlap = OverLap(observer.Rectangle, target.Rectangle);
            switch (observerDirection)
            {
                case Direction.Up:
                    return observer.Rectangle.Top > target.Rectangle.Top;
                case Direction.Left:
                    return observer.Rectangle.Left > target.Rectangle.Left;
                case Direction.Down:
                    return observer.Rectangle.Top < target.Rectangle.Top;
                case Direction.Right:
                    return observer.Rectangle.Left < target.Rectangle.Left;
                case Direction.Still:
                    return observer.Rectangle.Top < target.Rectangle.Top;
                default:
                    throw new ApplicationException("View direction not suported!");
            }
        }

        public static Sprite GetPlayer()
        {
            return Collisions.player;
        }

        public static double GetBearing(Sprite origin, Sprite target)
        {
            int deltaX = target.Rectangle.Left - origin.Rectangle.Left;
            int deltaY = target.Rectangle.Top - origin.Rectangle.Top;

            double angleInDegrees = Math.Atan2(-deltaY, deltaX) * 180 / Math.PI;
            return angleInDegrees;
        }

        /// <summary>
        /// Returns the range (in tiles) between two sprites
        /// </summary>
        public static int GetTileRange(Sprite origin, Sprite target)
        {
            int deltaX = target.Rectangle.Left - origin.Rectangle.Left;
            int deltaY = target.Rectangle.Top - origin.Rectangle.Top;
            double range = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            return (int)(range / tileSize);
        }
    }
}