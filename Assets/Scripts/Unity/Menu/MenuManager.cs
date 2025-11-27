using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudoGames.Unity.Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private string sceneGameName;
        [SerializeField] private string sceneMenuName;

        public void GoToGame()
        {
            SceneManager.LoadScene(sceneGameName);
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(sceneMenuName);
        }
    }
}