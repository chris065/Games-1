namespace Game
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

    public class FastLoop
    {
        public delegate void LoopCallback(GameTime gameTime);
        LoopCallback callback;

        private Stopwatch stopwatch;
        private TimeSpan previousElapsedTime;

        public FastLoop(LoopCallback callback)
        {
            this.callback = callback;
            Application.Idle += new EventHandler(OnApplicationEnterIdle);
            stopwatch = Stopwatch.StartNew();
            previousElapsedTime = TimeSpan.Zero;
        }

        void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle())
            {
                callback(GetTime());
            }
        }

        private GameTime GetTime()
        {
            TimeSpan time = stopwatch.Elapsed;
            TimeSpan elapsedTime = time - previousElapsedTime;
            previousElapsedTime = time;
            return new GameTime(elapsedTime, time);
        }

        private bool IsAppStillIdle()
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(
        out Message msg,
        IntPtr hWnd,
        uint messageFilterMin,
        uint messageFilterMax,
        uint flags);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public Int32 msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }
}