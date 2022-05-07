using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nikoban
{
    /// <summary>
    /// Interaction logic for ScoreBoardWindow.xaml
    /// </summary>
    public partial class ScoreBoardWindow : Window
    {
        public ScoreBoardWindow()
        {
            InitializeComponent();

            List<Result> Results = new List<Result>();
            List<string> Scores = File.ReadAllLines(@"Scores\score.txt").ToList();

            foreach (string item in Scores)
            {
                string[] scoreItem = item.Split(" ",StringSplitOptions.None);
                Results.Add(new Result(scoreItem[0], int.Parse(scoreItem[1])));
            }

            Results.OrderBy(x => x.Score);

            foreach (Result item in Results)
            {
                lb_Scores.Items.Add(item.ToString());
            }


        }
    }
    public class Result
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Result(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public override string ToString()
        {
            return $"{Name} {Score}";
        }

    }
}
