using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ThreeImages.Properties;

public partial class ThreeImagesForm : Form
{
    // The images.
    private Image bMapImageA;
    private Image bMapImageB;
    private Image bMapImageC;

    // Rects for the images.
    private Rectangle rectA;
    private Rectangle rectB;
    private Rectangle rectC;

    // A polygon region.
    GraphicsPath myPath = new GraphicsPath();

    // Did they click on an image?
    private bool isImageClicked = false;
    private int imageClicked;

    public ThreeImagesForm()
    {
        InitializeComponent();

        // Fill the images with bitmaps.
        bMapImageA = Resources.ninja_01;
        bMapImageB = Resources.ninja_02;
        bMapImageC = Resources.ninja_03;

        // Set the rects for the images.
        int xOffset = 10;
        int yOffset = 10;
        int yA = yOffset;
        int yB = yA + bMapImageA.Width + yOffset;
        int yC = yB + bMapImageB.Width + yOffset;
        rectA = new Rectangle(xOffset, yA, bMapImageA.Width, bMapImageA.Height);
        rectB = new Rectangle(xOffset, yB, bMapImageB.Width, bMapImageB.Height);
        rectC = new Rectangle(xOffset, yC, bMapImageC.Width, bMapImageC.Height);

        // Create an interesting region.
        myPath.StartFigure();
        myPath.AddLine(new Point(150, 10), new Point(120, 150));
        myPath.AddArc(200, 200, 100, 100, 0, 90);
        Point point1 = new Point(250, 250);
        Point point2 = new Point(350, 275);
        Point point3 = new Point(350, 325);
        Point point4 = new Point(250, 350);
        Point[] points = { point1, point2, point3, point4 };
        myPath.AddCurve(points);
        myPath.CloseFigure();
        CenterToScreen();
    }

    private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        // Get (x, y) of mouse click.
        Point mousePt = new Point(e.X, e.Y);

        // See if the mouse is anywhere in the 3 regions...
        if (rectA.Contains(mousePt))
        {
            isImageClicked = true;
            imageClicked = 0;
            this.Text = "You clicked image A";
        }
        else if (rectB.Contains(mousePt))
        {
            isImageClicked = true;
            imageClicked = 1;
            this.Text = "You clicked image B";
        }
        else if (rectC.Contains(mousePt))
        {
            isImageClicked = true;
            imageClicked = 2;
            this.Text = "You clicked image C";
        }
        else if (myPath.IsVisible(mousePt))
        {
            isImageClicked = true;
            imageClicked = 3;
            this.Text = "You clicked the strange shape...";
        }
        else    // Not in any shape, set defaults.
        {
            isImageClicked = false;
            this.Text = "Images";
        }

        // Redraw the client area.
        Invalidate();
    }

    private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Render all three images.
        // Use the DrawImage method to draw the image using the original pixel size
        g.DrawImage(bMapImageA, rectA);
        g.DrawImage(bMapImageB, rectB);
        g.DrawImage(bMapImageC, rectC);

        // Draw the graphics path.
        g.FillPath(Brushes.AliceBlue, myPath);

        // Draw outline (if clicked...)
        if (isImageClicked == true)
        {
            Pen outline = new Pen(Color.Red, 5);

            switch (imageClicked)
            {
                case 0:
                    g.DrawRectangle(outline, rectA);
                    break;

                case 1:
                    g.DrawRectangle(outline, rectB);
                    break;

                case 2:
                    g.DrawRectangle(outline, rectC);
                    break;

                case 3:
                    g.DrawPath(outline, myPath);
                    break;

                default:
                    break;
            }
        }
    }
}