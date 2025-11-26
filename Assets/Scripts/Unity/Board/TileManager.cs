using System;
using System.Collections.Generic;
using LudoGames.Games.GameController;
using LudoGames.Interface.Pawns;
using LudoGames.Types.Coordinates;
using LudoGames.Unity.Pawns;
using UnityEngine;

namespace LudoGames.Unity.Boards
{
    class TileManager : MonoBehaviour
    {
        int height;
        int width;
        public Dictionary<Coordinate, UITile> UITileRegistry = new Dictionary<Coordinate, UITile>();
        public Dictionary<IPawn, UIPawn> UIPawnRegistry = new Dictionary<IPawn, UIPawn>();
        public float size;
        public Vector2 offset;
        public Transform tileContainer;
        public UITile tilePrefab;
        public UIPawn pawnPrefabs;
        public Transform pawnContainer;

        void OnEnable()
        {
            GameController.makePath = OnAddPath;
            GameController.OnSpawnPawn = OnSpawnPawn;
            GameController.OnMovedPawn = OnMovedPawn;
        }

        public void OnAddPath(Coordinate coordinate)
        {
            var tile = Instantiate(tilePrefab, tileContainer);
            
            UITileRegistry[coordinate] = tile;
            tile.transform.localPosition = new Vector2(coordinate.X * size + offset.x, coordinate.Y * -size + offset.y);
            tile.labelCoordinate.text = $"{coordinate.X},{coordinate.Y}";
            Debug.Log($"Delegate call: {coordinate}");
        }

        public UITile GetUITile(Coordinate coordinate)
        {
            return UITileRegistry.GetValueOrDefault(coordinate);
        }

        public void OnSpawnPawn(IPawn pawn)
        {
            var pawnObject = Instantiate(pawnPrefabs, pawnContainer);
            UIPawnRegistry[pawn] = pawnObject;

            pawnObject.pawn = pawn;
        }

        private void OnMovedPawn(IPawn pawn)
        {
            var gameIPawn = GetUIPawn(pawn);
            if(gameIPawn)
            {
                gameIPawn.MovePawn(this);
            }
        }

        public UIPawn GetUIPawn(IPawn pawn)
        {
            return UIPawnRegistry.GetValueOrDefault(pawn);
        }
    }
}