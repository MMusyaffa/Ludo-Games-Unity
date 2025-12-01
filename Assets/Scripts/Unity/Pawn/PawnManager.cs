using System.Collections.Generic;
using UnityEngine;
using LudoGames.Enums.Colors;
using LudoGames.Enums.PawnStates;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Unity.GameManagers;
using LudoGames.Unity.Tiles;

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
            // DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            var game = GameManager.Instance._game;
            game.OnSpawnedPawn = OnSpawnedPawn;
            game.OnMovedPawn = OnMovedPawn;
            game.OnReturnPawn = OnReturnPawn;
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
                    return Color.magenta;
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
                pawnObject.homePawn = homeTile;
                pawnObject.Init(this, player, pawnIndex);

                ResetAllIndicatorPawn();
            } 
            else 
            { 
                Debug.Log($"Player {player} belum ada di PlayerPawns dictionary!"); 
            }
        }

        public void ResetAllIndicatorPawn()
        {
            foreach (var pawn in UIPawnRegistry.Values)
            {
                pawn.SetIndicator(false);
            }
        }

        public void ControlPawnIndicator(bool indicator)
        {
            var game = GameManager.Instance._game;
            var currentPlayer = game.currentPlayerTurn;

            ResetAllIndicatorPawn();

            if (!game.CanPlayerMovePawn(currentPlayer, GameManager.Instance.diceNumberResult))
            {
                return;
            }

            foreach (var pawn in UIPawnRegistry.Values)
            {
                if (pawn.Owner != currentPlayer)
                {
                    continue;
                }

                if (GameManager.Instance.diceNumberResult == 6)
                {
                    pawn.SetIndicator(indicator);
                    Debug.Log($"Owner: {pawn.Owner}");
                    continue;
                }

                if (pawn.pawn.PawnStatesEnum == PawnStatesEnum.OnBoard || pawn.pawn.PawnStatesEnum == PawnStatesEnum.OnFinishPath)
                {
                    pawn.SetIndicator(indicator);
                    Debug.Log($"Owner: {pawn.Owner}, state {pawn.pawn.PawnStatesEnum}");
                }
            }
        }

        public void SetSelectedPawn(UIPawn pawnUI)
        {
            var currentPlayer = GameManager.Instance._game.currentPlayerTurn;

            // ResetAllIndicatorPawn();

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

            // ResetAllIndicatorPawn();
            // pawnUI.SetIndicator(true);
            pawnSelectedIndex = pawnUI.Id;
            GameManager.Instance.ControlMovePawn(pawnSelectedIndex);

            Debug.Log($"Pawn {currentPlayer.Name} {pawnSelectedIndex} selected");
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
        }

        private void OnReturnPawn(IPawn pawn)
        {
            var game = GameManager.Instance._game;
            var knockedPlayerUI = game.GetPlayerPawnOwner(pawn);
            var knockedPawnUI = GetUIPawn(pawn);

            Debug.Log("Return pawn out");

            if (knockedPlayerUI != null && knockedPawnUI != null)
            {
                Debug.Log("Return pawn in");

                knockedPawnUI.ReturnPawnUI(knockedPlayerUI);
            }
        }
    }
}