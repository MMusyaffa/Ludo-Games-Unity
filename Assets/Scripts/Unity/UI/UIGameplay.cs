using UnityEngine;
using TMPro;
using LudoGames.Interface.Players;

namespace LudoGames.Unity.UI
{
    public class UIGameplay : MonoBehaviour
    {
        public TextMeshProUGUI player1NameUI;
        public TextMeshProUGUI player2NameUI;
        public TextMeshProUGUI player3NameUI;
        public TextMeshProUGUI player4NameUI;
        public GameObject player1Indicator;
        public GameObject player2Indicator;
        public GameObject player3Indicator;
        public GameObject player4Indicator;
        public TextMeshProUGUI playerRank1NameUI;
        public TextMeshProUGUI playerRank2NameUI;
        public TextMeshProUGUI playerRank3NameUI;
        public TextMeshProUGUI playerRank4NameUI;
        public GameObject notification;
        public TextMeshProUGUI pawnKilled;
        public TextMeshProUGUI pawnFinish;

        void Start()
        {
            player1NameUI.text = "Player 1 Name";
            player2NameUI.text = "Player 2 Name";
            player3NameUI.text = "Player 3 Name";
            player4NameUI.text = "Player 4 Name";

            player1Indicator.SetActive(false);
            player2Indicator.SetActive(false);
            player3Indicator.SetActive(false);
            player4Indicator.SetActive(false);
        }
    }
}
