using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoGames.Unity.Menus
{
    public class SfxManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip diceClip;
        [SerializeField] private AudioClip pawnClip;
        [SerializeField] private AudioClip winClip;
        [SerializeField] private AudioClip loseClip;

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