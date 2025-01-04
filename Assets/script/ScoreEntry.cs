using System;

[Serializable]
public class ScoreEntry
{
    public string playerName;  // �v���C���[�̖��O
    public int distance;          // �v���C���[�̃X�R�A

    public ScoreEntry(string name, int distance)
    {
        playerName = name;
        this.distance = distance;
    }
}

