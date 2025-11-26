using UnityEngine;
using LudoGames.Interface.Players;
using LudoGames.Games.GameController;
using TMPro;
using LudoGames.Unity.Pawns;

namespace LudoGames.Unity.GameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 
        internal GameController _game {get; private set; }
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private TextMeshProUGUI _diceNumberUI;
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
            DontDestroyOnLoad(gameObject);

            _game = new GameController();
            diceNumberResult = _game.diceNumber;
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
                Debug.Log($"New Player {name} Added, Path {_game.PlayerPaths}");
                _playerNameInput.text = "";
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

            _player1Name.text = _game.Players.Count > 0 ? _game.Players[0].Name : "";
            _player2Name.text = _game.Players.Count > 1 ? _game.Players[1].Name : "";
            _player3Name.text = _game.Players.Count > 2 ? _game.Players[2].Name : "";
            _player4Name.text = _game.Players.Count > 3 ? _game.Players[3].Name : "";
        }

        public void RollDice()
        {
            IPlayer currentPlayer = _game.currentPlayerTurn;

            diceNumberResult = _game.RollDice();;
            _diceNumberUI.text = $"{diceNumberResult}";
            Debug.Log($"Dice Rolled: {diceNumberResult}");

            if(!_game.CanPlayerMovePawn(currentPlayer, diceNumberResult))
            {
                _game.NextTurn();
                _currentPlayerUI.text = $"{_game.currentPlayerTurn.Name}";
                return;
            }

            // IPawn pawn = _game.SelectPawn(currentPlayer, num);

            // if (pawn.PawnStatesEnum == PawnStatesEnum.AtHome)
            // {
            //     if (num == 6)
            //     {
            //         Debug.Log("Pawn keluar dari home");
            //         pawn.PawnStatesEnum = PawnStatesEnum.OnBoard;
            //     }
            //     else
            //     {
            //         _game.NextTurn();
            //     }
            // }
            // else
            // {
            //     _game.MovePawn(currentPlayer, pawn, num);

            //     if(num != 6) 
            //     { 
            //         _game.NextTurn(); 
            //     }
            // }
        }

        public void TestMove()
        {
            var player = _game.currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            int pawnIndex = 2;

            var pawn = pawns[pawnIndex];
            // pawn.PositionIndex = 1;
            // GameController.OnMovedPawn?.Invoke(pawn);

            _game.MovePawn(player, pawn, 1);
            // _game.MovePawn(player, pawn, diceNumberResult);
        }
    }
}


