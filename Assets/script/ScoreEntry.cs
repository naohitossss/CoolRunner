using System;

[Serializable]
public class ScoreEntry
{
    public string playerName;  // プレイヤーの名前
    public int score;          // プレイヤーのスコア

    public ScoreEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}

