using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using UnityEngine.InputSystem;

public class HeatStroke : MonoBehaviour
{
    public float minStroke = 0f;
    [Header("熱中症ゲージ")]
    [Range(0, 100)]
    public float maxStroke = 100f;
    public float currentStroke;
    [Header("熱中症ゲージの増加量")]
    [Range(0, 10)]
    public float sunExposureRate = 3f;
    [Header("影に入った時の熱中症ゲージの減少量")]
    [Range(0, 10)]
    public float shadeRecoveryRate = 2f;

    private UnityEngine.UI.Slider strokeBar;

    private void Start()
    {
        currentStroke = minStroke;
        if (strokeBar != null)
        {
            strokeBar.maxValue = maxStroke;
            strokeBar.value = currentStroke;
        }
    }

    // strokeBarを設定するメソッド
    public void SetStrokeBar(UnityEngine.UI.Slider bar)
    {
        strokeBar = bar;
        if (strokeBar != null)
        {
            strokeBar.maxValue = maxStroke;
            strokeBar.value = currentStroke;
        }
    }

    private void Update()
    {
        // 熱中症値の処理を実行
        HandleStroke();

        // UI（スライダー）の値を更新
        strokeBar.value = currentStroke;
    }

    void HandleStroke()
    {
        // 熱中症値が最小値と最大値の範囲内に収まるように制限
        currentStroke = Mathf.Clamp(currentStroke, minStroke, maxStroke);

        // 熱中症値が最大に達した場合、ゲームオーバー処理を実行
        if (currentStroke >= maxStroke)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        EventHandler.CallGetGameOverEvent();
    }
    
}
