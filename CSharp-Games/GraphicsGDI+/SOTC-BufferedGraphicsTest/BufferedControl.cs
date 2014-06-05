using System;
using System.Drawing;
using System.Windows.Forms;

namespace BufferedGraphicsTest
{
  public partial class BufferedControl : UserControl
  {
    private bool _Dirty;
    private BufferedGraphicsContext _BufferContext;
    private BufferedGraphics _Buffer;
      
    public BufferedControl()
    {
      _BufferContext = new BufferedGraphicsContext();
      SizeGraphicsBuffer();
      SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
      SetStyle(ControlStyles.DoubleBuffer, false);
    }

    private void SizeGraphicsBuffer()
    {
      if (_Buffer != null)
      {
        _Buffer.Dispose();
        _Buffer = null;
      }

      if (_BufferContext == null)
        return;

      if (DisplayRectangle.Width <= 0) 
        return;
        
      if (DisplayRectangle.Height <= 0)
        return;

      using (Graphics graphics = CreateGraphics())
        _Buffer = _BufferContext.Allocate(graphics, 
          DisplayRectangle);

      Dirty = true;
    }

    public bool Dirty
    {
      get { return _Dirty; }
      set
      {
        if (!value)
          return;

        _Dirty = true;
        Invalidate();
      }
    }

    protected override void OnPaintBackground(
      PaintEventArgs pevent)
    { /* Do Nothing */ }

    protected override void OnSizeChanged(EventArgs e)
    {
      SizeGraphicsBuffer();
      base.OnSizeChanged(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (_Buffer == null)
      {
        Draw(e.Graphics);
        return;
      }

      if (_Dirty)
      {
        _Dirty = false;
        Draw(_Buffer.Graphics);
      }

      _Buffer.Render(e.Graphics);
    }

    public virtual void Draw(Graphics graphics)
    {
      if (ClientRectangle.Width <= 0)
        return;

      if (ClientRectangle.Height <= 0)
        return;

      using(SolidBrush backBrush = new SolidBrush(BackColor))
        graphics.FillRectangle(backBrush, ClientRectangle);
      
      if(_CurrentTextRectangle.Size == Size.Empty)
      {
        SizeF sf = graphics.MeasureString(StrToDraw, 
          _TextFont);

        _CurrentTextRectangle.Width = 
          (int)Math.Ceiling(sf.Width);

        _CurrentTextRectangle.Height = 
          (int)Math.Ceiling(sf.Height);
      }

      graphics.DrawString("{ } Switch On The Code", _TextFont, 
        Brushes.Green, _CurrentTextRectangle.Location);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (_Buffer != null)
        {
          _Buffer.Dispose();
          _Buffer = null;
        }

        if (_BufferContext != null)
        {
          _BufferContext.Dispose();
          _BufferContext = null;
        }

        if (_TextFont != null)
        {
          _TextFont.Dispose();
          _TextFont = null;
        }
      }

      base.Dispose(disposing);
    }

    /* Code below here is just for the ability to
     * drag around the string.
     */

    private const string StrToDraw = "{ } Switch On The Code";

    private Rectangle _CurrentTextRectangle =
      new Rectangle(10, 10, 0, 0);
    private Font _TextFont = new Font("Tahoma", 12);
    private Point _OrigMousePoint = Point.Empty;
    private Point _OrigTextPoint = Point.Empty;
    private bool _Moving = false;

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      if (e.Button != MouseButtons.Left)
        return;

      _Moving = false;
      if (!_CurrentTextRectangle.Contains(e.Location))
        return;

      _OrigMousePoint = e.Location;
      _OrigTextPoint = _CurrentTextRectangle.Location;
      _Moving = true;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);

      if (e.Button != MouseButtons.Left)
        return;

      _Moving = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);

      if (!_Moving)
      {
        Cursor = _CurrentTextRectangle.Contains(e.Location) ? 
          Cursors.Hand : Cursors.Default;
        return;
      }

      _CurrentTextRectangle.Location = new Point(
        _OrigTextPoint.X + (e.X - _OrigMousePoint.X), 
        _OrigTextPoint.Y + (e.Y - _OrigMousePoint.Y));
      Dirty = true;
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // BufferedControl
            // 
            this.Name = "BufferedControl";
            this.Size = new System.Drawing.Size(150, 36);
            this.ResumeLayout(false);

    }
  }
}

