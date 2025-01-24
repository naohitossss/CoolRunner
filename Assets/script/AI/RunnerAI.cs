using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;

public class RunnerAI : MonoBehaviour
{
    [SerializeField]  private float npcSpeed = 25f;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 startLanePosition;
    [SerializeField]  private float endLegth = 340f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 moveDirection;
    private AIObstacleAvoid obstacleAvoid;
    private Vector3 finalMoveDirection;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // (1) 同じオブジェクトにアタッチされている障害物回避スクリプトを取得
        obstacleAvoid = GetComponent<AIObstacleAvoid>();
        //InitializeAI();

        SetMoveDirection(targetPosition);
    }
    public void SetLanePosition(Vector3 position)
    {
        startLanePosition = position;
    }

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

    void Update()
    {
        //もし障害物回避スクリプトがあるなら、回避ベクトルを取得
        Vector3 avoidVec = Vector3.zero;
        finalMoveDirection = moveDirection;
        if (obstacleAvoid != null && obstacleAvoid.obstacleList != null && obstacleAvoid.obstacleList.Count > 0)
        {
            avoidVec = obstacleAvoid.GetAvoidVelocity(finalMoveDirection, npcSpeed);
            //「本来の移動方向 moveDirection + 回避方向 avoidVec3D」
            finalMoveDirection = (moveDirection + avoidVec).normalized;
        }
        else
        {
            SetMoveDirection(targetPosition);
        }
            rb.MovePosition(transform.position + finalMoveDirection * npcSpeed * Time.deltaTime);
            RotateTowardsMovementDirection(finalMoveDirection);
    }
    void RotateTowardsMovementDirection(Vector3 MoveDirection)
    {
        if (npcSpeed > 0.1f)
        {
            // 移動方向を取得
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection, Vector3.up);
            // スムーズな回転を適用
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    private void InitializeAI()
    {
        targetPosition = new Vector3(startLanePosition.x, startLanePosition.y, transform.position.z - endLegth) ;
    }
    void OnDrawGizmos()
    {
        // 進行方向を描画（青色）
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, finalMoveDirection * 5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPosition);
    }

}
