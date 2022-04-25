using Nikoban.Logic;
using Nikoban.Renderer;
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
    /// Interaction logic for LevelWindow.xaml
    /// </summary>
    public partial class LevelWindow : Window
    {
        GameController controller;
        public LevelWindow()
        {
            InitializeComponent();
            GameLogic logic = new GameLogic();
            display.SetupModel(logic);
            controller = new GameController(logic);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            controller.KeyPressed(e.Key);
            display.InvalidateVisual();
        }
    }
}
