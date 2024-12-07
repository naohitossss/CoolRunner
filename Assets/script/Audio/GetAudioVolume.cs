using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioVolume : MonoBehaviour
{
    private AudioSource BGM;
    void Start()
    {
        BGM = GetComponent<AudioSource>();
        if(AudioManager.Instance != null)   
        {
            BGM.volume = AudioManager.Instance.getVolume();
        }
    }
}
