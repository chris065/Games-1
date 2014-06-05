namespace Wolfenstein.Game
{
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;
    using Properties;

    /// <summary>
    /// Class for the Intro.
    /// </summary>
    public partial class Intro : Form
    {
        /// <summary>
        /// Screen Width of the window
        /// </summary>
        private const int ScreenWidth = 800;

        /// <summary>
        /// Screen height of the window
        /// </summary>
        private const int ScreenHeight = 600;

        /// <summary>
        /// Intro sound
        /// </summary>
        private readonly SoundPlayer introSound = new SoundPlayer(Resources.Intro);

        /// <summary>
        /// Start sound
        /// </summary>
        private readonly SoundPlayer startSound = new SoundPlayer(Resources.dun_dun_dun);

        /// <summary>
        /// Initializes a new instance of the Intro class
        /// </summary>
        public Intro()
        {
            InitializeComponent();
            this.MinimumSize = new Size(ScreenWidth, ScreenHeight);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = Resources.Team_Marcel_Proust;
            this.introSound.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Intro_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Return:
                    this.BackgroundImage = Resources.Game_Controls;
                    break;

                case Keys.Right:
                    this.BackgroundImage = Resources.CastleWolfensteinStory;
                    break;

                case Keys.Escape:
                    this.Close();
                    this.startSound.Play();
                    break;
            }
        }
    }
}
