using LudoGames.Enums.Colors;

namespace LudoGames.Interface.Players
{
    interface IPlayer
    {
        string Name { get; }
        ColorsEnum ColorEnum { get; set; }
    }
}