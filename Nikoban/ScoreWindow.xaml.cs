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
        private int newScore;
        private StreamWriter sw;

        public ScoreWindow(int newScore)
        {
            InitializeComponent();
            this.newScore = newScore;

            List<string> Scores = File.ReadAllLines(@"Scores\score.txt").ToList();
            sp_scores.Content = newScore.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sw = File.AppendText(@"Scores\score.txt");
            //sw.WriteLine($"{tb_name.Text} {newScore}");

            sw.WriteLine(new Result(tb_name.Text.Replace(' ','_'), newScore).ToString());

            sw.Close();

            Close();
        }
    }
}
