namespace BufferingExampleMSDN
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private BufferedGraphicsContext bufferContext;
        private BufferedGraphics graphicsBuffer;
        private System.Windows.Forms.Timer timer1;
        private readonly Random rand;

        public Form1()
        {
            this.InitializeComponent();

            Application.ApplicationExit += new EventHandler(this.ReleaseBuffer);

            // The control is first drawn to a buffer rather than directly to the screen, which can reduce flicker.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            rand = new Random();

            // Configure a timer to draw graphics updates.
            this.timer1 = new System.Windows.Forms.Timer();
            this.timer1.Interval = 40;
            this.timer1.Tick += new EventHandler(this.OnTimer);
            this.timer1.Start();

            this.bufferContext = BufferedGraphicsManager.Current;
            this.UpdateGraphicsBuffer();

            // Draw the first frame to the buffer.
            this.DrawToBuffer(this.graphicsBuffer.Graphics);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.graphicsBuffer.Render(e.Graphics);
        }

        private void ReleaseBuffer(object sender, EventArgs e)
        {
            // clean up the memory
            if (this.graphicsBuffer != null)
            {
                this.graphicsBuffer.Dispose();
            }                
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.graphicsBuffer != null)
            {
                this.graphicsBuffer.Dispose();
                this.graphicsBuffer = null;
            }

            this.bufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            this.graphicsBuffer = this.bufferContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
        }

        private void OnTimer(object sender, EventArgs e)
        {
            // Draw randomly positioned ellipses to the buffer.
            this.DrawToBuffer(this.graphicsBuffer.Graphics);

            // Render the graphics buffer to the form's HDC (or call Invalidate or Refresh)
            this.graphicsBuffer.Render(Graphics.FromHwnd(this.Handle));
        }

        private void DrawToBuffer(Graphics g)
        {
            // Clear the graphics buffer
            this.graphicsBuffer.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);

            // Draw randomly positioned and colored ellipses.
            int px = rand.Next(0, this.Width - 40);
            int py = rand.Next(0, this.Height - 40);
            Pen pen = new Pen(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)), 5);
            g.DrawEllipse(pen, px, py, rand.Next(20, this.Width - px), rand.Next(20, this.Height - py));

            // Draw information strings.
            g.DrawString("BufferingExample", new Font("Arial", 8), Brushes.White, 10, 10);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Re-create the graphics buffer for a new window size.
            this.UpdateGraphicsBuffer();

            // Cause the background to be cleared and redraw.
            this.DrawToBuffer(this.graphicsBuffer.Graphics);
            this.Refresh();
        }
    }
}