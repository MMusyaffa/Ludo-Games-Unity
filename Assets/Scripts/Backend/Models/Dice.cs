using LudoGames.Interface.Dices;

namespace LudoGames.Models.Dices
{
    class Dice : IDice
    {
        public int Sides { get; set; }

        public Dice(int side)
        {
            Sides = side;
        }
    }
}