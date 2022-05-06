using Nikoban.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nikoban
{
    class GameController
    {
        IGameControl control;
        public GameController(IGameControl control)
        {
            this.control = control;
        }
        public void KeyPressed(Key key)
        {
            switch(key)
            {
                case Key.Up:
                control.Move(GameLogic.Direction.left);
                    break;
                case Key.Down:
                    control.Move(GameLogic.Direction.right);
                    break;
                case Key.Left:
                    control.Move(GameLogic.Direction.up);
                    break;
                case Key.Right:
                    control.Move(GameLogic.Direction.down);
                    break;
                case Key.Escape:
                    control.Move(GameLogic.Direction.escape);
                    break;
            }
        }
    }
}
