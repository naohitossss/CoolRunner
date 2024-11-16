using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using UnityEngine.InputSystem;

public class HeatStroke : MonoBehaviour
{
    public float minStroke = 0f;
    [Header("中暑値")]
    [Range(0, 100)]
    public float maxStroke = 100f;
    public float currentStroke;
    [Header("在阳光下每秒增加的中暑值")]
    [Range(0, 10)]
    public float sunExposureRate = 2f;
    [Header("在阴影处每秒减少的中暑值")]
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
        

        // 处理中暑值的逻辑
        HandleStroke();

        // 更新 UI
        strokeBar.value = currentStroke;
    }

    void HandleStroke()
    {

        // 防止中暑值超出范围
        currentStroke = Mathf.Clamp(currentStroke, minStroke, maxStroke);

        // 当中暑值达到最大时，游戏结束
        if (currentStroke >= maxStroke)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        EventHandler.CallGetGameOverEvent();
       // playerInput.enabled = false;
    }
    
}
