using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DoubleBufferingBufferedGraphics
{
    public partial class Form1 : Form
    {
        private Timer timer1;
        private CheckBox checkBox1;
        private float angle;

        private bool useBufferGraphics;
        private Bitmap bitmapBuffer;
        private BufferedGraphics graphicsBuffer;
        private Graphics bitmapGraphics;
        private BufferedGraphicsContext bufferContext;

        public Form1()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Set the bitmap buffers
            this.UpdateGraphicsBuffer();
            this.UpdateBitmapBuffer();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            UpdatePosition();
            Invalidate();
        }

        private void UpdatePosition()
        {
            angle++;
            if (angle > 359)
            {
                angle = 0;
            }                
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (useBufferGraphics)
            {
                Draw(graphicsBuffer.Graphics);
                //Copy the back buffer to the screen
                graphicsBuffer.Render(Graphics.FromHwnd(this.Handle));
            }
            else
            {
                Draw(bitmapGraphics);
                //Copy the back buffer to the screen
                e.Graphics.DrawImage(bitmapBuffer, 0, 0, bitmapBuffer.Width, bitmapBuffer.Height);
            }
        }

        private void Draw(Graphics g)
        {
            //DrawShapes(g);
            RotateSquare(g);
        }

        private void DrawShapes(Graphics g)
        {
            g.Clear(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Matrix mx = new Matrix();
            mx.Rotate(angle, MatrixOrder.Append);
            mx.Translate(this.ClientSize.Width / 2, this.ClientSize.Height / 2, MatrixOrder.Append);
            g.Transform = mx;
            g.FillRectangle(Brushes.Red, -100, -100, 200, 200);

            mx = new Matrix();
            mx.Rotate(-angle, MatrixOrder.Append);
            mx.Translate(this.ClientSize.Width / 2, this.ClientSize.Height / 2, MatrixOrder.Append);
            g.Transform = mx;
            g.FillRectangle(Brushes.Green, -75, -75, 149, 149);

            mx = new Matrix();
            mx.Rotate(angle * 2, MatrixOrder.Append);
            mx.Translate(this.ClientSize.Width / 2, this.ClientSize.Height / 2, MatrixOrder.Append);
            g.Transform = mx;
            g.FillRectangle(Brushes.Blue, -50, -50, 100, 100);
        }

        private void RotateSquare(Graphics g)
        {
            g.Clear(Color.Black);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Matrix matrix = new Matrix();
            matrix.Rotate(this.angle, MatrixOrder.Append);
            matrix.Translate(this.ClientSize.Width / 2,
                this.ClientSize.Height / 2, MatrixOrder.Append);
            g.Transform = matrix;
            g.FillRectangle(Brushes.Azure, -100, -100, 200, 200);
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            this.useBufferGraphics = this.checkBox1.Checked;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Don't allow the background to paint when using e.Graphics.DrawImage
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // Re-create the graphics buffers for a new window size.
            this.UpdateGraphicsBuffer();
            this.UpdateBitmapBuffer();

            // Raise the SizeChanged event (or call Refresh directly)
            base.OnSizeChanged(e);
        }

        private void UpdateBitmapBuffer()
        {
            if (bitmapBuffer != null)
            {
                bitmapBuffer.Dispose();
            }

            this.bitmapBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.bitmapGraphics = Graphics.FromImage(this.bitmapBuffer);
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.graphicsBuffer != null)
            {
                this.graphicsBuffer.Dispose();
            }

            this.bufferContext = BufferedGraphicsManager.Current;
            this.bufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            this.graphicsBuffer = this.bufferContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
        }
    }
}