using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfenstein.Common;

namespace Wolfenstein.Game
{
    class Hero : Player
    {
        public Hero(int x, int y)
            : base(x,y)
        {

        }

        protected override void LoadResources()
        {
            throw new NotImplementedException();
        }
    }
}
