using static Nikoban.Logic.GameLogic;

namespace Nikoban.Logic
{
    internal interface IGameControl
    {
        void Move(Direction direction);
    }
}