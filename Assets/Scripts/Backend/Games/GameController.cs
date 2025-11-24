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


namespace LudoGames.Games.GameController
{
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
        public IPlayer _currentPlayerTurn {get; private set; }
        private ITile[,] _tiles;
        private IDice _dice;
        private int _diceNumber = 0;
        private HashSet<Coordinate> _homeAndSafeZone;
        private readonly Random _random = new();

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

            Console.WriteLine($"Board size: {_tiles.GetLength(0)} x {_tiles.GetLength(1)}\n");

            PathA = MakeAPath();
            PathB = MakeBPath();
            PathC = MakeCPath();
            PathD = MakeDPath();
        }

        private List<Coordinate> MakeAPath()
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

            Console.WriteLine("Path A Complete");
            return pathA;
        }

        private List<Coordinate> MakeBPath()
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

            Console.WriteLine("Path B Complete");
            return pathB;
        }

        private List<Coordinate> MakeCPath()
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

            Console.WriteLine("Path C Complete");
            return pathC;
        }

        private List<Coordinate> MakeDPath()
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

            Console.WriteLine("Path D Complete");
            return pathD;
        }

        public bool AddNewPlayer()
        {
            string name = AddPlayerName();
            IPlayer player = new Player(name);
            
            ColorsEnum color = RequestColorFromPlayer(player);
            player.ColorEnum = color;
            
            if (Players.Contains(player)) return false;

            List<Coordinate> playerPath = PathPlayerOrder();
            var pawns = CreatePlayerPawn(playerPath);

            Players.Add(player);
            PlayerScores[player] = 0;
            PlayerPawns[player] = pawns;
            PlayerPaths[player] = playerPath;

            if (_currentPlayerTurn == null) { _currentPlayerTurn = player; }

            return true;
        }

        public void AssignFirstPlayerTurn()
        {
            if (Players.Count == 0) { Console.WriteLine("Belum ada pemain");}

            int playerTurnOrder = _random.Next(Players.Count);
            _currentPlayerTurn = Players[playerTurnOrder];

            Console.WriteLine($"Giliran pemain {_currentPlayerTurn.Name} untuk jalan");
        }

        public int RollDice()
        {
            while(true)
            {
                Console.Write("Lempar dadu? (y): ");
                string input = Console.ReadLine()!;

                if (input?.ToLower() != "y") 
                { 
                    Console.WriteLine("Masukan y");
                    continue;
                }

                _diceNumber = _random.Next(1, _dice.Sides + 1);
                // _diceNumber = 6;

                Console.Write($"Dice Roll: {_diceNumber}\n");
                return _diceNumber;
            }
        }

        public IPawn SelectPawn(IPlayer player, int diceResult)
        {
            var pawns = PlayerPawns[player];

            Console.WriteLine($"{player.Name} - Pilih pawn untuk dijalankan: ");
            for (int i = 0; i < pawns.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Pawn {i + 1} - Posisi: {pawns[i].Coordinate}");
            }

            while(true)
            {
                Console.Write("Masukan nomor pawn: ");

                if (int.TryParse(Console.ReadLine(), out int selectedPawn))
                {
                    selectedPawn -= 1;
                    if (selectedPawn >= 0 && selectedPawn < pawns.Count)
                    {
                        var pawn = pawns[selectedPawn];

                        if(pawn.PawnStatesEnum == PawnStatesEnum.AtHome && diceResult != 6)
                        {
                            Console.WriteLine("Tidak bisa keluar home.");
                            continue;
                        }
                        else if(pawn.PawnStatesEnum == PawnStatesEnum.Finished)
                        {
                            Console.WriteLine("Sudah finish, pilih pawn lain.");
                            continue;
                        }

                        return pawns[selectedPawn];
                    }
                }
                Console.WriteLine("Pilihan tidak valid! Coba lagi.");
            }
        }

        public bool CanPawnMove(IPlayer player, int diceResult)
        {
            bool allPawnsHome = IsAllPawnsAtHome(player);
            bool isIsAnyPawnOnBoard = IsAnyPawnOnBoard(player);
            bool isAnyPawnFinsih = IsAnyPawnFinish(player);
            
            if (allPawnsHome)
            {
                if (diceResult != 6)
                {
                    Console.WriteLine($"Player {player.Name} tidak bisa jalan, skip");
                    return false;
                }
                else return true;
            }

            if (isAnyPawnFinsih && !isIsAnyPawnOnBoard)
            {
                if (diceResult != 6)
                {
                    Console.WriteLine($"Player {player.Name} tidak bisa jalan karena ada pawn finish dan masih di home, skip");
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
                    Console.WriteLine($"Dadu tidak boleh lebih dari {lastpath}"); 
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
                Console.WriteLine($"Pawn mencapai akhir untuk player {player.Name}!");
                return false;
            }

            Console.WriteLine($"Bidak maju sebanyak: {rollDice}, Dari block {currentCoordinate} ke block {newCoordinate}");
            return true;
        }

        public void NextTurn()
        {
            int currentPlayerIndex = Players.IndexOf(_currentPlayerTurn!);
            _currentPlayerTurn = Players[(currentPlayerIndex + 1) % Players.Count];

            Console.WriteLine($"\nGiliran pemain {_currentPlayerTurn.Name} untuk jalan");
        }

        private string AddPlayerName()
        {
            Console.Write("Masukkan nama player: ");
            return Console.ReadLine()!;
        }

        private ColorsEnum RequestColorFromPlayer(IPlayer player)
        {
            while (true)
            {
                Console.WriteLine("Pilihan warna");
                Console.WriteLine("1. Red");
                Console.WriteLine("2. Blue");
                Console.WriteLine("3. Green");
                Console.WriteLine("4. Yellow");
                Console.Write($"Pilih warna untuk player {player.Name}: ");

                if (int.TryParse(Console.ReadLine(), out int selected))
                {
                    selected -= 1;
                    if (Enum.IsDefined(typeof(ColorsEnum), selected))
                        {
                            var chosenColor = (ColorsEnum)selected;
                            if (!IsColorTaken(chosenColor)) 
                            {
                                player.ColorEnum = chosenColor;
                                Console.WriteLine($"player: {player.Name}, color: {player.ColorEnum}");

                                return chosenColor; 
                            } else { Console.WriteLine("Warna sudah dipilih pemain lain! Pilih warna lain."); }
                        } else { Console.WriteLine("Input tidak valid! Pilih angka 1-4."); }
                } else { Console.WriteLine("Input tidak valid! Harus angka!"); }
            }
        }

        private bool IsColorTaken(ColorsEnum color)
        {
            return Players.Any(p => p.ColorEnum == color);
        }

        private List<Coordinate> PathPlayerOrder()
        {
            int playerIndex = Players.Count; 
            
            return playerIndex switch
            {
                0 => PathD,
                1 => PathB,
                2 => PathA,
                3 => PathC,
                _ => throw new NotImplementedException(),
            };
        }
        
        private List<IPawn> CreatePlayerPawn(List<Coordinate> coordinate)
        {
            var pawns = new List<IPawn>();

            for (int i = 0; i < 4; i++)
            {
                pawns.Add(new Pawn((coordinate[0]), PawnStatesEnum.AtHome, 0));
            }

            return pawns;
        }
        
        private void UpdatePawnPosition(IPawn pawn, Coordinate coordinate, int index)
        {
            pawn.Coordinate = coordinate;
            pawn.PositionIndex = index;
        }
        
        private void KnockPawn(IPlayer player, IPawn pawn)
        {
            var path = PlayerPaths[player];

            pawn.Coordinate = path[0];
            pawn.PositionIndex = 0;
            pawn.PawnStatesEnum = PawnStatesEnum.AtHome;

            Console.WriteLine($"Pawn milik {player.Name} knock, pulang ke base");
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
                Console.WriteLine($"Player {player.Name}, pawn kamu masuk ke final path");
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
        //     return _currentPlayerTurn!;
        // }
    }
}