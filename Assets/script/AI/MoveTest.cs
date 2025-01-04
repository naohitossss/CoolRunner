using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float npcSpeed = 10.0f;
    [SerializeField] private Vector3 newPos;
    private Rigidbody rb;
    private Animator anim;

    private Vector3 moveDirection;
    private AIObstacleAvoid obstacleAvoid; 

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // (1) 同じオブジェクトにアタッチされている障害物回避スクリプトを取得
        obstacleAvoid = GetComponent<AIObstacleAvoid>();

        SetMoveDirection(newPos);
    }

    // 移動先方向を更新
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = (direction - transform.position).normalized;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
    public void SetSpeed(float speed)
    {
        npcSpeed = speed;
    }

    private void UpdateAnim()
    {
        anim.SetFloat("Speed", npcSpeed);
    }

    void Update()
    {
        // (2) もし障害物回避スクリプトがあるなら、回避ベクトルを取得
        Vector3 avoidVec = Vector3.zero;
        if (obstacleAvoid != null)
        {
            //avoidVec = obstacleAvoid.GetAvoidVelocity();
        }

        // (4) 「本来の移動方向 moveDirection + 回避方向 avoidVec3D」
        Vector3 finalMoveDirection = (moveDirection + avoidVec).normalized;

        // (5) 実際に移動
        rb.MovePosition(transform.position + finalMoveDirection * npcSpeed * Time.deltaTime);
        RotateTowardsMovementDirection(finalMoveDirection);
        UpdateAnim();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + moveDirection);
    }
    void RotateTowardsMovementDirection(Vector3 MoveDirection)
    {
        if (npcSpeed > 0.1f)
        {
            // 移動方向を取得
            // 上向きベクトルを基準に回転を計算
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection, Vector3.up);
            // スムーズな回転を適用
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
