using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float bossSpawnHeight = 5f;
    [SerializeField] private float bossForwardOffset = 100f;
    [SerializeField] private float laneChangeInterval = 3f;
    [SerializeField] private float laneChangeSpeed = 5f;

    private float laneChangeTimer;
    private Vector3 targetPosition;
    private int currentLane = 1; 
    private Vector3[] StreatLinePos = new Vector3[3];

    private void Start()
    {
        laneChangeTimer = laneChangeInterval;
    }

    public void SetStreatLinePos(List<Vector3> positions)
    {
        StreatLinePos = positions.ToArray();
        targetPosition = StreatLinePos[currentLane];

    }

    public void UpdatePosition(Transform bossTransform, Transform target)
    {
        if (target == null) return;

        // レーン変更タイマーの更新
        laneChangeTimer -= Time.deltaTime;
        if (laneChangeTimer <= 0)
        {
            // ランダムな新しいレーンを選択（現在のレーンは除外）
            int newLane;
            do
            {
                newLane = Random.Range(0, 3);
            } while (newLane == currentLane);

            currentLane = newLane;
            targetPosition =  StreatLinePos[currentLane];
            laneChangeTimer = laneChangeInterval;
        }
        UpdateBossPos(target);
    }

    private void UpdateBossPos(Transform target)
    {
         // ボス位置の計算
        Vector3 forwardPosition = new Vector3 (0f,0f,target.position.z + target.forward.z * bossForwardOffset);
        Vector3 bossPosition = forwardPosition + 
                             (Vector3.up * bossSpawnHeight) + 
                             targetPosition;

        // スムーズな移動
        transform.position = Vector3.Lerp(
            transform.position,
            bossPosition,
            Time.deltaTime * laneChangeSpeed
        );

        // プレイヤーの方向を向く
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                Time.deltaTime * followSpeed
            );
        }
    }

    public void setBossSpawnHeight(float height)
    {
        bossSpawnHeight = height;
    }
    public void setBossForwardOffset(float offset)
    {
        bossForwardOffset = offset;
    }
}
