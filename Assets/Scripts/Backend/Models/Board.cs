using LudoGames.Types.Coordinates;
using LudoGames.Interface.Tiles;
using LudoGames.Interface.Boards;
using LudoGames.Models.Tiles;

namespace LudoGames.Models.Boards
{    
    class Board : IBoard
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public ITile[,] Tiles { get; set; }

        public Board(int height, int width, ITile[,] tiles = null)
        {
            Height = height;
            Width = width;

            if(tiles == null)
            {
                tiles = new ITile[height, width];

                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        tiles[x, y] = new Tile(new Coordinate(x, y));
                    }
                }
            }

            Tiles = tiles;
        }
    }
}