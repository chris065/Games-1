

namespace Wolfenstein.Common
{
    using Interfaces;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class GameForm : Form
    {
        private readonly Bitmap screenBuffer;
        private readonly GDIGraphics renderer;
        private readonly FastLoop fastLoop;
        private float fps = 0;
        private readonly KeyboardState keyboardState;

        public static bool DEBUG_MODE { get; private set; }

        public GameForm()
        {
            // Prepare the form
            this.InitializeForm();

            // Initialize the game - this happens only once
            this.InitializeGame();

            // Set the graphics device
            this.screenBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            this.renderer = new GDIGraphics(screenBuffer);

            this.keyboardState = new KeyboardState();

            // Runs the game loop whenever the application is idle
            this.fastLoop = new FastLoop(GameLoop);

            DEBUG_MODE = true;
        }

        public abstract void Update(GameTime gameTime, IGraphics renderer, IControllerState keyboardState);

        public abstract void InitializeGame();

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            this.keyboardState.OnKeyDown(e.KeyCode);
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            this.keyboardState.OnKeyUp(e.KeyCode);
        }

        private void GameLoop(GameTime gameTime)
        {
            Update(gameTime, this.renderer, this.keyboardState);

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