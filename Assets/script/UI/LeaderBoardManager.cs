using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// ハイスコアを管理するクラス
public class LeaderboardManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static LeaderboardManager Instance { get; private set; }

    private List<ScoreEntry> scoreEntries = new List<ScoreEntry>(); // スコアエントリーのリスト
    private const int MaxEntries = 5;  // 表示する最大ランキング数

    private void Awake()
    {
        // シングルトンの初期化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでオブジェクトを保持
            LoadScores();  // 保存データを読み込み
        }
        else
        {
            Destroy(gameObject); // 既にインスタンスが存在する場合は破棄
        }
    }

    // 新しいスコアを追加
    public void AddScore(string playerName, int score)
    {
        RemoveScore(playerName, score); // 同じスコアがあれば削除
        scoreEntries.Add(new ScoreEntry(playerName, score)); // 新しいスコアを追加
        SortAndTrimScores(); // スコアをソートして上位を保持
        SaveScores(); // スコアを保存
    }

    // 指定したスコアを削除
    public void RemoveScore(string playerName, int score)
    {
        scoreEntries.RemoveAll(entry => 
            entry.playerName == playerName && entry.score == score); // 条件に合うスコアを削除
        SaveScores(); // スコアを保存
    }

    // スコアを降順でソートして上位5件を保持
    private void SortAndTrimScores()
    {
        scoreEntries = scoreEntries.OrderByDescending(entry => entry.score).Take(MaxEntries).ToList(); // スコアをソートして上位を保持
    }

    // スコアをJSONファイルに保存
    private void SaveScores()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper { scores = scoreEntries }); // スコアをJSONに変換
        File.WriteAllText(GetFilePath(), json); // ファイルに書き込み
    }

    // JSONファイルからスコアを読み込み
    private void LoadScores()
    {
        if (File.Exists(GetFilePath()))
        {
            string json = File.ReadAllText(GetFilePath()); // ファイルから読み込み
            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json); // JSONをオブジェクトに変換
            scoreEntries = wrapper.scores; // スコアリストを更新
        }
    }

    // 保存ファイルのパスを取得
    private string GetFilePath()
    {
        return Application.persistentDataPath + "/leaderboard.json"; // ファイルパスを返す
    }

    // 現在のスコアリストを取得
    public List<ScoreEntry> GetScores()
    {
        return scoreEntries; // スコアリストを返す
    }
}

// JSONシリアライズ用のラッパークラス
[System.Serializable]
public class ScoreListWrapper
{
    public List<ScoreEntry> scores; // スコアのリスト
}

