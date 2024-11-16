using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private NavMeshAgent agent;
    public float moveRadius = 10f; // NPC のランダム移動範囲
    public float detectionRange = 5f; // 他のNPCを検知する範囲
    public float conversationProbability = 0.5f; // 会話をする確率 (50%)
    public float conversationDuration = 3f; // 会話の持続時間

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPosition();
    }

    void MoveToRandomPosition()
    {
        // ランダムな位置を設定
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, moveRadius, -1);
        agent.SetDestination(navHit.position);
    }

    void Update()
    {
        // NPC がランダムに移動
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToRandomPosition();
        }

        // Raycast で前方にNPCがいるかを検知
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange))
        {
            // NPC にぶつかったら会話か回避かを決定
            if (hit.collider.CompareTag("NPC"))
            {
                // 確率に基づいて会話か回避を選択
                if (Random.value < conversationProbability)
                {
                    // 会話を開始
                    StartCoroutine(ConversationCoroutine());
                }
                else
                {
                    // NPC を避ける
                    AvoidNPC(hit.collider.transform);
                }
            }
        }
    }

    // 会話コルーチン
    IEnumerator ConversationCoroutine()
    {
        // 会話中は移動を停止
        agent.isStopped = true;

        
        
        yield return new WaitForSeconds(conversationDuration);

       
        // 再びランダムに移動
        agent.isStopped = false;
        MoveToRandomPosition();
    }

    // NPC を回避する関数
    void AvoidNPC(Transform otherNPC)
    {
        // 回避のために横に移動する
        Vector3 avoidanceDirection = (transform.position - otherNPC.position).normalized;
        Vector3 newDestination = transform.position + avoidanceDirection * 2f; // 2ユニット回避
        agent.SetDestination(newDestination);

        //Debug.Log("NPC を避けました。");
    }
}
