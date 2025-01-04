using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LeaderboardDisplay : MonoBehaviour
{
    public Transform scoreListParent;  // スコア表示用の親オブジェクト
    public GameObject scoreEntryPrefab;  // スコア表示用のUIプレハブ

    private void Start()
    {
        DisplayScores();
    }

    public void DisplayScores()
    {
        if (scoreListParent == null || scoreEntryPrefab == null)
        {
            Debug.LogError("Required references are not set in LeaderboardDisplay!");
            return;
        }

        // 既存の表示をクリア
        foreach (Transform child in scoreListParent)
        {
            Destroy(child.gameObject);
        }

        // スコアデータをLeaderboardManagerから取得してUIを生成
        List<ScoreEntry> scores = LeaderboardManager.Instance.GetScores();
        foreach (ScoreEntry scoreEntry in scores)
        {
            Debug.Log(scoreEntry.playerName + ": " + scoreEntry.distance);
            GameObject entry = Instantiate(scoreEntryPrefab, scoreListParent);
            entry.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = scoreEntry.playerName;
            entry.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = scoreEntry.distance.ToString() + "m";
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Openpanel");
    }
}
