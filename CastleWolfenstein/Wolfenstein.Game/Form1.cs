namespace Wolfenstein.Game
{
    using System.Drawing;
    using Wolfenstein.Common;

    public partial class Form1 : GameForm
    {
        private Game game;

        public Form1()
            : base()
        {
            InitializeComponent();
        }

        public override int FormWidth
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override int FormHeight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override void RunGame(GameTime gameTime, Graphics screenGraphics)
        {
            this.game.Run(gameTime, screenGraphics);
        }

        public override void InitializeGame()
        {
            this.game = new Game();
        }

        public override void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            this.game.level.OnKeyDown(e.KeyCode);
        }

        public override void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            this.game.level.OnKeyUp();
        }
    }
}
