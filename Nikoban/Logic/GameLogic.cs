using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoban.Logic
{
    public class GameLogic : IGameModel, IGameControl
    {
        public enum GameItem
        {
            player, wall, floor, box, target // lehetséges elemek a pályán
        }
    }
}
