using LudoGames.Types.Coordinates;
using LudoGames.Enums.TileTypes;

namespace LudoGames.Interface.Tiles
{
    interface ITile
    {
        TileTypesEnum TileTypes { get; }
        Coordinate Coordinate { get; }
    }
}