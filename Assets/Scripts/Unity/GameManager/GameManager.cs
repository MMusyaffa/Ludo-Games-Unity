using UnityEngine;
using TMPro;
using LudoGames.Enums.PawnStates;
using LudoGames.Interface.Players;
using LudoGames.Games.GameController;
using LudoGames.Unity.Menus;
using LudoGames.Unity.UI;

namespace LudoGames.Unity.GameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 
        internal GameController _game {get; private set; }
        [SerializeField] private UIGameplay _uiGameplay;
        [SerializeField] private SfxManager _sfxManager;
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private TextMeshProUGUI _diceNumberUI;
        [SerializeField] private UIDice _dice;
        public bool isPawnAlreadyMoved = false;
        public bool isDiceRolled = false;
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
        }

        public void StartRollDice()
        {
            StartCoroutine(_dice.RollDiceAnimation());
            _sfxManager.PlayDiceClip();
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
                    _game.MovePawn(player, pawn, 0);
                    _sfxManager.PlayPawnClip();
                    
                    Debug.Log("Pawn keluar dari home");

                    return;
                }
            }
            else
            {
                _game.MovePawn(player, pawn, diceNumberResult);
                isPawnAlreadyMoved = true;
                _sfxManager.PlayPawnClip();

                if (diceNumberResult != 6)
                {
                    NextTurn();
                    return;
                }
            }
        }

        private void NextTurn()
        {
            _game.NextTurn();
            _currentPlayerUI.text = $"{_game.currentPlayerTurn.Name}";
            // isPawnAlreadyMoved = false;
            isDiceRolled = false;
        }

        public void TestMove()
        {
            var player = _game.currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            var pawn = pawns[2];

            _game.MovePawn(player, pawn, 1);
        }
    }
}


