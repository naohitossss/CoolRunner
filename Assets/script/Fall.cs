using UnityEngine;

public class Fall : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        // キャラクターが落とし穴に触れた場合
        if (other.CompareTag("Player"))
        {

            EventHandler.CallGetGameOverEvent();
        }
    }
}
