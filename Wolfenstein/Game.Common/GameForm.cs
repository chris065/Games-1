namespace Game
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class GameForm : Form
    {
        private readonly Bitmap screenBuffer;
        private readonly Graphics screenGraphics;
        private readonly FastLoop fastLoop;
        private float fps = 0;

        private Level level;

        public static bool DEBUG_MODE { get; private set; }

        public GameForm()
        {
            // Prepare the form
            InitializeForm();

            // Create level
            this.level = new Level();

            // Set the form client size to the level size
            this.ClientSize = level.ClientSize;

            // Set the graphics device
            screenBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            screenGraphics = Graphics.FromImage(screenBuffer);

            // Runs the game loop whenever the application is idle
            fastLoop = new FastLoop(GameLoop);

            DEBUG_MODE = true;
        }

        void GameLoop(GameTime gameTime)
        {
            // This is the main game loop
            level.Update(gameTime);
            level.Draw(gameTime, screenGraphics);

            // Redraw the Form window
            Invalidate();

            if (GameForm.DEBUG_MODE)
            {
                fps = (1000 / (float)gameTime.ElapsedTime.TotalMilliseconds) * 0.1f + fps * 0.9f;
                System.Console.WriteLine("fps: {0,5:F1}", fps);
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            // Render the screenbuffer to the screen
            e.Graphics.DrawImage(screenBuffer, 0, 0, screenBuffer.Width, screenBuffer.Height);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Don't allow the background to paint when using e.Graphics.DrawImage
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            level.OnKeyDown(e.KeyCode);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            level.OnKeyUp();
        }

        private void InitializeForm()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.Black;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
        }
    }
}