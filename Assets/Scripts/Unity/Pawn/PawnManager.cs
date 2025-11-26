using System.Collections.Generic;
using UnityEngine;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Enums.Colors;
using LudoGames.Unity.GameManagers;
using LudoGames.Unity.Boards;
using LudoGames.Enums.PawnStates;

namespace LudoGames.Unity.Pawns
{
    class PawnManager : MonoBehaviour
    {
        public static PawnManager Instance { get; private set; }
        [SerializeField] private TileManager _tileManager;
        // [SerializeField] private GameManager _gameManager;
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
            Color pawnColorUI = GetPlayerColor(player.ColorEnum);

            UIPawnRegistry[pawn] = pawnObject;
            pawnObject.pawn = pawn;
            pawnObject.SetPawnUIColor(pawnColorUI);

            if (GameManager.Instance._game.PlayerPawns.TryGetValue(player, out var Pawns))
            {
                int pawnIndex = GameManager.Instance._game.PlayerPawns[player].IndexOf(pawn);
                Transform homeTile = GetPlayerPawnHome(player, pawnIndex);

                pawnObject.SetPawnId(pawnIndex);
                pawnObject.transform.position = homeTile.position;
                pawnObject.Init(this, pawn, pawnIndex);
            } else { Debug.Log($"Player {player} belum ada di PlayerPawns dictionary!"); }
        }

        public void OnClickedPawn(UIPawn pawnUI)
        {
            ResetPawnSelections(pawnUI);
            var game = GameManager.Instance._game;
            var player = game.currentPlayerTurn;
            var dice = GameManager.Instance.diceNumberResult;
            var selectedPawn = game.SelectPawn(player, dice, pawnUI.GetPawnId());

            if(selectedPawn != null)
            {
                Debug.Log($"Pawn {pawnUI.GetPawnId()} milik {player.Name} dipilih untuk bergerak!");
                // OnMovedPawn(selectedPawn);
            }
        }

        public void ResetPawnSelections(UIPawn exceptPawn = null)
        {
            foreach (var uiPawn in UIPawnRegistry.Values)
            {
                if (uiPawn != exceptPawn)
                {
                    uiPawn.DeselectPawn();
                }
            }
        }

        private void OnMovedPawn(IPawn pawn)
        {
            var game = GameManager.Instance._game;
            var player = game.currentPlayerTurn;
            var dice = GameManager.Instance.diceNumberResult;
            bool CanPlayerMovePawn = game.CanPlayerMovePawn(player, dice);
            var gameIPawn = GetUIPawn(pawn);

            if(gameIPawn)
            {
                if (CanPlayerMovePawn)
                {
                    if (pawn.PawnStatesEnum == PawnStatesEnum.AtHome)
                    {
                        if (dice == 6)
                        {
                            Debug.Log("Pawn keluar dari home");
                            pawn.PawnStatesEnum = PawnStatesEnum.OnBoard;
                            gameIPawn.MovePawnUI(_tileManager, player);
                        }
                        else
                        {
                            game.NextTurn();
                        }
                    }
                    else
                    {
                        game.MovePawn(player, pawn, dice);

                        var updatePawnUI = GetUIPawn(pawn);
                        if(updatePawnUI != null)
                        {
                            updatePawnUI.MovePawnUI(_tileManager, player);
                        }

                        if(dice != 6) 
                        { 
                            game.NextTurn(); 
                        }
                    }
                    
                }
            }
        }
    }
}