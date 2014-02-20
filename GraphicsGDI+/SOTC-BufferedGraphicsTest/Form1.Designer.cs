namespace BufferedGraphicsTest
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.bufferedControl1 = new BufferedGraphicsTest.BufferedControl();
            this.SuspendLayout();
            // 
            // bufferedControl1
            // 
            this.bufferedControl1.Dirty = false;
            this.bufferedControl1.Location = new System.Drawing.Point(0, 0);
            this.bufferedControl1.Name = "bufferedControl1";
            this.bufferedControl1.Size = new System.Drawing.Size(292, 233);
            this.bufferedControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.bufferedControl1);
            this.Name = "Form1";
            this.Text = "Buffered Graphics Test";
            this.ResumeLayout(false);

    }

    #endregion

    private BufferedControl bufferedControl1;
  }
}

