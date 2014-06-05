namespace DrawWithPen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    // From "A Beginner’s Primer on Drawing Graphics using the .NET Framework"
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Acquire an instance of a drawing surface by calling the CreateGraphics method of the form.
            // The 'using' statement ensures that we dispose of the Graphics object when finished
            using (Graphics g = CreateGraphics())
            {
                this.DrawLine(g);
                this.DrawPie(g);
                this.DrawBezier(g);
            }

            ////// Alternatively draw on the Graphics object provided by the form
            ////this.DrawLine(e.Graphics);
            ////this.DrawPie(e.Graphics);
            ////this.DrawBezier(e.Graphics);
        }

        private void DrawLine(Graphics g)
        {
            // Draw a red line
            Pen p = new Pen(Color.Red, 7);
            g.DrawLine(p, 1, 1, 100, 100);
        }

        private void DrawPie(Graphics g)
        {
            // Draw a blue pie
            Pen p = new Pen(Color.Blue, 3);
            g.DrawPie(p, 1, 1, 100, 100, -30, 60);
        }

        private void DrawBezier(Graphics g)
        {
            // Draw a black Bezier curve.
            Pen pen = new Pen(Color.Black);
            pen.Width = 5;
            pen.DashStyle = DashStyle.Dash;

            // Specify which type of drawing must occur at the ends of the stroke
            pen.StartCap = LineCap.RoundAnchor;
            pen.EndCap = LineCap.ArrowAnchor;

            g.DrawBezier(
                pen,
                new Point(10, 30),
                new Point(30, 200),
                new Point(50, -100),
                new Point(70, 100));
        }
    }
}