namespace Wolfenstein.Game
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Entry class for the application
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Start intro
            Application.Run(new Intro());  
 
            // Start game
            Application.Run(new GameWindow());
        }
    }
}
