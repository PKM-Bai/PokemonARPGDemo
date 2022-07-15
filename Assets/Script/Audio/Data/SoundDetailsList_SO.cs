using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDetailsList_SO", menuName = "Audio/SoundDetailsList_SO")]
public class SoundDetailsList_SO : ScriptableObject {
    public List<SoundDetails> soundDetailsList; 

    public SoundDetails GetSoundDetails(SoundName name)
    {
        return soundDetailsList.Find(s => s.soundName == name);
    }   

}

[System.Serializable]
public class SoundDetails
{
    public SoundName soundName;
    public AudioClip soundCilp;
    [Range(0f, 1f)]
    public float soundVolume;
}
