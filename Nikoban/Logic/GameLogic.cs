using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nikoban.Logic
{
    public enum GameMode
    {
        funmode, playthrough
    }

    public class GameLogic : IGameModel, IGameControl
    {
        public enum GameItem
        {
            player, wall, floor, box, target, box_on_target, player_on_target // lehetséges elemek a pályán
        }

        public enum Direction
        {
            up, down, left, right, escape
        }
        private Random r;
        int funIndex;
        public GameItem[,] Map { get; set; }
        public GameMode gameMode { get; set; }
        public bool[,] TargetCheckMap { get; set; }
        public int Life { get; set; }

        private List<string> levels;
        int levelIndex;
        public GameLogic()
        {

        }
        public GameLogic(GameMode gm)
        {
            this.gameMode = gm;
            r = new Random();
            levelIndex = 0;
            score = 0;
            Life = 3;
            levels = new List<string>();
            foreach (var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Levels")))
            {
                levels.Add(item);
            }
            funIndex = r.Next(0, levels.Count - 1);
            if (gameMode == GameMode.funmode)
            {
                LoadMap(levels[funIndex]);
            }
            else
            {
                LoadMap(levels[levelIndex]);
            }
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
            int future_x = x;
            int future_y = y;
            bool box_stuck = false;

            switch (direction)
            {
                case Direction.up:
                    y--;
                    future_y -= 2;
                    break;
                case Direction.down:

                    y++;
                    future_y += 2;

                    break;
                case Direction.left:

                    x--;
                    future_x -= 2;

                    break;
                case Direction.right:
                    x++;
                    future_x += 2;
                    break;
                default:
                    break;
            }
            if (((Map[x, y] == GameItem.box || Map[x, y] == GameItem.box_on_target) && (Map[future_x, future_y] != GameItem.wall && Map[future_x, future_y] != GameItem.box && Map[future_x, future_y] != GameItem.box_on_target))
                || ((Map[x, y] == GameItem.floor) || (Map[x, y] == GameItem.target))) //lehet tolni
            {
                if (Map[old_x, old_y] == GameItem.player)
                {
                    Map[old_x, old_y] = GameItem.floor;
                }
                else
                {
                    Map[old_x, old_y] = GameItem.target;
                }
                if (Map[x, y] == GameItem.box)
                {
                    Map[x, y] = GameItem.player;
                    if (Map[future_x, future_y] == GameItem.floor)
                    {
                        Map[future_x, future_y] = GameItem.box;
                    }
                    else
                    {
                        Map[future_x, future_y] = GameItem.box_on_target;
                    }
                }
                else if (Map[x, y] == GameItem.box_on_target)
                {
                    Map[x, y] = GameItem.player_on_target;
                    if (Map[future_x, future_y] == GameItem.floor)
                    {
                        Map[future_x, future_y] = GameItem.box;
                    }
                    else
                    {
                        Map[future_x, future_y] = GameItem.box_on_target;
                    }
                }
                else if (Map[x, y] == GameItem.floor)
                {
                    Map[x, y] = GameItem.player;
                }
                else if (Map[x, y] == GameItem.target)
                {
                    Map[x, y] = GameItem.player_on_target;
                }
                if ((Map[future_x, future_y] == GameItem.box || Map[future_x, future_y] == GameItem.box) && BoxStuck(future_x, future_y))
                {
                    box_stuck = true;
                }
            }
            if (direction == Direction.escape)
            {
                if (MessageBox.Show("Do you want to quit?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (gameMode == GameMode.playthrough)
                    {
                        score -= 100;
                    }
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
                    if (MessageBox.Show("Do you want to reload the game?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
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
                }
            }
            if (box_stuck)
            {
                if (Life > 1)
                {
                    if (gameMode == GameMode.playthrough)
                    {
                        Life--;
                        foreach (var item in Application.Current.Windows)
                        {
                            if (item is LevelWindow)
                            {
                                if (Life == 2)
                                {
                                    (item as LevelWindow).hp_bar.Source = new BitmapImage(new Uri("Images/hpbars/hpbar2.png", UriKind.RelativeOrAbsolute));
                                }
                                else
                                {
                                    (item as LevelWindow).hp_bar.Source = new BitmapImage(new Uri("Images/hpbars/hpbar1.png", UriKind.RelativeOrAbsolute));
                                }
                            }
                        }
                        MessageBox.Show($"The box stucked. You have {Life} life left.");
                        score--;
                        LoadMap(levels[levelIndex]);
                    }
                    else
                    {
                        if (MessageBox.Show("Do you want to play it again?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
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
                else
                {
                    if (gameMode == GameMode.playthrough)
                    {
                        foreach (var item in Application.Current.Windows)
                        {
                            if (item is LevelWindow)
                            {
                                (item as LevelWindow).hp_bar.Source = new BitmapImage(new Uri("Images/hpbars/hpbar0.png", UriKind.RelativeOrAbsolute));
                            }
                        }
                        MessageBox.Show($"Defeat! Your score is: {score}");
                    }
                    else
                    {
                        MessageBox.Show("Defeat!");
                    }
                    foreach (var item in Application.Current.Windows)
                    {
                        if (item is LevelWindow)
                        {
                            (item as Window).Close();
                        }

                    }
                }
            }
            if (MapDone())
            {
                if (levelIndex <= levels.Count && gameMode == GameMode.playthrough)
                {
                    MessageBox.Show($"{score}");
                    levelIndex++;
                    score += 100;
                    LoadMap(levels[levelIndex]);
                }
                else if (levelIndex > levels.Count && gameMode == GameMode.playthrough)
                {
                    MessageBox.Show($"Victory! Your score is: {score}");
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
                    if (MessageBox.Show("Do you want to play more?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

        private int score; // játékos pontszáma (PBA-LSZ-EIM-BP lásd specifikáció/pontozás)

        public int Score { get { return score; } }
        private int[] CurrentPosition()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == GameItem.player || Map[i, j] == GameItem.player_on_target)
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

        public bool MapDone()
        {
            bool done = true;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1) && done; j++)
                {
                    if (Map[i, j] == GameItem.box) // van box enum, tehát nincs minden doboz a helyén
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
