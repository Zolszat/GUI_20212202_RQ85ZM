using Nikoban.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Nikoban.Renderer
{
    public class Display : FrameworkElement
    {
        private Size size;
        IGameModel model;
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if(model != null && size.Width>0 && size.Height>0)
            {
                double tileHeight = size.Height / model.Map.GetLength(0);
                double tileWidth = size.Width / model.Map.GetLength(1);
                for (int i = 0; i < model.Map.GetLength(0); i++)
                {
                    for (int j = 0; j < model.Map.GetLength(1); j++)
                    {
                        switch (model.Map[i,j])
                        {
                            case GameLogic.GameItem.player:
                                drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 1), new Rect(i * tileWidth, j * tileHeight, tileWidth, tileHeight));
                                break;
                            case GameLogic.GameItem.wall:
                                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), new Rect(i * tileWidth, j * tileHeight, tileWidth, tileHeight));
                                break;
                            case GameLogic.GameItem.floor:
                                drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 1), new Rect(i * tileWidth, j * tileHeight, tileWidth, tileHeight));
                                break;
                            case GameLogic.GameItem.box:
                                drawingContext.DrawRectangle(Brushes.Brown, new Pen(Brushes.Black, 1), new Rect(i * tileWidth, j * tileHeight, tileWidth, tileHeight));
                                break;
                            case GameLogic.GameItem.target:
                                drawingContext.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 1), new Rect(i * tileWidth, j * tileHeight, tileWidth, tileHeight));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public void Resize(Size size)
        {
            this.size = size;
            this.InvalidateVisual();
        }
    }
}
