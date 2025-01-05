using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro.Examples;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField playernameInput;
    public TextMeshProUGUI errorTX;
    // Start is called before the first frame update


    void Start()
    {
        playernameInput.onValueChanged.AddListener(ValidateInput);
    }

    void ValidateInput(string input)
    {
        // 正規表現で無効な文字を除去
        string sanitizedInput = Regex.Replace(input, @"[^\w\u4E00-\u9FFF\u3040-\u30FF\uAC00-\uD7AF]", "");

        // 長さをチェック（3〜15文字）
        if (sanitizedInput.Length > 15)
        {
            sanitizedInput = sanitizedInput.Substring(0, 15);
        }

        // 修正した文字列を反映
        if (input != sanitizedInput)
        {
            playernameInput.text = sanitizedInput;
        }
    }

        public void OnNameChangeNameBottonPress()
        {
            if (playernameInput.text.Length < 1)
            {
                LeaderboardManager.Instance.AddScore("NoNamePlayer");
            }
            else
            {
                LeaderboardManager.Instance.AddScore(playernameInput.text);
            }
        }
}
