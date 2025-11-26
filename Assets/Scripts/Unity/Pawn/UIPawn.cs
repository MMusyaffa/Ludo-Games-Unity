using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Unity.Boards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LudoGames.Unity.Pawns
{
    class UIPawn : MonoBehaviour, IPointerClickHandler
    {
        public IPawn pawn;
        private PawnManager _pawnManager;
        [SerializeField] private int id;
        [SerializeField] private Image _pawnImage;
        [SerializeField] bool _isPawnSelected;

        public void Init(PawnManager manager, IPawn backendPawn, int pawnId)
        {
            _pawnManager = manager;
            pawn = backendPawn;
            id = pawnId;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectPawn();
        }

        public void SetPawnId(int indexId)
        {
            id = indexId;
        }

        public int GetPawnId()
        {
            return id;
        }

        public void SetPawnUIColor(Color color)
        {
            _pawnImage.color = color;
        }

        public int SelectPawn()
        {
            if (!_isPawnSelected)
            {
                _isPawnSelected = true;
                PawnManager.Instance.OnClickedPawn(this);
                Debug.Log($"Pawn: {id}, {pawn} selected");

                return id;
            }
            return -1;
        }

        public void DeselectPawn()
        {
            if (_isPawnSelected)
            {
                _isPawnSelected = false;
                Debug.Log($"Pawn: {id}, {pawn} deselected");
            }
        }

        public void MovePawnUI(TileManager tileManager, IPlayer player)
        {
            var path = tileManager.GetPlayerPathTiles(player);
            var tile = path[pawn.PositionIndex];

            transform.position = tile.position;

            Debug.Log($"POS IDX = {pawn.PositionIndex} | PATH COUNT = {path.Count}");
            Debug.Log($"TARGET TILE = {path[pawn.PositionIndex].name}");

            Debug.Log($"Move Pawn | IDX: {pawn.PositionIndex} | PawnPos: {transform.position} | TilePos: {tile.position}"
            );
        }
    }
}