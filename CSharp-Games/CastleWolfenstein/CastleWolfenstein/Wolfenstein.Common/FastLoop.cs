namespace Wolfenstein.Common
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public int msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    /// <summary>
    /// A class for the Fast loop
    /// </summary>
    public class FastLoop
    {
        /// <summary>
        /// Loop callback
        /// </summary>
        private LoopCallback callback;

        /// <summary>
        /// A stopwatch
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// Timespan between elapsed times.
        /// </summary>
        private TimeSpan previousElapsedTime;

        /// <summary>
        /// Initializes a new instance of the FastLoop class
        /// </summary>
        /// <param name="callback">A callback</param>
        public FastLoop(LoopCallback callback)
        {
            this.callback = callback;
            Application.Idle += new EventHandler(this.OnApplicationEnterIdle);
            this.stopwatch = Stopwatch.StartNew();
            this.previousElapsedTime = TimeSpan.Zero;
        }

        public delegate void LoopCallback(GameTime gameTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (this.IsAppStillIdle())
            {
                this.callback(this.GetTime());
            }
        }

        /// <summary>
        /// Gets the game time
        /// </summary>
        /// <returns>Game time</returns>
        private GameTime GetTime()
        {
            TimeSpan time = this.stopwatch.Elapsed;
            TimeSpan elapsedTime = time - this.previousElapsedTime;
            this.previousElapsedTime = time;
            return new GameTime(elapsedTime, time);
        }

        /// <summary>
        /// Checks if application is still idle
        /// </summary>
        /// <returns></returns>
        private bool IsAppStillIdle()
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }


        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
    }
}