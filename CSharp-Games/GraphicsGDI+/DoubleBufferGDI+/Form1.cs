namespace DoubleBufferGDI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private Graphics graphicsOnScreen; // On-screen graphics surface associated with the form
        private Graphics graphicsOffScreen; // Off-screen graphics surface associated with the bitmap stored in memory
        private Bitmap buffer; // Memory stoarage for the off-screen buffer
        private Random rand;

        public Form1()
        {
            // Required method for Designer support - do not modify
            // the contents of this method with the code editor.
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.graphicsOnScreen = this.CreateGraphics();
            this.graphicsOnScreen.Clear(Color.Black);
            this.buffer = new Bitmap(this.Width, this.Height);
            this.graphicsOffScreen = Graphics.FromImage(this.buffer);
            this.rand = new Random();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.graphicsOffScreen.Clear(Color.Navy);
            this.graphicsOnScreen.DrawImage(this.buffer, 0, 0);
        }

        // Called by our timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.graphicsOffScreen.DrawEllipse(Pens.AntiqueWhite, this.rand.Next(0, this.Width), this.rand.Next(0, this.Height), 2, 2);
            this.graphicsOnScreen.DrawImage(this.buffer, 0, 0);
        }

        // Called when a portion of the form is invalidated or refreshed,
        // e.g. by resizing or hiding behind another window.
        // If not implemented the form will stay empty until the timer event is raised to redraw.
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.graphicsOnScreen.DrawImage(this.buffer, 0, 0);
        }
    }
}