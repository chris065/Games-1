namespace MessageBox
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Form1()
        {
            this.Top = 100;
            this.Left = 75;
            this.Height = 100;
            this.Width = 500;

            InitializeComponent();
        }

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
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString(), "Key Pressed!");
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            // Which mouse button was clicked?
            if (e.Button == MouseButtons.Left)
            {
                MessageBox.Show("Left click!");
            }                
            else if (e.Button == MouseButtons.Right)
            {
                MessageBox.Show("Right click!");
            }               
            else if (e.Button == MouseButtons.Middle)
            {
                MessageBox.Show("Middle click!");
            }                
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = "Current Pos: (" + e.X + ", " + e.Y + ")";
        }
    }
}