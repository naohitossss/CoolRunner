using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RunnerAI : MonoBehaviour
{
    public float npcSpeed = 25f;
    private NavMeshAgent navAgent;
    private Vector3 targetPosition;
    private Vector3 startLanePosition;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        InitializeAI();
    }
    public void SetLanePosition(Vector3 position)
    {
        startLanePosition = position;
    }
    private void InitializeAI()
    {
        if (navAgent == null)
        {
            return;
        }
        
        navAgent.speed = npcSpeed;
        targetPosition = new Vector3(startLanePosition.x, startLanePosition.y, transform.position.z - 340f) ;
        navAgent.SetDestination(targetPosition);
    }

    void Update()
    {        
       
            RotateTowardsMovementDirection();
        
    }

    void RotateTowardsMovementDirection()
    {
        if (navAgent.velocity.magnitude > 0.1f)
        {
            // 移動方向を取得
            Vector3 direction = navAgent.velocity.normalized;
            // 上向きベクトルを基準に回転を計算
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // スムーズな回転を適用
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

}
