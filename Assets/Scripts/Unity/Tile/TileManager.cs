using System.Collections.Generic;
using UnityEngine;
using LudoGames.Types.Coordinates;
using LudoGames.Enums.Colors;
using LudoGames.Interface.Players;

namespace LudoGames.Unity.Tiles
{
    class TileManager : MonoBehaviour
    {
        private Dictionary<Coordinate, UITile> UITileRegistry = new Dictionary<Coordinate, UITile>();
        // [SerializeField] private float size;
        // [SerializeField] private Vector2 offset;
        // [SerializeField] private Transform tileContainer;
        // [SerializeField] private UITile tilePrefab;
        [SerializeField] private List<Transform> tilesPathA;
        [SerializeField] private List<Transform> tilesPathB;
        [SerializeField] private List<Transform> tilesPathC;
        [SerializeField] private List<Transform> tilesPathD;

        // public UITile GetUITile(Coordinate coordinate)
        // {
        //     return UITileRegistry.GetValueOrDefault(coordinate);
        // }

        public List<Transform> GetPlayerPathTiles(IPlayer player)
        {
            switch (player.ColorEnum)
            {
                case ColorsEnum.Red:
                    return tilesPathA;
                case ColorsEnum.Green:
                    return tilesPathB;
                case ColorsEnum.Yellow:
                    return tilesPathC;
                case ColorsEnum.Blue:
                    return tilesPathD;
            }

            return null;
        }
    }
}