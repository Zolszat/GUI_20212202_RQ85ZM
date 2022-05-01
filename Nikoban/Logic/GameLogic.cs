using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nikoban.Logic
{
    public class GameLogic : IGameModel, IGameControl
    {
        public enum GameItem
        {
            player, wall, floor, box, target, box_on_target, player_on_target // lehetséges elemek a pályán
        }
        public enum Direction
        {
            up, down, left, right
        }
        public GameItem[,] Map { get; set; }
        public bool[,] TargetCheckMap { get; set; }
        public bool shouldICloseTheWindow = false;
        private List<string> levels;
        private int life;
        int levelIndex = 0;
        public GameLogic()
        {
            score = 0;
            life = 5;
            levels = new List<string>();
            foreach(var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(),"Levels")))
            {
                levels.Add(item);
            }
            if(levelIndex <= levels.Count)
            {
                LoadMap(levels[levelIndex]);
            }
            else
            {
                Environment.Exit(5);
            }
        }
        private void LoadMap(string path)
        {
            score += 100;
            string[] lines = File.ReadAllLines(path);
            Map = new GameItem[int.Parse(lines[0]), int.Parse(lines[1])];
            TargetCheckMap = new bool[int.Parse(lines[0]), int.Parse(lines[1])];
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (ConvertToGameItem(lines[i + 2][j]) == GameItem.target || ConvertToGameItem(lines[i + 2][j]) == GameItem.box_on_target || ConvertToGameItem(lines[i + 2][j]) == GameItem.player_on_target)
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
            bool box_stuck = false;
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
                if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.up) && (Map[x, y - 1] == GameItem.floor || Map[x, y - 1] == GameItem.target)) // dobozt tolunk fel és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x, y - 1] = GameItem.box;
                    score--;
                    if(BoxStuck(x,y-1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.down) && (Map[x, y + 1] == GameItem.floor || Map[x, y + 1] == GameItem.target)) // dobozt tolunk le és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x, y + 1] = GameItem.box;
                    score--;
                    if (BoxStuck(x, y + 1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.left) && (Map[x - 1, y] == GameItem.floor || Map[x - 1, y] == GameItem.target)) // dobozt tolunk balra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x - 1, y] = GameItem.box;
                    score--;
                    if (BoxStuck(x-1, y))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.right) && (Map[x + 1, y] == GameItem.floor || Map[x + 1, y] == GameItem.target)) // dobozt tolunk jobbra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x + 1, y] = GameItem.box;
                    score--;
                    if (BoxStuck(x+1, y))
                    {
                        box_stuck = true;
                    }
                }
                else if (Map[x, y] == GameItem.floor || Map[x, y] == GameItem.target) // üres mezőre lépünk
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                }
                else if (Map[x, y] == GameItem.player_on_target || Map[x, y] == GameItem.floor) // üres mezőre lépünk
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                }
            }
            else // ha nem target mezőről lépünk el
            {
                if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.up) && (Map[x, y - 1] == GameItem.floor || Map[x, y - 1] == GameItem.target)) // dobozt tolunk fel és nincs mögötte fal
                {
                    if (Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x, y - 1] = GameItem.box;
                        score--;
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x, y - 1] = GameItem.box;
                        score--;
                    }
                    if (BoxStuck(x, y - 1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.down) && (Map[x, y + 1] == GameItem.floor || Map[x, y + 1] == GameItem.target)) // dobozt tolunk le és nincs mögötte fal
                {
                    if (Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x, y + 1] = GameItem.box;
                        score--;
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x, y + 1] = GameItem.box;
                        score--;
                    }
                    if (BoxStuck(x, y + 1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.left) && (Map[x - 1, y] == GameItem.floor || Map[x - 1, y] == GameItem.target)) // dobozt tolunk balra és nincs mögötte fal
                {
                    if (Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x - 1, y] = GameItem.box;
                        score--;
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x - 1, y] = GameItem.box;
                        score--;
                    }
                    if (BoxStuck(x - 1, y))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && direction.Equals(Direction.right) && (Map[x + 1, y] == GameItem.floor || Map[x + 1, y] == GameItem.target)) // dobozt tolunk jobbra és nincs mögötte fal
                {
                    if(Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x + 1, y] = GameItem.box;
                        score--;
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x + 1, y] = GameItem.box;
                        score--;
                    }
                    if (BoxStuck(x + 1, y))
                    {
                        box_stuck = true;
                    }
                }
                else if (Map[x, y] == GameItem.floor) // üres mezőre lépünk
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player;
                    score--;
                }
                else if (Map[x, y] == GameItem.target || Map[x, y] == GameItem.box_on_target) // targetre lépünk
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player_on_target;
                    score--;
                }
            }
            if(box_stuck)
            {
                MessageBox.Show("The box stucked. You lose 1 HP and you can restart the level.");
                if(life != 0)
                {
                    life--;
                    LoadMap(levels[levelIndex]);

                }
                else
                {
                    MessageBox.Show($"Defeat! Your score is: {score}");
                    shouldICloseTheWindow = true;
                }
                
            }
            if(MapDone())
            {
                if(levelIndex <= levels.Count)
                {
                    MessageBox.Show($"{score}");
                    levelIndex++;
                    LoadMap(levels[levelIndex]);
                }
                else
                {
                    MessageBox.Show($"Victory! Your score is: {score}");
                    shouldICloseTheWindow = true;
                }
            }
        }
        private int score; // játékos pontszáma (PBA-LSZ-EIM-BP lásd specifikáció/pontozás)
        private int[] CurrentPosition()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if(Map[i,j] == GameItem.player || Map[i,j] == GameItem.player_on_target)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return new int[] { -1, -1 };
        }
        private bool BoxStuck(int x, int y) // ellenőrzi, hogy egy doboz olyan helyzetben van-e, ahonnan már biztosan nem lehet tovább mozgatni és nincs a helyén
        {
            var u = Map[x, y - 1];
            var r = Map[x + 1, y];
            var l = Map[x - 1, y];
            var d = Map[x, y + 1];
            if (Map[x, y] == GameItem.box)
            {
                if (u == GameItem.wall && (r == GameItem.wall || l == GameItem.wall))
                {
                    return true;
                }
                else if (r == GameItem.wall && (u == GameItem.wall || d == GameItem.wall))
                {
                    return true;
                }
                else if (l == GameItem.wall && (u == GameItem.wall || d == GameItem.wall))
                {
                    return true;
                }
                else if (d == GameItem.wall && (r == GameItem.wall || l == GameItem.wall))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
                case 'X':
                    return GameItem.box_on_target;
                default:
                    return GameItem.floor;
            }
        }
    }
}
