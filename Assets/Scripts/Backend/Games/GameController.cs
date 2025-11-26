using LudoGames.Types.Coordinates;
using LudoGames.Enums.Colors;
using LudoGames.Enums.PawnStates;
using LudoGames.Interface.Boards;
using LudoGames.Interface.Dices;
using LudoGames.Interface.Pawns;
using LudoGames.Interface.Players;
using LudoGames.Interface.Tiles;
using LudoGames.Models.Boards;
using LudoGames.Models.Dices;
using LudoGames.Models.Player;
using LudoGames.Models.Pawns;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;


namespace LudoGames.Games.GameController
{
    public delegate void PathAdd(Coordinate coordinate);

    class GameController
    {
        public List<IPlayer> Players { get; private set; }
        public Dictionary<IPlayer, int> PlayerScores { get; private set; }
        public Dictionary<IPlayer, List<IPawn>> PlayerPawns { get; private set; }
        public Dictionary<IPlayer, List<Coordinate>> PlayerPaths { get; private set; }
        public List<Coordinate> PathA { get; private set; }
        public List<Coordinate> PathB { get; private set; }
        public List<Coordinate> PathC { get; private set; }
        public List<Coordinate> PathD { get; private set; }
        public IPlayer currentPlayerTurn {get; private set; }
        private ITile[,] _tiles;
        private IDice _dice;
        public int diceNumber { get; private set; } = 0;
        private HashSet<Coordinate> _homeAndSafeZone;
        private readonly System.Random _random = new();
        // public static PathAdd makePath;
        public Action<IPlayer, IPawn> OnSpawnedPawn;
        public Action<IPawn> OnMovedPawn;

        public GameController()
        {
            Players = new List<IPlayer>();
            PlayerPawns = new Dictionary<IPlayer, List<IPawn>>();
            PlayerScores = new Dictionary<IPlayer, int>();
            PlayerPaths = new Dictionary<IPlayer, List<Coordinate>>();

            IBoard board = new Board(15,15);
            _tiles = board.Tiles;
            _dice = new Dice(6);
            _homeAndSafeZone = new HashSet<Coordinate>();

            Debug.Log($"Board size: {_tiles.GetLength(0)} x {_tiles.GetLength(1)}\n");

            PathA = MakePathA();
            PathB = MakePathB();
            PathC = MakePathC();
            PathD = MakePathD();
        }

        private List<Coordinate> MakePathA()
        {
            List<Coordinate> pathA = new List<Coordinate>();

            for (int x = 1; x <= 5; x++) { pathA.Add(_tiles[x, 6].Coordinate); }
            for (int y = 5; y >= 0; y--) { pathA.Add(_tiles[6, y].Coordinate); }
            pathA.Add(_tiles[7,0].Coordinate);

            for (int y = 0; y <= 5; y++) { pathA.Add(_tiles[8, y].Coordinate); }
            for (int x = 9; x <= 14; x++) { pathA.Add(_tiles[x, 6].Coordinate); }
            pathA.Add(_tiles[14,7].Coordinate);

            for (int x = 14; x >=9; x--) { pathA.Add(_tiles[x, 8].Coordinate); }
            for (int y = 9; y <= 14; y++) { pathA.Add(_tiles[8, y].Coordinate); }
            pathA.Add(_tiles[7,14].Coordinate);

            for (int y = 14; y >=9; y--) { pathA.Add(_tiles[6, y].Coordinate); }
            for (int x = 5; x >=0; x--) { pathA.Add(_tiles[x, 8].Coordinate); }
            for (int x = 0; x <= 6; x++) { pathA.Add(_tiles[x, 7].Coordinate); }

            Debug.Log("Path A Complete");
            return pathA;
        }

        private List<Coordinate> MakePathB()
        {
            List<Coordinate> pathB = new List<Coordinate>();

            for (int y = 1; y <= 5; y++) { pathB.Add(_tiles[8, y].Coordinate); }
            for (int x = 9; x <= 14; x++) { pathB.Add(_tiles[x, 6].Coordinate); }
            pathB.Add(_tiles[14,7].Coordinate);

            for (int x = 14; x >=9; x--) { pathB.Add(_tiles[x, 8].Coordinate); }
            for (int y = 9; y <= 14; y++) { pathB.Add(_tiles[8, y].Coordinate); }
            pathB.Add(_tiles[7,14].Coordinate);

            for (int y = 14; y >=9; y--) { pathB.Add(_tiles[6, y].Coordinate); }
            for (int x = 5; x >=0; x--) { pathB.Add(_tiles[x, 8].Coordinate); }
            pathB.Add(_tiles[0,7].Coordinate);

            for (int x = 0; x <= 5; x++) { pathB.Add(_tiles[x, 6].Coordinate); }
            for (int y = 5; y >= 0; y--) { pathB.Add(_tiles[6, y].Coordinate); }
            for (int y = 0; y <= 6; y++) { pathB.Add(_tiles[7, y].Coordinate); }

            Debug.Log("Path B Complete");
            return pathB;
        }

        private List<Coordinate> MakePathC()
        {
            List<Coordinate> pathC = new List<Coordinate>();

            for (int x = 13; x >=9; x--) { pathC.Add(_tiles[x, 8].Coordinate); }
            for (int y = 9; y <= 14; y++) { pathC.Add(_tiles[8, y].Coordinate); }
            pathC.Add(_tiles[7,14].Coordinate);

            for (int y = 14; y >=9; y--) { pathC.Add(_tiles[6, y].Coordinate); }
            for (int x = 5; x >=0; x--) { pathC.Add(_tiles[x, 8].Coordinate); }
            pathC.Add(_tiles[0,7].Coordinate);

            for (int x = 0; x <= 5; x++) { pathC.Add(_tiles[x, 6].Coordinate); }
            for (int y = 5; y >= 0; y--) { pathC.Add(_tiles[6, y].Coordinate); }
            pathC.Add(_tiles[7,0].Coordinate);

            for (int y = 0; y <= 5; y++) { pathC.Add(_tiles[8, y].Coordinate); }
            for (int x = 9; x <= 14; x++) { pathC.Add(_tiles[x, 6].Coordinate); }
            for (int x = 14; x >= 8; x--) { pathC.Add(_tiles[x, 7].Coordinate); }

            Debug.Log("Path C Complete");
            return pathC;
        }

        private List<Coordinate> MakePathD()
        {
            List<Coordinate> pathD = new List<Coordinate>();

            for (int y = 13; y >=9; y--) { pathD.Add(_tiles[6, y].Coordinate); }
            for (int x = 5; x >=0; x--) { pathD.Add(_tiles[x, 8].Coordinate); }
            pathD.Add(_tiles[0,7].Coordinate);

            for (int x = 0; x <= 5; x++) { pathD.Add(_tiles[x, 6].Coordinate); }
            for (int y = 5; y >= 0; y--) { pathD.Add(_tiles[6, y].Coordinate); }
            pathD.Add(_tiles[7,0].Coordinate);

            for (int y = 0; y <= 5; y++) { pathD.Add(_tiles[8, y].Coordinate); }
            for (int x = 9; x <= 14; x++) { pathD.Add(_tiles[x, 6].Coordinate); }
            pathD.Add(_tiles[14,7].Coordinate);

            for (int x = 14; x >=9; x--) { pathD.Add(_tiles[x, 8].Coordinate); }
            for (int y = 9; y <= 14; y++) { pathD.Add(_tiles[8, y].Coordinate); }
            for (int y = 14; y >= 8; y--) { pathD.Add(_tiles[7, y].Coordinate); }

            Debug.Log("Path D Complete");
            return pathD;
        }

        public bool AddNewPlayer(string name)
        {
            IPlayer player = new Player(name);
            var playerColor = AssignColorForPlayer(player);
            
            if (Players.Contains(player)) return false;

            Players.Add(player);
            List<Coordinate> playerPath = PathPlayerOrder(player);
            var pawns = CreatePlayerPawn(player, playerPath);

            player.ColorEnum = playerColor;
            PlayerScores[player] = 0;
            PlayerPawns[player] = pawns;
            PlayerPaths[player] = playerPath;

            foreach (var pawn in pawns)
            {
                OnSpawnedPawn?.Invoke(player, pawn);
            }

            if (currentPlayerTurn == null) { currentPlayerTurn = player; }

            return true;
        }

        public void AssignFirstPlayerTurn()
        {
            if (Players.Count == 0) { Debug.Log("Belum ada pemain");}

            int playerTurnOrder = _random.Next(Players.Count);
            currentPlayerTurn = Players[playerTurnOrder];

            Debug.Log($"Giliran pemain {currentPlayerTurn.Name} untuk jalan");
        }   

        public int RollDice()
        {
            diceNumber = _random.Next(1, _dice.Sides + 1);
            // diceNumber = 6;

            return diceNumber;
        }

        public IPawn SelectPawn(IPlayer player, int diceResult, int pawnIndex)
        {
            var pawns = PlayerPawns[player];
            var pawn = pawns[pawnIndex];
            
            if (pawn.PawnStatesEnum == PawnStatesEnum.AtHome && diceResult != 6)
            {
                Debug.Log("Tidak bisa keluar home.");
            }
            else if (pawn.PawnStatesEnum == PawnStatesEnum.Finished)
            {
                Debug.Log("Sudah finish, pilih pawn lain.");
            }

            return pawns[pawnIndex];
        }

        public bool CanPlayerMovePawn(IPlayer player, int diceResult)
        {
            bool allPawnsHome = IsAllPawnsAtHome(player);
            bool isIsAnyPawnOnBoard = IsAnyPawnOnBoard(player);
            bool isAnyPawnFinsih = IsAnyPawnFinish(player);
            
            if (allPawnsHome)
            {
                if (diceResult != 6)
                {
                    Debug.Log($"Player {player.Name} tidak bisa jalan, skip");
                    return false;
                }
                else return true;
            }

            if (isAnyPawnFinsih && !isIsAnyPawnOnBoard)
            {
                if (diceResult != 6)
                {
                    Debug.Log($"Player {player.Name} tidak bisa jalan karena ada pawn finish dan masih di home, skip");
                    return false;
                }
                else return true;
            }

            return true;
        }

        public bool MovePawn(IPlayer player, IPawn pawn, int rollDice)
        {
            var path = PlayerPaths[player];
            int newIndex = pawn.PositionIndex + rollDice;
            bool pawnOnFinalPath = IsPawnOnFinalPath(player, pawn);

            Coordinate currentCoordinate = pawn.Coordinate;

            if (pawnOnFinalPath)
            {
                int lastpath = path.Count - 1 - pawn.PositionIndex;

                if (rollDice > lastpath) 
                { 
                    Debug.Log($"Dadu tidak boleh lebih dari {lastpath}"); 
                    return false;
                }
            }
            
            Coordinate newCoordinate = path[newIndex];
            UpdatePawnPosition(pawn, newCoordinate, newIndex);
            
            if (IsPawnCollide(player, pawn)) { Console.Write(" Collide"); }

            int lastPath = path.Count - 1 - pawn.PositionIndex;
            if (lastPath == 0)
            {
                pawn.PawnStatesEnum = PawnStatesEnum.Finished;
                Debug.Log($"Pawn mencapai akhir untuk player {player.Name}!");
                return false;
            }

            Debug.Log($"Bidak maju sebanyak: {rollDice}, Dari block {currentCoordinate} ke block {newCoordinate}");
            return true;
        }

        public void NextTurn()
        {
            int currentPlayerIndex = Players.IndexOf(currentPlayerTurn!);
            currentPlayerTurn = Players[(currentPlayerIndex + 1) % Players.Count];

            Debug.Log($"\nGiliran pemain {currentPlayerTurn.Name} untuk jalan");
        }

        private ColorsEnum AssignColorForPlayer(IPlayer player)
        {
            int playerIndex = Players.Count; 
            
            return playerIndex switch
            {
                0 => ColorsEnum.Blue,
                1 => ColorsEnum.Green,
                2 => ColorsEnum.Red,
                3 => ColorsEnum.Yellow,
                _ => throw new NotImplementedException(),
            };
        }

        private List<Coordinate> PathPlayerOrder(IPlayer player)
        {
            int playerIndex = Players.IndexOf(player); 
            
            return playerIndex switch
            {
                0 => PathD,
                1 => PathA,
                2 => PathB,
                3 => PathC,
                _ => throw new NotImplementedException(),
            };
        }
        
        private List<IPawn> CreatePlayerPawn(IPlayer player, List<Coordinate> coordinate)
        {
            var pawns = new List<IPawn>();
            
            for (int i = 0; i < 4; i++)
            {
                var newPawn = new Pawn ((coordinate[0]), PawnStatesEnum.AtHome, 0);
                pawns.Add(newPawn);
                // OnSpawnedPawn?.Invoke(player, newPawn);
            }

            return pawns;
        }
        
        private void UpdatePawnPosition(IPawn pawn, Coordinate coordinate, int index)
        {
            pawn.Coordinate = coordinate;
            pawn.PositionIndex = index;

            OnMovedPawn?.Invoke(pawn);
        }
        
        private void KnockPawn(IPlayer player, IPawn pawn)
        {
            var path = PlayerPaths[player];

            pawn.Coordinate = path[0];
            pawn.PositionIndex = 0;
            pawn.PawnStatesEnum = PawnStatesEnum.AtHome;

            Debug.Log($"Pawn milik {player.Name} knock, pulang ke base");
        }

        private bool IsAllPawnsAtHome(IPlayer player)
        {
            return PlayerPawns[player].All(p => p.PawnStatesEnum == PawnStatesEnum.AtHome);
        }

        private bool IsPawnOnFinalPath(IPlayer player, IPawn pawn)
        {
            var path = PlayerPaths[player];
            int lastSixPath = path.Count - 6;

            if (pawn.PositionIndex >= lastSixPath && pawn.PositionIndex < path.Count)
            {
                Debug.Log($"Player {player.Name}, pawn kamu masuk ke final path");
                pawn.PawnStatesEnum = PawnStatesEnum.OnFinishPath;
                return true;
            }
            return false;
        }

        private bool IsAnyPawnFinish(IPlayer player)
        {
            return PlayerPawns[player].Any(p => p.PawnStatesEnum == PawnStatesEnum.Finished);
        }

        private bool IsAnyPawnOnBoard(IPlayer player)
        {
            return PlayerPawns[player].Any(p => p.PawnStatesEnum == PawnStatesEnum.OnBoard);
        }
    
        private bool IsPawnCollide(IPlayer movedPlayer, IPawn movedPawn)
        {
            if (_homeAndSafeZone.Contains(movedPawn.Coordinate)) { return false; }

            foreach (var player in Players)
            {
                if (player == movedPlayer) { continue; }

                foreach (var pawn in PlayerPawns[player])
                {
                    if (pawn.Coordinate.Equals(movedPawn.Coordinate))
                    {
                        KnockPawn(player, pawn);
                        return true;
                    }
                }
            }

            return false;
        }

        // public IPlayer GetCurrentPlayerTurn()
        // {
        //     return currentPlayerTurn!;
        // }

        // public IReadOnlyDictionary<IPlayer, List<IPawn>> GetPlayerPawns()
        // {
        //     return PlayerPawns;
        // }
    }
}