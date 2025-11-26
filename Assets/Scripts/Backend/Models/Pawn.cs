using LudoGames.Types.Coordinates;
using LudoGames.Enums.PawnStates;
using LudoGames.Interface.Pawns;
using System;

namespace LudoGames.Models.Pawns
{
    class Pawn : IPawn
    {
        public Coordinate Coordinate { get; set; }
        public PawnStatesEnum PawnStatesEnum { get; set; }
        public int PositionIndex { get; set; }

        public Pawn(Coordinate coordinate, PawnStatesEnum pawnStates, int position = 0)
        {
            Coordinate = coordinate;
            PawnStatesEnum = pawnStates;
            PositionIndex = position;
        }

        public override string ToString()
        {
            return $"at {Coordinate} is {PawnStatesEnum}";
        }
    }
}