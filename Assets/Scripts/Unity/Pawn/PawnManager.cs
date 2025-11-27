using System.Collections.Generic;
using UnityEngine;
using LudoGames.Enums.Colors;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Unity.GameManagers;
using LudoGames.Unity.Boards;

namespace LudoGames.Unity.Pawns
{
    class PawnManager : MonoBehaviour
    {
        public static PawnManager Instance { get; private set; }
        [SerializeField] private TileManager _tileManager;
        // [SerializeField] private GameManager _gameManager;
        public int pawnSelectedIndex = -1;
        [SerializeField] private Transform _pawnContainer;
        [SerializeField] private UIPawn _pawnPrefabs;
        [SerializeField] private List<Transform> _homePawnPathA;
        [SerializeField] private List<Transform> _homePawnPathB;
        [SerializeField] private List<Transform> _homePawnPathC;
        [SerializeField] private List<Transform> _homePawnPathD;
        private Dictionary<IPawn, UIPawn> UIPawnRegistry = new Dictionary<IPawn, UIPawn>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            var game = GameManager.Instance._game;
            game.OnSpawnedPawn = OnSpawnedPawn;
            game.OnMovedPawn += OnMovedPawn;
        }

        private Transform GetPlayerPawnHome(IPlayer player, int index)
        {
            switch (player.ColorEnum)
            {
                case ColorsEnum.Red:
                    return _homePawnPathA[index];
                case ColorsEnum.Green:
                    return _homePawnPathB[index];
                case ColorsEnum.Yellow:
                    return _homePawnPathC[index];
                case ColorsEnum.Blue:
                    return _homePawnPathD[index];
            }

            return null;
        }

        private Color GetPlayerColor(ColorsEnum colorEnum)
        {
            switch(colorEnum)
            {
                case ColorsEnum.Red:
                    return Color.red;
                case ColorsEnum.Green:
                    return Color.green;
                case ColorsEnum.Yellow:
                    return Color.yellow;
                case ColorsEnum.Blue:
                    return Color.cyan;
            }
            return Color.white;
        }

        public UIPawn GetUIPawn(IPawn pawn)
        {
            return UIPawnRegistry.GetValueOrDefault(pawn);
        }

        public void OnSpawnedPawn(IPlayer player, IPawn pawn)
        {
            var pawnObject = Instantiate(_pawnPrefabs, _pawnContainer);
            var playerPawn = GameManager.Instance._game.PlayerPawns;
            Color pawnColorUI = GetPlayerColor(player.ColorEnum);

            UIPawnRegistry[pawn] = pawnObject;
            pawnObject.pawn = pawn;
            pawnObject.SetPawnUIColor(pawnColorUI);

            if (playerPawn.TryGetValue(player, out var Pawns))
            {
                int pawnIndex = playerPawn[player].IndexOf(pawn);
                Transform homeTile = GetPlayerPawnHome(player, pawnIndex);

                pawnObject.SetPawnId(pawnIndex);
                pawnObject.transform.position = homeTile.position;
                pawnObject.Init(this, player, pawnIndex);
            } 
            else 
            { 
                Debug.Log($"Player {player} belum ada di PlayerPawns dictionary!"); 
            }
        }

        public void SetSelectedPawn(UIPawn pawnUI)
        {
            var currentPlayer = GameManager.Instance._game.currentPlayerTurn;

            if (!GameManager.Instance.isDiceRolled)
            {
                Debug.Log("Harus roll dadu terlebih dahulu!");
                return;
            }

            if (GameManager.Instance.isPawnAlreadyMoved)
            {
                Debug.Log("Cannot move another pawn this turn!");
                return;
            }

            if (pawnUI.Owner != currentPlayer)
            {
                Debug.Log($"{currentPlayer} Cannot select other player {pawnUI.Owner} pawn");
                return;
            }

            pawnSelectedIndex = pawnUI.Id;
            Debug.Log($"Pawn {currentPlayer.Name} {pawnSelectedIndex} selected");

            GameManager.Instance.ControlMovePawn(pawnSelectedIndex);
        }


        private void OnMovedPawn(IPawn pawn)
        {
            var game = GameManager.Instance._game;
            var currentPlayer = game.currentPlayerTurn;
            var pawnUI = GetUIPawn(pawn);

            if(pawnUI)
            {
                pawnUI.MovePawnUI(_tileManager, currentPlayer);
            }

            // var knockedPawn = game.IsPawnCollide(currentPlayer, pawn);

            // if (knockedPawn != null)
            // {
            //     var knockedPlayerUI = game.GetPlayerPawnOwner(knockedPawn);
            //     var knockedPawnUI = GetUIPawn(pawn);

            //     knockedPawnUI.ReturnPawnUI(_tileManager, knockedPlayerUI);
            // }
        }
    }
}