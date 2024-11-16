using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに触れたかチェック
        if (other.CompareTag("Player"))
        {
            // レベルクリア処理を実行
            EndLevel();
        }
    }

    private void EndLevel()
    {
        // 必要に応じて追加のチェックを行うことができます
        // 例：ゲームの状態確認やプレイヤーの状態確認など
        EventHandler.CallGetGameClearEvent();
    }
}