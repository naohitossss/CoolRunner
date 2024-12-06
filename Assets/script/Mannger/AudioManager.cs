using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // シングルトンインスタンス
    private float audioVolume;

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを切り替えても破棄されない
        }
        else
        {
            Destroy(gameObject); // 複数のインスタンスが生成されないようにする
        }
    }

    public float getVolume() 
    { 
        return audioVolume;
    }

    // 音量を変更するメソッド
    void ChangeVolume(float volume)
    {
        audioVolume = volume;
    }
}
