using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace DrawString
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CenterToScreen();
            this.Text = "Basic Paint Form (click on me)";
        }

        private ArrayList myPts = new ArrayList();

        public void MyPaintHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
        }

        private void MainForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Grab a new Graphics object.
            Graphics g = Graphics.FromHwnd(this.Handle);

            // Now draw a 10*10 circle at mouse click.
            // g.DrawEllipse(new Pen(Color.Green), e.X, e.Y, 10, 10);
            // Add to points collection.
            myPts.Add(new Point(e.X, e.Y));
            Invalidate(); // means close window
        }

        private void MainForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString("Hello GDI+", new Font("Times New Roman", 20),
            new SolidBrush(Color.Black), 0, 0);
            foreach (Point p in myPts)
                g.DrawEllipse(new Pen(Color.Green), p.X, p.Y, 10, 10);
        }
    }
}
