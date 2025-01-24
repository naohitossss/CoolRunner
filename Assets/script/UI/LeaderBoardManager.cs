using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

// ハイスコアを管理するクラス
public class LeaderboardManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static LeaderboardManager Instance { get; private set; }
    public static FirebaseFirestore db { get; private set; }

    [SerializeField] private List<ScoreEntry> scoreEntries = new List<ScoreEntry>(); // スコアエントリーのリスト
    private const int MaxEntries = 5;  // 表示する最大ランキング数

    private int currentDistance = 0; // 現在の距離

    private void Awake()
    {
        if (Instance == null)
        {
         
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    Debug.Log("Firebase Initialized!");
                }
                else
                {
                    Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
                }
            });
        }

    }
    private void Start()
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
            DontDestroyOnLoad(gameObject); // シーンをまたいでオブジェクトを保持
            LoadScores();  // 保存データを読み込み
        }
        else
        {
            Destroy(gameObject); // 既にインスタンスが存在する場合は破棄
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetCurrentDistance(float distance)
    {
        currentDistance = (int)distance;
    }

    public int GetCurrentDistance(float distance)
    {
        return currentDistance;
    }


    // 新しいスコアを追加
    public void AddScore(string playerName)
    {
        // 重複エントリのチェック
        bool scoreExists = scoreEntries.Any(ScoreEntry => ScoreEntry.playerName == playerName && ScoreEntry.distance == currentDistance);

        if (scoreExists)
        {
            Debug.Log($"Duplicate entry found: Name = {playerName}, Score = {currentDistance}. Entry not added.");
            return; // 重複が見つかった場合、追加しない
        }

        // 新しいスコアを追加
        scoreEntries.Add(new ScoreEntry(playerName,(int)currentDistance));
        Debug.Log($"New score added: Name = {playerName}, Score = {currentDistance}");

        // Firestoreにも追加
        var userdata = new Dictionary<string, object>
        {
            { "Name", playerName },
            { "Distance", currentDistance }
        };

        db.Collection("users_ranking").Document($"{playerName}_{currentDistance}").SetAsync(userdata).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Document successfully written to Firestore!");
            }
            else
            {
                Debug.LogError($"Failed to write document: {task.Exception}");
            }
        });
    }

    // FireStoreからスコアを読み込み
    private void LoadScores()
    {
        db.Collection("users_ranking").OrderByDescending("Distance").Limit(MaxEntries).GetSnapshotAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Dictionary<string, object> userData = doc.ToDictionary();
                        if(scoreEntries.Any(ScoreEntry => ScoreEntry.playerName == userData["Name"].ToString() && 
                        ScoreEntry.distance == int.Parse(userData["Distance"].ToString())))continue;
                        scoreEntries.Add(new ScoreEntry(userData["Name"].ToString(), int.Parse(userData["Distance"].ToString())));
                        Debug.Log($"Retrieved document: {doc.Id}");
                    }
                }
                else
                {
                    Debug.LogError($"Error retrieving documents: {task.Exception}");
                }
            });
    }

    // 現在のスコアリストを取得
    public List<ScoreEntry> GetScores()
    {
        LoadScores();
        return scoreEntries; // スコアリストを返す
    }
}

// JSONシリアライズ用のラッパークラス
[System.Serializable]
public class ScoreListWrapper
{
    public List<ScoreEntry> scores; // スコアのリスト
}

