using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
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
using LudoGames.Unity.GameManagers;
namespace LudoGames.Games.GameController
{
    class GameController
    {
        public List<IPlayer> Players { get; private set; }
        public IPlayer currentPlayerTurn {get; private set; }
        public Dictionary<IPlayer, int> PlayerScores { get; private set; }
        public Dictionary<IPlayer, List<IPawn>> PlayerPawns { get; private set; }
        public Dictionary<IPlayer, List<Coordinate>> PlayerPaths { get; private set; }
        public List<Coordinate> PathA { get; private set; }
        public List<Coordinate> PathB { get; private set; }
        public List<Coordinate> PathC { get; private set; }
        public List<Coordinate> PathD { get; private set; }
        private ITile[,] _tiles;
        private IDice _dice;
        private HashSet<Coordinate> _homeAndSafeZone;
        private readonly System.Random _random = new();
        public int diceNumber { get; private set; } = 0;
        public Action<IPlayer, IPawn> OnSpawnedPawn;
        public Action<IPawn> OnMovedPawn;
        public Action<IPawn> OnReturnPawn;

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

            InitializationHomeSafeZone();
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
            var playerColor = AssignColorForPlayer();
            
            if (Players.Contains(player)) return false;

            Players.Add(player);
            List<Coordinate> playerPath = PathPlayerOrder(player);
            var pawns = CreatePlayerPawn(playerPath);

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
            bool allPawnsFinish = IsAllPawnsFinish(player);
            bool isAnyPawnOnBoard = IsAnyPawnOnBoard(player);
            bool isAnyPawnOnFinalPath = IsAnyPawnOnFinalPath(player);
            bool isAnyPawnFinish = IsAnyPawnFinish(player);

            if (allPawnsHome)
            {
                if (diceResult != 6)
                {
                    Debug.Log($"Player {player.Name} tidak bisa jalan, skip");
                    return false;
                }
                else return true;
            }

            if (allPawnsFinish)
            {
                Debug.Log($"Player {player.Name} tidak bisa jalan karena semua pawn sudah finish");
                return false;
            }

            // if (isAnyPawnOnFinalPath)
            // {
            //     bool anyPawnCanMove = CanAnyPawnOnFinalPathMove(player, diceResult);

            //     if (!anyPawnCanMove)
            //     {
            //         Debug.Log($"Player {player.Name} tidak bisa jalan karena dadu {diceResult} lebih dari sisa langkah final path");
            //         return false;
            //     }

            //     // kalau ada pawn final yang bisa bergerak → boleh jalan
            //     return true;
            // }

            bool anyPawnCanMove = CanPlayerMoveAnyPawn(player, diceResult);

            if (!anyPawnCanMove)
            {
                Debug.Log($"Player {player.Name} skip (tidak ada pawn yang bisa bergerak)");
                return false;
            }

            // if (isAnyPawnFinish && !isAnyPawnOnBoard)
            // {
            //     if (diceResult != 6)
            //     {
            //         Debug.Log($"Player {player.Name} tidak bisa jalan karena ada pawn finish dan masih di home, skip");
            //         return false;
            //     }
                
            //     else return true;
            // }

            

            // foreach (var pawn in PlayerPawns[player])
            // {
            //     if (pawn.PawnStatesEnum == PawnStatesEnum.AtHome && diceNumber == 6)
            //     {
            //         continue;
            //     }

            //     if (IsPawnOnFinalPath(player, pawn))
            //     {
            //         return true;
            //     }

            //     int lastPath = PlayerPaths[player].Count - 1 - pawn.PositionIndex;
            //     if (diceResult > lastPath)
            //     {
            //         return false;
            //         // break;
            //     }

            //     // if (IsPawnOnFinalPath(player, pawn))
            //     // {
            //     //     int lastPath = PlayerPaths[player].Count - 1 - pawn.PositionIndex;
            //     //     if (diceResult > lastPath)
            //     //     {
            //     //         Debug.Log($"Player {player.Name} tidak bisa jalan, dadu {diceResult} lebih dari sisa langkah {lastPath}");
            //     //         return false;
            //     //     }
            //     // }
            // }

            return true;
        }

        private bool CanPlayerMoveAnyPawn(IPlayer player, int diceResult)
        {
            int pathLeft = -1;
            foreach (var pawn in PlayerPawns[player])
            {
                switch (pawn.PawnStatesEnum)
                {
                    case PawnStatesEnum.AtHome:
                        if (diceResult == 6)
                            return true;
                        break;
                    case PawnStatesEnum.OnBoard:
                        return true;
                    case PawnStatesEnum.OnFinishPath:
                        int remainingPath = PlayerPaths[player].Count - 1 - pawn.PositionIndex;
                        pathLeft = remainingPath;
                        if (diceResult <= remainingPath)
                            return true;
                        break;
                }

                Debug.Log($"Player {player.Name}, pawn - {pawn}, {pathLeft}");
            }

            return false;
        }


        public bool MovePawn(IPlayer player, IPawn pawn, int rollDice,  out bool killedPawn, out bool finishedPawn)
        {
            killedPawn = false;
            finishedPawn = false;

            var path = PlayerPaths[player];
            int newIndex = pawn.PositionIndex + rollDice;
            bool pawnOnFinalPath = IsPawnOnFinalPath(player, pawn);
            int lastPath = path.Count - 1 - pawn.PositionIndex;

            Coordinate currentCoordinate = pawn.Coordinate;

            if (pawnOnFinalPath)
            {
                pawn.PawnStatesEnum = PawnStatesEnum.OnFinishPath;
                if (rollDice > lastPath) 
                { 
                    Debug.Log($"Dadu tidak boleh lebih dari {lastPath}"); 
                    return false;
                }
            }
            
            Coordinate newCoordinate = path[newIndex];
            UpdatePawnPosition(pawn, newCoordinate, newIndex);
            
            var knockPawn = IsPawnCollide(player, pawn);
            if (knockPawn != null) 
            {
                Debug.Log("Collide");
                killedPawn = true;
            }

            if (newIndex == path.Count - 1)
            {
                pawn.PawnStatesEnum = PawnStatesEnum.Finished;
                PlayerScores[player] += 1;
                PlayerScoreLeaderboard();
                GameManager.Instance.CheckGameWinningState();
                finishedPawn = true;

                Debug.Log($"Pawn mencapai akhir untuk player {player.Name}, score {PlayerScores[player]}");
                return true;
            }

            Debug.Log($"Bidak maju sebanyak: {rollDice}, Dari block {currentCoordinate} ke block {newCoordinate}");
            return true;
        }

        public void NextTurn()
        {
            int currentPlayerIndex = Players.IndexOf(currentPlayerTurn!);
            currentPlayerTurn = Players[(currentPlayerIndex + 1) % Players.Count];

            Debug.Log($"\nNext turn, Giliran pemain {currentPlayerTurn.Name} untuk jalan");
        }

        public IPawn IsPawnCollide(IPlayer movedPlayer, IPawn movedPawn)
        {
            if (_homeAndSafeZone.Contains(movedPawn.Coordinate)) 
            {
                Debug.Log("Collide on Home or Safe Zone");
                return null;
            }

            foreach (var player in Players)
            {
                if (player == movedPlayer) { continue; }

                foreach (var pawn in PlayerPawns[player])
                {
                    if (pawn.Coordinate.Equals(movedPawn.Coordinate))
                    {
                        Debug.Log($"{player.Name}, {pawn.Coordinate} and {movedPlayer.Name}, {movedPawn.Coordinate}");
                        KnockPawn(player, pawn);
                        OnReturnPawn?.Invoke(pawn);
                        
                        return pawn;
                    }
                }
            }

            return null;
        }

        private ColorsEnum AssignColorForPlayer()
        {
            int playerIndex = Players.Count; 
            
            return playerIndex switch
            {
                0 => ColorsEnum.Blue,
                1 => ColorsEnum.Green,
                2 => ColorsEnum.Red,
                3 => ColorsEnum.Yellow,
                _ => ColorsEnum.Blue,
            };
        }

        private List<Coordinate> PathPlayerOrder(IPlayer player)
        {
            int playerIndex = Players.IndexOf(player); 
            
            return playerIndex switch
            {
                0 => PathD,
                1 => PathB,
                2 => PathA,
                3 => PathC,
                _ => PathD,
            };
        }
        
        private List<IPawn> CreatePlayerPawn(List<Coordinate> coordinate)
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

        private void InitializationHomeSafeZone()
        {
            _homeAndSafeZone.Add(new Coordinate(6,13));
            _homeAndSafeZone.Add(new Coordinate(2,8));
            _homeAndSafeZone.Add(new Coordinate(1,6));
            _homeAndSafeZone.Add(new Coordinate(6,2));
            _homeAndSafeZone.Add(new Coordinate(8,1));
            _homeAndSafeZone.Add(new Coordinate(12,6));
            _homeAndSafeZone.Add(new Coordinate(13,8));
            _homeAndSafeZone.Add(new Coordinate(8,12));
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

        private bool IsAnyPawnOnBoard(IPlayer player)
        {
            return PlayerPawns[player].Any(p => p.PawnStatesEnum == PawnStatesEnum.OnBoard);
        }

        private bool IsAnyPawnOnFinalPath(IPlayer player)
        {
            return PlayerPawns[player].Any(p => p.PawnStatesEnum == PawnStatesEnum.OnFinishPath);
        }

        private bool IsAnyPawnFinish(IPlayer player)
        {
            return PlayerPawns[player].Any(p => p.PawnStatesEnum == PawnStatesEnum.Finished);
        }

        private bool IsAllPawnsFinish(IPlayer player)
        {
            return PlayerPawns[player].All(p => p.PawnStatesEnum == PawnStatesEnum.Finished);
        }

        public bool IsAllPlayerFinished(IPlayer player)
        {
            var pawns = PlayerPawns[player];
            return pawns.All(p => p.PawnStatesEnum == PawnStatesEnum.Finished);
        }

        private void PlayerScoreLeaderboard()
        {
            Debug.Log("Scoreboard");

            var leaderboard = PlayerScores.OrderByDescending(x => x.Value);
            int rank = 1;

            foreach (var rankInfo in leaderboard)
            {
                Debug.Log($"#{rank}, {rankInfo.Key.Name} - {rankInfo.Value}");
                rank++;
            }
        }

        public IPlayer GetPlayerPawnOwner(IPawn pawn)
        {
            foreach (var player in PlayerPawns)
            {
                var owner = player.Key;
                var pawnList = player.Value;

                if (pawnList.Contains(pawn))
                {
                    return owner;
                }
            }

            return null;
        }

    }
}