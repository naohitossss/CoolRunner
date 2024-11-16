using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // ゲームオーバーとクリア画面のパネル
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    private bool isStart = false;
    // 追加するフィールド
    public TextMeshProUGUI distanceText; // 距離表示用のテキスト
    private float currentDistance = 0f;

    // スクリプト開始時に実行
    private void Start()
    {
        // マウスカーソルのロックを解除
        Cursor.lockState = CursorLockMode.None;
    }

    // スクリプトが有効になった時に実行
    private void OnEnable()
    {
        // ゲーム速度を通常に戻す
        Time.timeScale = 1;
        // ポイント獲得イベントの登録
        EventHandler.GetPointEvent += OnGetPointEvent;
        // ゲーム終了イベントの登録
        EventHandler.GetGameOverEvent += OnGetGameOverEvent;
        EventHandler.GetGameClearEvent += OnGetGameClearEvent;
        EventHandler.UpdateDistanceEvent += OnUpdateDistance; // 距離更新イベントの登録
    }

    // スクリプトが無効になった時に実行
    private void OnDisable()
    {
        // イベントの登録解除
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GetGameOverEvent -= OnGetGameOverEvent;
        EventHandler.GetGameClearEvent -= OnGetGameClearEvent;
        EventHandler.UpdateDistanceEvent -= OnUpdateDistance; // イベントの登録解除
    }


    /// ゲームオーバー時のイベントハンドラー
    private void OnGetGameOverEvent()
    {
        // ゲームオーバーパネルを表示
        gameOverPanel.SetActive(true);
        Debug.Log("currentDistance: " + currentDistance);
        LeaderboardManager.Instance.AddScore("LocalPlayer", (int)currentDistance);
        // ゲームオーバーパネルが表示された時の処理
        if (gameOverPanel.activeInHierarchy)
        {
            // マウスカーソルを表示して操作可能にする
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // ゲームを一時停止
            Time.timeScale = 0;
        }
    }

    // ゲームクリア時のイベントハンドラー
    private void OnGetGameClearEvent()
{
    // distanceTextのnullチェック
    if (distanceText != null)
    {
        distanceText.gameObject.SetActive(false);
    }
    
    // gameClearPanelのnullチェック
    if (gameClearPanel != null)
    {
        gameClearPanel.SetActive(true);
        
        // ゲームクリアパネルが表示された時の処理
        if (gameClearPanel.activeInHierarchy)
        {
            // マウスカーソルを表示して操作可能にする
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // ゲームを一時停止
            Time.timeScale = 0;
        }
    }
    else
    {
        Debug.LogError("gameClearPanel が設定されていません。InspectorでUIManagerの設定を確認してください。");
        }
    }   

    // タイトル画面に戻る
    public void LoadTitle()
    {
        SceneManager.LoadScene("Openpanel");
    }

    private void OnGetPointEvent()
    {
        // ポイント獲得時のデバッグログ
        Debug.Log("Point event triggered!");
        // 将来的にスコア表示などの機能を追加予定
    }

    // ゲームを再スタート
    public void RestartGame()
    {
        // ゲーム速度を通常に戻す
        Time.timeScale = 1;
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 新しく追加するメソッド
    private void OnUpdateDistance(float distance)
    {
        currentDistance = distance;
        if (distanceText != null)
        {
            distanceText.text =  currentDistance.ToString("F1") + "m";
        }
    }
}