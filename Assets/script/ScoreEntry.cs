using System;

[Serializable]
public class ScoreEntry
{
    public string playerName;  // �v���C���[�̖��O
    public int score;          // �v���C���[�̃X�R�A

    public ScoreEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}

