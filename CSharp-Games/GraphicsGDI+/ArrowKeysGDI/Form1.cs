namespace ArrowKeysGDI
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
        private const int Step = 10;
        private const int RectWidth = 15;
        private const int RectHeight = 15;
        private readonly Brush fillBrush = Brushes.Crimson;

        private Graphics visual;
        private int rectX, rectY;

        public Form1()
        {
            // Required method for Designer support - do not modify
            // the contents of this method with the code editor.
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initial position of the rectangle
            this.rectX = 0;
            this.rectY = 0;

            // Creates the graphics for the control
            this.visual = this.CreateGraphics();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Delete the rectangle (paint with the background color)
            this.DrawRect(new SolidBrush(this.BackColor));

            // Move the rectangle
            this.MoveRect(e.KeyCode);

            // Draw with the fill color (make visible) on the new position
            this.DrawRect(this.fillBrush);
        }

        private void DrawRect(Brush brush)
        {
            this.visual.FillRectangle(brush, new Rectangle(this.rectX, this.rectY, RectWidth, RectHeight));
        }

        private void MoveRect(Keys key)
        {
            switch (key)
            {
                case Keys.Down:
                    this.rectY += Step;
                    break;
                case Keys.Up:
                    this.rectY -= Step;
                    break;
                case Keys.Left:
                    this.rectX -= Step;
                    break;
                case Keys.Right:
                    this.rectX += Step;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.DrawRect(this.fillBrush);
        }
    }
}
