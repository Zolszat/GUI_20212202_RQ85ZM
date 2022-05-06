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
    /// Interaction logic for ScoreWindow.xaml
    /// </summary>
    public partial class ScoreWindow : Window
    {
        public ScoreWindow(int newScore)
        {
            InitializeComponent();

            List<string> Scores = File.ReadAllLines(@"Scores\score.txt").ToList();
            Scores.Add(newScore.ToString());
            Scores = Scores.OrderByDescending(x => int.Parse(x)).ToList();
            File.AppendText(newScore.ToString());

            foreach (var item in Scores)
            {
                sp_scores.Items.Add(item);
            }



        }
    }
}
