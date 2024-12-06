using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// ハイスコアを管理するクラス
public class LeaderboardManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static LeaderboardManager Instance { get; private set; }

    private List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
    private const int MaxEntries = 5;  // 表示する最大ランキング数

    private void Awake()
    {
        // シングルトンの初期化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();  // 保存データを読み込み
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 新しいスコアを追加
    public void AddScore(string playerName, int score)
    {
        RemoveScore(playerName, score);
        scoreEntries.Add(new ScoreEntry(playerName, score));
        SortAndTrimScores();
        SaveScores();
    }

    // 指定したスコアを削除
    public void RemoveScore(string playerName, int score)
    {
        scoreEntries.RemoveAll(entry => 
            entry.playerName == playerName && entry.score == score);
        SaveScores();
    }

    // スコアを降順でソートして上位5件を保持
    private void SortAndTrimScores()
    {
        scoreEntries = scoreEntries.OrderByDescending(entry => entry.score).Take(MaxEntries).ToList();
    }

    // スコアをJSONファイルに保存
    private void SaveScores()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper { scores = scoreEntries });
        File.WriteAllText(GetFilePath(), json);
    }

    // JSONファイルからスコアを読み込み
    private void LoadScores()
    {
        if (File.Exists(GetFilePath()))
        {
            string json = File.ReadAllText(GetFilePath());
            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
            scoreEntries = wrapper.scores;
        }
    }

    // 保存ファイルのパスを取得
    private string GetFilePath()
    {
        return Application.persistentDataPath + "/leaderboard.json";
    }

    // 現在のスコアリストを取得
    public List<ScoreEntry> GetScores()
    {
        return scoreEntries;
    }
}

// JSONシリアライズ用のラッパークラス
[System.Serializable]
public class ScoreListWrapper
{
    public List<ScoreEntry> scores;
}

