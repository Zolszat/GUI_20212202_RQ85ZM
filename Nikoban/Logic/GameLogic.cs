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
        public enum GameMode
        {
            funmode, playthrough
        }
        public enum Direction
        {
            up, down, left, right
        }
        private Random r;
        int funIndex;
        public GameItem[,] Map { get; set; }
        public GameMode gameMode { get; set; }
        public bool[,] TargetCheckMap { get; set; }
        public int Life { get; set; }
        public bool ShouldICloseTheWindow { get; set; }
        private List<string> levels;
        int levelIndex = 0;
        public GameLogic()
        {
            r = new Random();
            gameMode = GameMode.funmode;
            score = 0;
            Life = 5;
            levels = new List<string>();
            foreach(var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(),"Levels")))
            {
                levels.Add(item);
            }
            LoadMap(levels[levelIndex]);

            funIndex = r.Next(0, levels.Count - 1);
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
                    if (ConvertToGameItem(lines[i + 2][j]) == GameItem.target || ConvertToGameItem(lines[i + 2][j]) == GameItem.box_on_target 
                        || ConvertToGameItem(lines[i + 2][j]) == GameItem.player_on_target)
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
                if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.up) && (Map[x, y - 1] == GameItem.floor 
                    || Map[x, y - 1] == GameItem.target)) // dobozt tolunk fel és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x, y - 1] = GameItem.box;
                    if(gameMode == GameMode.playthrough)
                    {
                        score--;
                    }
                    if(BoxStuck(x,y-1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) &&
                    direction.Equals(Direction.down) && (Map[x, y + 1] == GameItem.floor 
                    || Map[x, y + 1] == GameItem.target)) // dobozt tolunk le és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x, y + 1] = GameItem.box;
                    if (gameMode == GameMode.playthrough)
                    {
                        score--;
                    }
                    if (BoxStuck(x, y + 1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.left) && (Map[x - 1, y] == GameItem.floor 
                    || Map[x - 1, y] == GameItem.target)) // dobozt tolunk balra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x - 1, y] = GameItem.box;
                    if (gameMode == GameMode.playthrough)
                    {
                        score--;
                    }
                    if (BoxStuck(x-1, y))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.right) && (Map[x + 1, y] == GameItem.floor 
                    || Map[x + 1, y] == GameItem.target)) // dobozt tolunk jobbra és nincs mögötte fal
                {
                    Map[old_x, old_y] = GameItem.target;
                    Map[x, y] = GameItem.player;
                    Map[x + 1, y] = GameItem.box;
                    if (gameMode == GameMode.playthrough)
                    {
                        score--;
                    }
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
                if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.up) && (Map[x, y - 1] == GameItem.floor 
                    || Map[x, y - 1] == GameItem.target)) // dobozt tolunk fel és nincs mögötte fal
                {
                    if (Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x, y - 1] = GameItem.box;

                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x, y - 1] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    if (BoxStuck(x, y - 1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.down) && (Map[x, y + 1] == GameItem.floor 
                    || Map[x, y + 1] == GameItem.target)) // dobozt tolunk le és nincs mögötte fal
                {
                    if (Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x, y + 1] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x, y + 1] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    if (BoxStuck(x, y + 1))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.left) && (Map[x - 1, y] == GameItem.floor 
                    || Map[x - 1, y] == GameItem.target)) // dobozt tolunk balra és nincs mögötte fal
                {
                    if (Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x - 1, y] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x - 1, y] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    if (BoxStuck(x - 1, y))
                    {
                        box_stuck = true;
                    }
                }
                else if ((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) 
                    && direction.Equals(Direction.right) && (Map[x + 1, y] == GameItem.floor 
                    || Map[x + 1, y] == GameItem.target)) // dobozt tolunk jobbra és nincs mögötte fal
                {
                    if(Map[x, y] == GameItem.box_on_target)
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player_on_target;
                        Map[x + 1, y] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
                    }
                    else
                    {
                        Map[old_x, old_y] = GameItem.floor;
                        Map[x, y] = GameItem.player;
                        Map[x + 1, y] = GameItem.box;
                        if (gameMode == GameMode.playthrough)
                        {
                            score--;
                        }
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
                    if (gameMode == GameMode.playthrough)
                    {
                        score--;
                    }
                }
                else if (Map[x, y] == GameItem.target || Map[x, y] == GameItem.box_on_target 
                    && EmptySpace(Map[x+x-old_x,y+y-old_y]) ) // targetre lépünk
                {
                    Map[old_x, old_y] = GameItem.floor;
                    Map[x, y] = GameItem.player_on_target;
                    if (gameMode == GameMode.playthrough)
                    {
                        score--;
                    }
                }
            }
            if(box_stuck)
            {
                if(Life > 1)
                {
                    Life--;
                    MessageBox.Show($"The box stucked. You have {Life} life left.");
                    if (gameMode == GameMode.playthrough)
                    {
                        score--;
                        LoadMap(levels[levelIndex]);
                    }
                    else
                    {
                        LoadMap(levels[funIndex]);
                    }
                }
                else
                {
                    if (gameMode == GameMode.playthrough)
                    {
                        MessageBox.Show($"Defeat! Your score is: {score}");
                        WriteToText(score.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Defeat!");
                    }
                    foreach (var item in Application.Current.Windows)
                    {
                        if(item is LevelWindow)
                        {
                            (item as Window).Close();
                        }

                    }
                }
            }
            if(MapDone())
            {
                if (levelIndex <= levels.Count && gameMode == GameMode.playthrough)
                {
                    MessageBox.Show($"{score}");
                    levelIndex++;
                    LoadMap(levels[levelIndex]);
                }
                else if(levelIndex > levels.Count && gameMode == GameMode.playthrough)
                {
                    MessageBox.Show($"Victory! Your score is: {score}");
                    WriteToText(score.ToString());
                    foreach (var item in Application.Current.Windows)
                    {
                        if (item is LevelWindow)
                        {
                            (item as Window).Close();
                        }
                    }
                }
                else
                {
                    if(MessageBox.Show("Do you want to play more?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        funIndex = r.Next(1, levels.Count - 1);
                        LoadMap(levels[funIndex]);
                    }
                    else
                    {
                        foreach (var item in Application.Current.Windows)
                        {
                            if (item is LevelWindow)
                            {
                                (item as Window).Close();
                            }
                        }
                    }
                }
                
            }
        }

        private void WriteToText(string text)
        {
            string file = "score.txt";
            if (!File.Exists(file))
            {
                StreamWriter sw = File.CreateText(file);
                sw.WriteLine(text);

            }
            else
            {
                StreamWriter sw = File.AppendText(file);
                sw.WriteLine(text);
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

        private bool EmptySpace(GameItem gameItem)
        {
            if (gameItem == GameItem.floor || gameItem == GameItem.target)
            {
                return true;
            }
            return false;
        }


    }
}
