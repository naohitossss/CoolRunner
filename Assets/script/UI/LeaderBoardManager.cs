using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    private List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
    private const int MaxEntries = 5;  // 表示する最大ランキング数

    private void Awake()
    {
        // Singletonパターンでインスタンスを保持
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // シーンをまたいでオブジェクトを保持
            LoadScores();  // 起動時にスコアを読み込み
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(string playerName, int score)
    {
        RemoveScore(playerName, score);
        scoreEntries.Add(new ScoreEntry(playerName, score));
        SortAndTrimScores();
        SaveScores();
    }

    public void RemoveScore(string playerName, int score)
    {
        scoreEntries.RemoveAll(entry => 
            entry.playerName == playerName && entry.score == score);
        SaveScores();
    }

    private void SortAndTrimScores()
    {
        scoreEntries = scoreEntries.OrderByDescending(entry => entry.score).Take(MaxEntries).ToList();
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper { scores = scoreEntries });
        File.WriteAllText(GetFilePath(), json);
    }

    private void LoadScores()
    {
        if (File.Exists(GetFilePath()))
        {
            string json = File.ReadAllText(GetFilePath());
            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
            scoreEntries = wrapper.scores;
        }
    }

    private string GetFilePath()
    {
        return Application.persistentDataPath + "/leaderboard.json";
    }

    public List<ScoreEntry> GetScores()
    {
        return scoreEntries;
    }
}

[System.Serializable]
public class ScoreListWrapper
{
    public List<ScoreEntry> scores;
}

