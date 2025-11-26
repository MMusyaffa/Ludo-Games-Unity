using UnityEngine;
using LudoGames.Enums.PawnStates;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Games.GameController;
using LudoGames.Models.Player;
using TMPro;
using LudoGames.Models.Pawns;

namespace LudoGames.Unity.GameManager
{
    public class GameManager : MonoBehaviour
    {
        private GameController _game;
        public TMP_InputField playerNameInput;

        void Start()
        {
            _game = new GameController();
            Debug.Log("Game Started");
        }

        public void AddPlayerButton()
        {
            string name = playerNameInput.text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Name must filled");
                return;
            }

            if (_game.Players.Count < 4)
            {
                _game.AddNewPlayer(name);
                Debug.Log($"New Player {name} Added");
                playerNameInput.text = "";
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
        }

        public void RollDice()
        {
            IPlayer currentPlayer = _game._currentPlayerTurn;
            int num = _game.RollDice();

            if(!_game.CanPawnMove(currentPlayer, num))
            {
                _game.NextTurn();
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
            var player = _game._currentPlayerTurn;
            var pawns = _game.PlayerPawns[player];
            int diceNumber = 6;
            int pawnIndex = 2;

            var pawn = pawns[pawnIndex];

            _game.MovePawn(player, pawn, diceNumber);
        }
    }
}


