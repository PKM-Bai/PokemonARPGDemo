using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void SetSound(SoundDetails soundDetails)
    {
        audioSource.clip = soundDetails.soundCilp;
        audioSource.volume = soundDetails.soundVolume;
    }
}
