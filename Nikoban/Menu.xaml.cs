using System;
using System.Collections.Generic;
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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            buttongrid2.Visibility = Visibility.Hidden;
            buttongrid3.Visibility = Visibility.Hidden;
            buttongrid4.Visibility = Visibility.Hidden;
            credit.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(5);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            buttongrid.Visibility = Visibility.Hidden;
            buttongrid3.Visibility = Visibility.Visible;
        }

        private void Button_Click_P(object sender, RoutedEventArgs e)
        {
            LevelWindow lvl = new LevelWindow(Logic.GameMode.playthrough);
            lvl.display.Selected_texture = Renderer.SelectedTexture.pirate;
            this.Hide();
            lvl.ShowDialog();
            buttongrid.Visibility = Visibility.Visible;
            buttongrid2.Visibility = Visibility.Hidden;
            this.Show();
        }

        private void Button_Click_S(object sender, RoutedEventArgs e)
        {
            LevelWindow lvl = new LevelWindow(Logic.GameMode.playthrough);
            lvl.display.Selected_texture = Renderer.SelectedTexture.shrek;
            this.Hide();
            lvl.ShowDialog();
            buttongrid.Visibility = Visibility.Visible;
            buttongrid2.Visibility = Visibility.Hidden;
            this.Show();
        }

        private void Button_Click_SW(object sender, RoutedEventArgs e)
        {
            LevelWindow lvl = new LevelWindow(Logic.GameMode.playthrough);
            lvl.display.Selected_texture = Renderer.SelectedTexture.star_wars;
            this.Hide();
            lvl.ShowDialog();
            buttongrid.Visibility = Visibility.Visible;
            buttongrid2.Visibility = Visibility.Hidden;
            this.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            buttongrid4.Visibility = Visibility.Hidden;
            buttongrid3.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            buttongrid3.Visibility = Visibility.Hidden;
            buttongrid2.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            buttongrid3.Visibility = Visibility.Hidden;
            buttongrid4.Visibility = Visibility.Visible;
        }

        private void Button_Click_CreditBack(object sender, RoutedEventArgs e)
        {
            credit.Visibility = Visibility.Hidden;
            buttongrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_Credits(object sender, RoutedEventArgs e)
        {
            credit.Visibility = Visibility.Visible;
            buttongrid.Visibility = Visibility.Hidden;
        }

        private void Button_Click_BackToMenu(object sender, RoutedEventArgs e)
        {
            buttongrid3.Visibility = Visibility.Hidden;
            buttongrid.Visibility = Visibility.Visible;
        }
        private void Button_Click_BackToChoosing(object sender, RoutedEventArgs e)
        {
            buttongrid2.Visibility = Visibility.Hidden;
            buttongrid3.Visibility = Visibility.Visible;
        }

        private void Button_Click_FunMode(object sender, RoutedEventArgs e)
        {
            LevelWindow lvl = new LevelWindow(Logic.GameMode.funmode);
            Random r = new Random();
            int x = r.Next(1, 4);
            if(x == 1)
            {
                lvl.display.Selected_texture = Renderer.SelectedTexture.star_wars;
            }
            else if(x == 2)
            {
                lvl.display.Selected_texture = Renderer.SelectedTexture.pirate;
            }
            else
            {
                lvl.display.Selected_texture = Renderer.SelectedTexture.shrek;
            }
            this.Hide();
            lvl.ShowDialog();
            buttongrid.Visibility = Visibility.Visible;
            buttongrid2.Visibility = Visibility.Hidden;
            this.Show();
        }
    }
}
