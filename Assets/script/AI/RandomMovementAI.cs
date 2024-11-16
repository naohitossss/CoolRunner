using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShadowMovement : MonoBehaviour
{
    public float wanderRadius = 15f;          // ランダム移動の半径
    public float npcSpeed = 3.5f;             // NPCの移動速度
    public Transform lightTarget;             // 光源のTransform
    private NavMeshAgent agent;               // NavMeshAgent
    private Animator anim;                   // Animator
    private Vector3 newPos;                   
    private int maxAttempts = 10;             // 影の中の位置を探す最大試行回数
    public string targetLightName = "Directional Light"; // 光源の名前

    void Start()
    {
        StartCoroutine(InitializeAfterNavMeshBake());
    }

    private IEnumerator InitializeAfterNavMeshBake()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = npcSpeed;               // 移動速度の設定
        agent.stoppingDistance = 3f;        // 停止距離の精度
        agent.acceleration = 10f;             // 加速度設定
        agent.autoBraking = false;            // 自動ブレーキをオフ
        anim = GetComponent<Animator>();
        Light[] lights = FindObjectsOfType<Light>(); // 光源を取得
        // 光源を探す
        foreach (Light light in lights)
        {
            if (light.name == targetLightName)
            {
                lightTarget = light.transform;
            }
        }
        MoveToNewPosition();                  // 最初の目的地を設定
    }

    void Update()
    {
        if (agent == null) return;
        
        // パスが保留中でなく、残りの距離が停止距離以下の場合
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNewPosition();
        }

        RotateTowardsMovementDirection();    // 移動方向に回転させる
        UpdateAnim();                        // アニメーションを更新
    }

    // ランダムな新しい目的地を設定する関数
    void MoveToNewPosition()
    {
        int attempts = 0;
        bool foundShadowPosition = false;

        while (attempts < maxAttempts)
        {
            // 新しいランダムな位置を探す
            newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if (IsInShadow(newPos))
            {
                foundShadowPosition = true;
                break;
            }
            attempts++;
        }

        if (foundShadowPosition)
        {
            agent.SetDestination(newPos); // NavMeshAgent に新しい目的地を設定
        }
        
    }

    // 位置が影の中にあるかを判定する関数
    bool IsInShadow(Vector3 position)
    {
        Vector3 targetDirection = (Quaternion.Euler(lightTarget.eulerAngles) * Vector3.forward).normalized * -1;
        Ray ray = new Ray(position, targetDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red); // デバッグ用の赤い線を表示
            return true;  // 影の中
        }
        return false;     // 光の中
    }

    // 移動方向に基づいてキャラクターを回転させる関数
    void RotateTowardsMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // ランダムな位置を計算する関数
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    // アニメーションの更新
    private void UpdateAnim()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speedPercent);
    }
}


