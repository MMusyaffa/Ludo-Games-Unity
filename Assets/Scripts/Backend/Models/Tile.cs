using LudoGames.Types.Coordinates;
using LudoGames.Enums.TileTypes;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Tiles;
using System.Collections.Generic;

namespace LudoGames.Models.Tiles
{
    class Tile : ITile
    {
        public TileTypesEnum TileTypes { get; }
        public Coordinate Coordinate { get; }

        public Tile(TileTypesEnum tileTypes, Coordinate coordinate)
        {
            TileTypes = tileTypes;
            Coordinate = coordinate;
        }
    }
}