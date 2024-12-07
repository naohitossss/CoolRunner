using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // �V���O���g���C���X�^���X
    private float audioVolume;

    void Awake()
    {
        // �V���O���g���̐ݒ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[����؂�ւ��Ă��j������Ȃ�
        }
        else
        {
            Destroy(gameObject); // �����̃C���X�^���X����������Ȃ��悤�ɂ���
        }
    }

    public float getVolume() 
    { 
        return audioVolume;
    }

    // ���ʂ�ύX���郁�\�b�h
    public void ChangeVolume(float volume)
    {
        audioVolume = volume;
    }
}
