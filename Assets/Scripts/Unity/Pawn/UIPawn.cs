using UnityEngine;
using UnityEngine.UI;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Unity.Tiles;

namespace LudoGames.Unity.Pawns
{
    class UIPawn : MonoBehaviour
    {
        public int Id;
        public IPlayer Owner;
        public IPawn pawn;
        public Transform homePawn;
        private PawnManager _pawnManager;
        [SerializeField] private Image _pawnImage;
        [SerializeField] private Image _pawnIndicatorImage;
        [SerializeField] private Button _pawnButton;

        public void Init(PawnManager manager, IPlayer owner, int pawnId)
        {
            _pawnManager = manager;
            Owner = owner;
            Id = pawnId;
            _pawnButton.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            _pawnManager.SetSelectedPawn(this);
        }

        public void SetPawnId(int indexId)
        {
            Id = indexId;
        }

        public int GetPawnId()
        {
            return Id;
        }

        public void SetPawnUIColor(Color color)
        {
            _pawnImage.color = color;
        }

        public void SetIndicator(bool active)
        {
            if (_pawnIndicatorImage != null)
            {
                _pawnIndicatorImage.gameObject.SetActive(active);
            }
        }

        public void MovePawnUI(TileManager tileManager, IPlayer player)
        {
            var path = tileManager.GetPlayerPathTiles(player);
            var tile = path[pawn.PositionIndex];

            transform.position = tile.position;

            Debug.Log($"Path Count: {path.Count}, Target: {path[pawn.PositionIndex].name} " + 
                    $"Move Pawn | IDX: {pawn.PositionIndex} | PawnPos: {transform.position} | TilePos: {tile.position}");
        }

        public void ReturnPawnUI(IPlayer player)
        {
            // var path = tileManager.GetPlayerPathTiles(player);
            // var tile = path[pawn.PositionIndex = 0];

            transform.position = homePawn.position;

            Debug.Log($"From UIPawn {player.Name}");
        }

        public void MoveNextPawnUI(TileManager tileManager, IPlayer player)
        {
            var path = tileManager.GetPlayerPathTiles(player);
            var tile = path[pawn.PositionIndex];

            Vector3 newPos = new Vector3(
                tile.position.x + 1f,
                tile.position.y,
                tile.position.z
            );

            transform.position = newPos;
        }
    }
}