using UnityEngine;
using UnityEngine.UI;

namespace LudoGames.Unity.Menus
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private float x, y;

        void Update()
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x,y) * Time.deltaTime, rawImage.uvRect.size);
        }
    }
}
