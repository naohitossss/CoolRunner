using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeChange : MonoBehaviour
{
    private Slider volumeSlider;
    [SerializeField] private float defaultVolume = 0.5f;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.minValue = 0.0f;
        volumeSlider.maxValue = 1.0f;
        volumeSlider.value = defaultVolume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        if(AudioManager.Instance != null    )
        {
            AudioManager.Instance.ChangeVolume(value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
