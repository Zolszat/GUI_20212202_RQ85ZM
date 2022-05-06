using Nikoban.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nikoban.Renderer
{
    public enum SelectedTexture
    {
        star_wars, pirate, shrek
    }
    public class Display : FrameworkElement
    {
        private Size size;
        IGameModel model;
        public SelectedTexture Selected_texture { get; set; }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (model != null && size.Width > 0 && size.Height > 0)
            {
                double tileHeight = size.Height / model.Map.GetLength(0);
                double tileWidth = size.Width / model.Map.GetLength(1);
                ImageBrush brush = new ImageBrush();
                if (Selected_texture == SelectedTexture.pirate)
                {
                    for (int i = 0; i < model.Map.GetLength(0); i++)
                    {
                        for (int j = 0; j < model.Map.GetLength(1); j++)
                        {
                            switch (model.Map[i, j])
                            {
                                case GameLogic.GameItem.player:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/character_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.wall:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/wall_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.floor:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/floor_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.box:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/box_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/finish_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.box_on_target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/box_on_target_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.player_on_target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/pirate_template/player_finish_p.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                default:
                                    break;
                            }
                            drawingContext.DrawRectangle(brush, new Pen(Brushes.Black, 1), new Rect(j * tileWidth, i * tileHeight, tileWidth, tileHeight));
                        }
                    }
                }
                else if (Selected_texture == SelectedTexture.shrek)
                {
                    for (int i = 0; i < model.Map.GetLength(0); i++)
                    {
                        for (int j = 0; j < model.Map.GetLength(1); j++)
                        {
                            switch (model.Map[i, j])
                            {
                                case GameLogic.GameItem.player:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/character_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.wall:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/wall_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.floor:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/floor_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.box:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/box_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/finish_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.box_on_target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/box_on_target_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.player_on_target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/shrek_template/player_finish_s.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                default:
                                    break;
                            }
                            drawingContext.DrawRectangle(brush, new Pen(Brushes.Black, 1), new Rect(j * tileWidth, i * tileHeight, tileWidth, tileHeight));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < model.Map.GetLength(0); i++)
                    {
                        for (int j = 0; j < model.Map.GetLength(1); j++)
                        {
                            switch (model.Map[i, j])
                            {
                                case GameLogic.GameItem.player:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/character_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.wall:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/wall_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.floor:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/floor_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.box:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/box_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/finish_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.box_on_target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/box_on_target_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.GameItem.player_on_target:
                                    brush = new ImageBrush(new BitmapImage(new Uri("Images/star_wars_template/player_finish_sw.jpg", UriKind.RelativeOrAbsolute)));
                                    break;
                                default:
                                    break;
                            }
                            drawingContext.DrawRectangle(brush, new Pen(Brushes.Black, 1), new Rect(j * tileWidth, i * tileHeight, tileWidth, tileHeight));
                        }
                    }
                }
            }
        }
        public void Resize(Size size)
        {
            this.size = size;
            InvalidateVisual();
        }
    }
}
