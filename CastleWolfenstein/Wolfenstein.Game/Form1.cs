namespace Wolfenstein.Game
{
    using System.Drawing;
    using Wolfenstein.Common;

    public partial class Form1 : GameForm
    {
        private Game game;

        public Form1() : base()
        {
            // Required by Windows
            InitializeComponent();
        }

        /// <summary>
        /// InitializeGame will be called once per game and is the place to load
        /// all of your content and set up the game window.
        /// You must set the window Client size here!
        /// </summary>
        public override void InitializeGame()
        {
            this.game = new Game();

            // Set the form client size to the level size
            this.ClientSize = this.game.level.ClientSize;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, handling input and drawing the game.
        /// </summary>
        public override void Update(GameTime gameTime, Graphics screenGraphics, KeyboardState keyboardState)
        {
            this.game.Run(gameTime, screenGraphics, keyboardState);
        }


    }
}