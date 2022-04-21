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
        public enum Direction
        {
            up, down, left, right
        }
        public GameItem[,] Map { get; set; }
        public bool[,] TargetCheckMap { get; set; }
        private Queue<string> levels;
        public GameLogic()
        {
            levels = new Queue<string>();
            foreach(var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(),"Levels")))
            {
                levels.Enqueue(item);
            }
            LoadMap(levels.Dequeue());

        }
        private void LoadMap(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Map = new GameItem[int.Parse(lines[0]), int.Parse(lines[1])];
            TargetCheckMap = new bool[int.Parse(lines[0]), int.Parse(lines[1])];
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (ConvertToGameItem(lines[i + 2][j]) == GameItem.target)
                    {
                        TargetCheckMap[i, j] = true;
                    }
                    else
                    {
                        TargetCheckMap[i, j] = false;
                    }
                    Map[i, j] = ConvertToGameItem(lines[i + 2][j]);
                }
            }
        }
        public void Move(Direction direction)
        {
            var position = CurrentPosition();
            int x = position[0];
            int y = position[1];
            int old_x = x;
            int old_y = y;
            switch (direction)
            {
                case Direction.up:
                    if(y-1>=0)
                    {
                        y--;
                    }
                    break;
                case Direction.down:
                    if(y+1<Map.GetLength(0))
                    {
                        y++;
                    }
                    break;
                case Direction.left:
                    if(x-1>=0)
                    {
                        x--;
                    }
                    break;
                case Direction.right:
                    if(x+1<Map.GetLength(1))
                    {
                        x++;
                    }
                    break;
                default:
                    break;
            }
            if (TargetCheckMap[old_x, old_y] == true) // ha target mezőről lépünk el
            {
                if (Map[x, y] == GameItem.box && direction.Equals(Direction.up) && (Map[x, y - 1] == GameItem.floor || Map[x, y - 1] == GameItem.target)) // dobozt tolunk fel és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x, y - 1] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.box && direction.Equals(Direction.down) && (Map[x, y + 1] == GameItem.floor || Map[x, y + 1] == GameItem.target)) // dobozt tolunk le és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x, y + 1] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.box && direction.Equals(Direction.left) && (Map[x - 1, y] == GameItem.floor || Map[x - 1, y] == GameItem.target)) // dobozt tolunk balra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x - 1, y] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.box && direction.Equals(Direction.right) && (Map[x + 1, y] == GameItem.floor || Map[x + 1, y] == GameItem.target)) // dobozt tolunk jobbra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x + 1, y] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.floor || Map[x, y] == GameItem.target) // üres mezőre lépünk
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                }
            }
            else // ha nem target mezőről lépünk el
            {
                if (Map[x, y] == GameItem.box && direction.Equals(Direction.up) && (Map[x, y - 1] == GameItem.floor || Map[x, y - 1] == GameItem.target)) // dobozt tolunk fel és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player;
                    Map[x, y - 1] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.box && direction.Equals(Direction.down) && (Map[x, y + 1] == GameItem.floor || Map[x, y + 1] == GameItem.target)) // dobozt tolunk le és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player;
                    Map[x, y + 1] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.box && direction.Equals(Direction.left) && (Map[x - 1, y] == GameItem.floor || Map[x - 1, y] == GameItem.target)) // dobozt tolunk balra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player;
                    Map[x - 1, y] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.box && direction.Equals(Direction.right) && (Map[x + 1, y] == GameItem.floor || Map[x + 1, y] == GameItem.target)) // dobozt tolunk jobbra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player;
                    Map[x + 1, y] = GameItem.box;
                }
                else if (Map[x, y] == GameItem.floor || Map[x,y]==GameItem.target) // üres mezőre lépünk
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player;
                }
            }
            if(MapDone())
            {
                LoadMap(levels.Dequeue());
            }
        }
        private int[] CurrentPosition()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if(Map[i,j] == GameItem.player)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return new int[] { -1, -1 };
        }
        private bool MapDone()
        {
            bool done = true;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1) && done; j++)
                {
                    if(Map[i,j]!=GameItem.box && TargetCheckMap[i,j]) // target alapból, de nincs rajta doboz
                    {
                        done = false;
                    }
                }
            }
            return done;
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
