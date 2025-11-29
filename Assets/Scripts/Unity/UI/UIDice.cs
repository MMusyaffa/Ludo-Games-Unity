using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LudoGames.Unity.UI
{
    class UIDice : MonoBehaviour
    {
        private Sprite[] diceSides;
        private Image image;
        public System.Action OnDiceAnimationFinished;

        void Start()
        {
            image = GetComponent<Image>();
            diceSides = Resources.LoadAll<Sprite>("Sprites/Dice/DiceSides/");
        }

        public IEnumerator RollDiceAnimation()
        {
            for (int i = 0; i < 20; i++)
            {
                int randomSide = Random.Range(0, diceSides.Length);
                image.sprite = diceSides[randomSide];

                yield return new WaitForSeconds(0.05f);
            }

            OnDiceAnimationFinished?.Invoke();
        }

        public void SetFinalDiceSprite(int number)
        {
            image.sprite = diceSides[number - 1];
        }
    }
}