using System;
using LudoGames.Enums.Colors;
using LudoGames.Interface.Players;

namespace LudoGames.Models.Player
{
    class Player : IPlayer
    {
        public string Name { get; }
        public ColorsEnum ColorEnum { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Player other) { return false; }

            return Name == other.Name && ColorEnum == other.ColorEnum;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ColorEnum);
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}