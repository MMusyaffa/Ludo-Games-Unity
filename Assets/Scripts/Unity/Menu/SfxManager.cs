using UnityEngine;
using UnityEngine.UI;

namespace LudoGames.Unity.Menus
{
    public class SfxManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioClip buttonClick;
        [SerializeField] private AudioClip diceClip;
        [SerializeField] private AudioClip pawnClip;
        [SerializeField] private AudioClip winClip;
        [SerializeField] private AudioClip loseClip;

        void Start()
        {
            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                LoadVolume();
            }
            else
            {
                PlayerPrefs.SetFloat("sfxVolume", 1);
                LoadVolume();
            }
        }

        public void SetVolume()
        {
            audioSource.volume = sfxSlider.value;
            SaveVolume();
        }

        private void SaveVolume()
        {
            PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        }

        private void LoadVolume()
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }

        public void PlayButtonClickClip()
        {
            audioSource.PlayOneShot(buttonClick);
        }

        public void PlayDiceClip()
        {
            audioSource.PlayOneShot(diceClip);
        }

        public void PlayPawnClip()
        {
            audioSource.PlayOneShot(pawnClip);
        }

        public void PlayWinClip()
        {
            audioSource.PlayOneShot(winClip);
        }

        public void PlayLoseClip()
        {
            audioSource.PlayOneShot(loseClip);
        }
    }
}