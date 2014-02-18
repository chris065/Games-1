namespace Wolfenstein.Common
{
    using System.Drawing;
    using System;
    using System.Windows.Forms;

    public class HumanPlayer : MovingObject
    {
        // Player speed in pixels per second
        private const int PlayerSpeed = 100;
        public Bitmap bmpPlayer;

        // Animation speed in frames per second
        private const float AnimationSpeed = 12.0f;
        private float currentFrame = 0;

        public HumanPlayer(int x, int y, Bitmap sprite)
            : base(x, y, sprite)
        {
        }

        public override void Move(GameTime gameTime, KeyboardState keyboardState)
        {
            PointF temp_position = this.Position;
            Point temp_mapPos = this.MapPosition;

            float move = (float)(PlayerSpeed * gameTime.ElapsedTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.Position = new PointF(this.Position.X - move, this.Position.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.Position = new PointF(this.Position.X + move, this.Position.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.Position = new PointF(this.Position.X, this.Position.Y - move);
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.Position = new PointF(this.Position.X, this.Position.Y + move);
            }
            else
            {
                return;
            }

            // The position on the map is represented by two integers (pixel accuracy)
            this.MapPosition = new Point((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y));

            Rectangle bounds = new Rectangle(new Point(this.MapPosition.X, this.MapPosition.Y), this.Bounds);

            if (Collisions.ValidateXY(bounds) && Collisions.CheckWallCollision(bounds))
            {
                // Player was moved
                return;
            }

            // Player cannot go here, return the old coords
            this.Position = temp_position;
            this.MapPosition = temp_mapPos;
        }

        private void AnimateSprite(GameTime gameTime, Graphics mapGraphics, Bitmap sprite, int frames, bool flip)
        {
            currentFrame += (float)(AnimationSpeed * gameTime.ElapsedTime.TotalSeconds) * (flip ? -1 : 1);

            int frameWidth = sprite.Width / frames;
            int frameIndex = (int)Math.Round(currentFrame) % frames + (frames - 1) * (flip ? 1 : 0);

            Console.WriteLine(frameIndex);

            Rectangle frame = new Rectangle(frameIndex * frameWidth, 0, frameWidth, sprite.Height);
            Rectangle location = new Rectangle(this.MapPosition.X, this.MapPosition.Y, frame.Width, frame.Height);

            mapGraphics.DrawImage(sprite, location, frame, GraphicsUnit.Pixel);
        }
    }
}