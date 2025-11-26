using System.Collections.Generic;
using UnityEngine;
using LudoGames.Types.Coordinates;
using LudoGames.Interface.Players;
using LudoGames.Enums.Colors;

namespace LudoGames.Unity.Boards
{
    class TileManager : MonoBehaviour
    {
        private Dictionary<Coordinate, UITile> UITileRegistry = new Dictionary<Coordinate, UITile>();
        [SerializeField] private float size;
        [SerializeField] private Vector2 offset;
        [SerializeField] private Transform tileContainer;
        [SerializeField] private UITile tilePrefab;
        [SerializeField] private List<Transform> tilesPathA;
        [SerializeField] private List<Transform> tilesPathB;
        [SerializeField] private List<Transform> tilesPathC;
        [SerializeField] private List<Transform> tilesPathD;

        void OnEnable()
        {
            // GameController.makePath = OnAddPath;
        }

        public UITile GetUITile(Coordinate coordinate)
        {
            return UITileRegistry.GetValueOrDefault(coordinate);
        }

        public void OnAddPath(Coordinate coordinate)
        {
            var tile = Instantiate(tilePrefab, tileContainer);
            
            UITileRegistry[coordinate] = tile;
            tile.transform.localPosition = new Vector2(coordinate.X * size + offset.x, coordinate.Y * -size + offset.y);
            tile.labelCoordinate.text = $"{coordinate.X},{coordinate.Y}";
            Debug.Log($"Delegate call: {coordinate}");
        }

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