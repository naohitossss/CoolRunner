using System;
using UnityEngine;

public static class EventHandler 

{
    // 既存のイベント
    public static event Action GetPointEvent;
    public static event Action GetGameOverEvent;
    public static event Action GetGameClearEvent;
    public static event Action GetGameStopEvent;

    // 新しく追加する距離更新イベント
    public static event Action<float> UpdateDistanceEvent;

    // 既存のメソッド
    public static void CallGetPointEvent()
    {
        GetPointEvent?.Invoke();
    }

    public static void CallGetGameOverEvent()
    {
        GetGameOverEvent?.Invoke();
    }
    public static void CallGetGameStopEvent()
    {
        GetGameStopEvent?.Invoke();
    }

    public static void CallGetGameClearEvent()
    {
        GetGameClearEvent?.Invoke();
    }

    // 新しく追加する距離更新メソッド
    public static void CallUpdateDistanceEvent(float distance)
    {
        UpdateDistanceEvent?.Invoke(distance);
    }
} 