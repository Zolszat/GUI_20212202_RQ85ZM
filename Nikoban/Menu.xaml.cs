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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(5);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            buttongrid.Visibility = Visibility.Hidden;
            buttongrid2.Visibility = Visibility.Visible;
        }

        private void Button_Click_P(object sender, RoutedEventArgs e)
        {
            LevelWindow lvl = new LevelWindow();
            lvl.display.Selected_texture = Renderer.SelectedTexture.pirate;
            this.Hide();
            lvl.ShowDialog();
            buttongrid.Visibility = Visibility.Visible;
            buttongrid2.Visibility = Visibility.Hidden;
            this.Show();
        }
    }
}
