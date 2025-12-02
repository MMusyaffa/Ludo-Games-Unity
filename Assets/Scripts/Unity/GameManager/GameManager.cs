using UnityEngine;
using TMPro;
using LudoGames.Enums.PawnStates;
using LudoGames.Interface.Players;
using LudoGames.Games.GameController;
using LudoGames.Unity.Menus;
using LudoGames.Unity.UI;
using LudoGames.Unity.Pawns;
using System.Linq;
using System.Collections;

namespace LudoGames.Unity.GameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 
        internal GameController _game {get; private set; }
        [SerializeField] private UIGameplay _uiGameplay;
        [SerializeField] private PawnManager _pawnManager;
        [SerializeField] private SfxManager _sfxManager;
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private TextMeshProUGUI _diceNumberUI;
        [SerializeField] private UIDice _dice;
        public bool isPawnAlreadyMoved = false;
        public bool isDiceRolled = false;
        public bool isAnyPawnKilled = false;
        public bool isAnyPawnFinished = false;
        [SerializeField] private CanvasGroup _winningPanel;
        [SerializeField] private TextMeshProUGUI _currentPlayerUI;
        [SerializeField] private TextMeshProUGUI _player1Name;
        [SerializeField] private TextMeshProUGUI _player2Name;
        [SerializeField] private TextMeshProUGUI _player3Name;
        [SerializeField] private TextMeshProUGUI _player4Name;
        public int diceNumberResult = 0;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            // DontDestroyOnLoad(gameObject);

            _game = new GameController();
            diceNumberResult = _game.diceNumber;
            _dice.OnDiceAnimationFinished = OnDiceAnimationFinished;

            _diceNumberUI.text = $"{diceNumberResult}";
            _player1Name.text = $"";
            _player2Name.text = $"";
            _player3Name.text = $"";
            _player4Name.text = $"";

            Debug.Log("Game Started");
        }

        public void AddPlayerButton()
        {
            string name = _playerNameInput.text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Name must filled");
                return;
            }

            if (_game.Players.Count < 4)
            {
                _game.AddNewPlayer(name);
                _playerNameInput.text = "";
                
                Debug.Log($"New Player {name} Added, Path {_game.PlayerPaths}");

                _uiGameplay.player1NameUI.text = _game.Players.Count > 0 ? _game.Players[0].Name : "";
                _uiGameplay.player2NameUI.text = _game.Players.Count > 1 ? _game.Players[1].Name : "";
                _uiGameplay.player3NameUI.text = _game.Players.Count > 2 ? _game.Players[2].Name : "";
                _uiGameplay.player4NameUI.text = _game.Players.Count > 3 ? _game.Players[3].Name : "";
            }
            else
            {
                Debug.LogWarning("Player Full");
            }
        }

        public void StartGame()
        {
            if (_game.Players.Count < 2)
            {
                Debug.Log("Player must 2 or above to start game");
                return;
            }

            Debug.Log("Player list: ");
            foreach (var player in _game.Players)
            {
                Debug.Log($"Name {player.Name}, Color {player.ColorEnum}");
            }

            _game.AssignFirstPlayerTurn();
            _currentPlayerUI.text = $"{_game.currentPlayerTurn.Name}";
            isPawnAlreadyMoved = false;     
            isDiceRolled = false;

            _player1Name.text = _game.Players.Count > 0 ? _game.Players[0].Name : "";
            _player2Name.text = _game.Players.Count > 1 ? _game.Players[1].Name : "";
            _player3Name.text = _game.Players.Count > 2 ? _game.Players[2].Name : "";
            _player4Name.text = _game.Players.Count > 3 ? _game.Players[3].Name : "";
        }

        private void RollDice()
        {
            IPlayer currentPlayer = _game.currentPlayerTurn;

            diceNumberResult = _game.RollDice();;
            _diceNumberUI.text = $"{diceNumberResult}";
            isPawnAlreadyMoved = false;
            isDiceRolled = true;
            // _sfxManager.PlayDiceClip();

            Debug.Log($"Dice Rolled: {diceNumberResult}");

            if(!_game.CanPlayerMovePawn(currentPlayer, diceNumberResult))
            {
                NextTurn();
                return;
            }
            else
            {
                _pawnManager.ControlPawnIndicator(true);
            }
        }

        public void StartRollDice()
        {
            if (!isDiceRolled)
            {
                StartCoroutine(_dice.RollDiceAnimation());
                _sfxManager.PlayDiceClip();
            }
            
        }

        private void OnDiceAnimationFinished()
        {
            RollDice();
            _dice.SetFinalDiceSprite(diceNumberResult);
        }

        public void ControlMovePawn(int pawnIndex)
        {
            var player = _game.currentPlayerTurn;
            var pawn = _game.SelectPawn(player, diceNumberResult, pawnIndex);

            if (pawn.PawnStatesEnum == PawnStatesEnum.AtHome)
            {
                if (diceNumberResult == 6)
                {
                    isPawnAlreadyMoved = true;
                    pawn.PawnStatesEnum = PawnStatesEnum.OnBoard;
                    _game.MovePawn(player, pawn, 0, out _, out _);
                    _sfxManager.PlayPawnClip();
                    isDiceRolled = false;
                    PawnManager.Instance.ResetAllIndicatorPawn();
                    
                    Debug.Log("Pawn keluar dari home");

                    return;
                }
                else
                {
                    return;
                }
            }

            bool killedPawn;
            bool finishedPawn;
            bool pawnValidMove = _game.MovePawn(player, pawn, diceNumberResult, out killedPawn, out finishedPawn);
            
            if (!pawnValidMove)
            {
                return;
            }

            isAnyPawnKilled = killedPawn;
            isAnyPawnFinished = finishedPawn;

            _pawnManager.ResetAllIndicatorPawn();
            isDiceRolled = false;
            isPawnAlreadyMoved = true;
            _sfxManager.PlayPawnClip();

            // if (isAnyPawnKilled || isAnyPawnFinished)
            // {
            //     Debug.Log("Kill atau Finish → tidak next turn");
            //     return;
            // }

            if (isAnyPawnKilled)
            {
                ShowNotification($"{player.Name} killed a pawn!", "");
                return;
            }

            if (isAnyPawnFinished)
            {
                ShowNotification("", $"{player.Name} finished a pawn!");
                return;
            }

            if (diceNumberResult != 6)
            {
                NextTurn();
            }
        }

        private void NextTurn()
        {
            _game.NextTurn();
            _currentPlayerUI.text = $"{_game.currentPlayerTurn.Name}";
            // isPawnAlreadyMoved = false;
            isDiceRolled = false;
        }

        public void CheckGameWinningState()
        {
            int totalPlayers = _game.Players.Count;
            int playersFinishedAll = 0;

            foreach (var player in _game.Players)
            {
                if (_game.IsAllPawnsFinish(player))
                    playersFinishedAll++;
            }

            int requiredFinished = totalPlayers - 1;

            Debug.Log($"Players finished: {playersFinishedAll}/{requiredFinished}");

            if (playersFinishedAll >= requiredFinished)
            {
                Debug.Log("GAME END!");
                ShowWinningPanel();
            }
        }

        public void ShowWinningPanel()
        {
            var leaderboard = _game.PlayerScores.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            _uiGameplay.playerRank1NameUI.text = leaderboard.Count > 0 ? leaderboard[0].Name : "-";
            _uiGameplay.playerRank2NameUI.text = leaderboard.Count > 1 ? leaderboard[1].Name : "-";
            _uiGameplay.playerRank3NameUI.text = leaderboard.Count > 2 ? leaderboard[2].Name : "-";
            _uiGameplay.playerRank4NameUI.text = leaderboard.Count > 3 ? leaderboard[3].Name : "-";

            _winningPanel.alpha = 1f;
            _winningPanel.blocksRaycasts = true;
            _winningPanel.interactable = true;
        }

        public void TestMove1()
        {
            var player = _game.currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            var pawn = pawns[1];

            _game.MovePawn(player, pawn, 1, out _, out _);
        }

        public void TestMove2()
        {
            var player = _game.currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            var pawn = pawns[2];

            _game.MovePawn(player, pawn, 1, out _, out _);
        }

        public void TestMove3()
        {
            var player = _game.currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            var pawn = pawns[3];

            _game.MovePawn(player, pawn, 1, out _, out _);
        }

        public void TestMove4()
        {
            var player = _game.currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            var pawn = pawns[0];

            _game.MovePawn(player, pawn, 1, out _, out _);
        }

        public void UpdateTurnIndicator()
        {
            // Matikan semua indikator dulu
            _uiGameplay.player1Indicator.SetActive(false);
            _uiGameplay.player2Indicator.SetActive(false);
            _uiGameplay.player3Indicator.SetActive(false);
            _uiGameplay.player4Indicator.SetActive(false);

            var game = GameManager.Instance._game;
            var currentPlayer = game.currentPlayerTurn;
            int index = game.Players.IndexOf(currentPlayer);

            if (index == 0) _uiGameplay.player1Indicator.SetActive(true);
            if (index == 1) _uiGameplay.player2Indicator.SetActive(true);
            if (index == 2) _uiGameplay.player3Indicator.SetActive(true);
            if (index == 3) _uiGameplay.player4Indicator.SetActive(true);
        }

        private void ShowNotification(string killedMsg = "", string finishMsg = "")
        {
            _uiGameplay.notification.SetActive(true);

            _uiGameplay.pawnKilled.gameObject.SetActive(!string.IsNullOrEmpty(killedMsg));
            _uiGameplay.pawnFinish.gameObject.SetActive(!string.IsNullOrEmpty(finishMsg));

            _uiGameplay.pawnKilled.text = killedMsg;
            _uiGameplay.pawnFinish.text = finishMsg;

            // Opsional: auto hide setelah 2 detik
            StartCoroutine(HideNotifAfterDelay());
        }

        private IEnumerator HideNotifAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            _uiGameplay.notification.SetActive(false);
        }
    }
}


