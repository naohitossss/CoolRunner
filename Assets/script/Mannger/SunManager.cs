using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunManager : MonoBehaviour
{
    [SerializeField] private Toggle sunControlToggle;
    public static SunManager Instance; 
   private bool isSunMoving = false; // 太陽が時間ごとに移動するスイッチ

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
        if (sunControlToggle != null)
        {
            // 初期値を設定
            sunControlToggle.isOn = isSunMoving;
            
            // トグルの値が変更されたときのイベントを登録
            sunControlToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        else
        {
            Debug.LogWarning("Sun Control Toggle is not assigned!");
        }
    }
    private void OnToggleValueChanged(bool value)
    {
        isSunMoving = value;
        Debug.Log($"Sun movement is now {(isSunMoving ? "enabled" : "disabled")}");
    }

    public bool GetIisSunMoving()
    {
        return isSunMoving;
    }
}


