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
        private void LoadMap(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Map = new GameItem[int.Parse(lines[0]), int.Parse(lines[1])];
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j] = ConvertToGameItem(lines[i + 2][j])
                }
            }
        }
        private GameItem ConvertToGameItem(char c)
        {
            switch (c)
            {
                case '#':
                    return GameItem.wall;
                case 'P':
                    return GameItem.player;
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
