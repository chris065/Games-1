namespace FunWithGraphics
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using FunWithGraphics.Properties;

    public partial class Form1 : Form
    {
        private readonly Bitmap bmpBackground = Resources.FunWithImages;

        public Form1()
        {
            this.InitializeComponent();

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bm = new Bitmap(this.bmpBackground);
            e.Graphics.DrawImage(bm, 1, 1, this.Width, this.Height);
        }
    }
}