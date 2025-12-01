using UnityEngine;
using TMPro;

namespace LudoGames.Unity.UI
{
    public class UIGameplay : MonoBehaviour
    {
        public TextMeshProUGUI player1NameUI;
        public TextMeshProUGUI player2NameUI;
        public TextMeshProUGUI player3NameUI;
        public TextMeshProUGUI player4NameUI;

        public TextMeshProUGUI playerRank1NameUI;
        public TextMeshProUGUI playerRank2NameUI;
        public TextMeshProUGUI playerRank3NameUI;
        public TextMeshProUGUI playerRank4NameUI;

        void Start()
        {
            player1NameUI.text = "Player 1 Name";
            player2NameUI.text = "Player 2 Name";
            player3NameUI.text = "Player 3 Name";
            player4NameUI.text = "Player 4 Name";
        }
    }
}
