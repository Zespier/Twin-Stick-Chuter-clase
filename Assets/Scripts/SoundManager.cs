using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public AudioMixer audioMixer;

    public void SetSound(float volume) {
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(volume) * 20);
    }
    
    public void SetMusic(float volume) {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

}
