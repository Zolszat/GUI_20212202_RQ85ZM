using static Nikoban.Logic.GameLogic;

namespace Nikoban.Logic
{
    public interface IGameModel
    {
        GameItem[,] Map { get; set; }
        int Life { get; set; }
    }
}