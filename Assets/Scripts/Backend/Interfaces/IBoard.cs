using LudoGames.Interface.Tiles;

namespace LudoGames.Interface.Boards
{
    interface IBoard
    {
        int Height { get; set; }
        int Width { get; set; }
        ITile[,] Tiles { get; set; }
    }
}