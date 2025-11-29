using LudoGames.Types.Coordinates;
using LudoGames.Interface.Tiles;

namespace LudoGames.Models.Tiles
{
    class Tile : ITile
    {
        public Coordinate Coordinate { get; }

        public Tile(Coordinate coordinate)
        {
            Coordinate = coordinate;
        }
    }
}