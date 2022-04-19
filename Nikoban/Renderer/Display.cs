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
        }
        public void Resize(Size size)
        {
            this.size = size;
            this.InvalidateVisual();
        }
    }
}
