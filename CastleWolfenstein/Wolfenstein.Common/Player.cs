namespace Wolfenstein.Common
{
    using System.Drawing;
    //using Wolfenstein.Common.Properties;
    using System;
    using System.Windows.Forms;

    public enum Direction { Left, Right, Up, Down, Center }

    public class Player
    {
        // Player speed in pixels per second
        private const int PlayerSpeed = 100;
        public Bitmap bmpPlayer;

        // Animation speed in frames per second
        private const float AnimationSpeed = 12.0f;
        private float currentFrame = 0;

        public PointF position;
        private Point mapPosition;
        public Direction playerDirection;

        public Player(int x, int y, Bitmap sprite)
        {
            position = new PointF(x, y);
            mapPosition = new Point((int)Math.Round(position.X), (int)Math.Round(position.Y));
            bmpPlayer = sprite;
        }

        private void CheckDirection(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.playerDirection = Direction.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.playerDirection = Direction.Right;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.playerDirection = Direction.Up;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.playerDirection = Direction.Down;
            }
            else
            {
                this.playerDirection = Direction.Center;
            }
        }

        public void Move(GameTime gameTime, KeyboardState keyboardState)
        {
            CheckDirection(keyboardState);

            PointF temp_position = position;
            Point temp_mapPos = mapPosition;

            float move = (float)(PlayerSpeed * gameTime.ElapsedTime.TotalSeconds);

            if (playerDirection == Direction.Left)
            {
                position.X -= move;
            }
            else if (playerDirection == Direction.Right)
            {
                position.X += move;
            }
            else if (playerDirection == Direction.Up)
            {
                position.Y -= move;
            }
            else if (playerDirection == Direction.Down)
            {
                position.Y += move;
            }

            mapPosition = new Point((int)Math.Round(position.X), (int)Math.Round(position.Y));

            Rectangle bounds = new Rectangle(mapPosition.X, mapPosition.Y, bmpPlayer.Width, bmpPlayer.Height);

            if (Collisions.ValidateXY(bounds) && Collisions.CheckWallCollision(bounds))
            {
                // Player was moved
                return;
            }

            // Player cannot go here, return the old coords
            position = temp_position;
            mapPosition = temp_mapPos;
        }

        public void Draw(GameTime gameTime, Graphics mapGraphics)
        {
            // Draw the player to the map buffer
            mapGraphics.DrawImage(bmpPlayer, mapPosition.X, mapPosition.Y, bmpPlayer.Width, bmpPlayer.Height);
        }

        private void AnimateSprite(GameTime gameTime, Graphics mapGraphics, Bitmap sprite, int frames, bool flip)
        {
            currentFrame += (float)(AnimationSpeed * gameTime.ElapsedTime.TotalSeconds) * (flip ? -1 : 1);

            int frameWidth = sprite.Width / frames;
            int frameIndex = (int)Math.Round(currentFrame) % frames + (frames - 1) * (flip ? 1 : 0);

            Console.WriteLine(frameIndex);

            Rectangle frame = new Rectangle(frameIndex * frameWidth, 0, frameWidth, sprite.Height);
            Rectangle location = new Rectangle(mapPosition.X, mapPosition.Y, frame.Width, frame.Height);

            mapGraphics.DrawImage(sprite, location, frame, GraphicsUnit.Pixel);
        }
    }
}