using System;
using System.Collections.Generic;
using System.IO;
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
        public GameItem[,] Map { get; set; }
        private GameItem ConvertToGameItem(char c)
        {
            switch (c)
            {
                case '#':
                    return GameItem.wall;
                case 'P':
                    return GameItem.player;
                case ' ':
                    return GameItem.floor;
                case 'B':
                    return GameItem.box;
                case 'T':
                    return GameItem.target;
                default:
                    return GameItem.floor;
            }
        }
    }
}
