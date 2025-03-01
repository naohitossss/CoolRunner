using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{
    [SerializeField] private GameObject attackPrefab;   // 投げるオブジェクト
    [SerializeField] private Transform throwPoint;      // ボスの手元（投げ始めの位置）
    [SerializeField] private int objectCount = 5;       // 投げるオブジェクトの数
    [SerializeField] private float spreadRadius = 8f;   // 着地範囲の半径
    [SerializeField] private float forwardOffset = 100f;  // プレイヤー前方のオフセット
    [SerializeField] private float throwHeight = 5f;    // 投げ上げる高さ
    [SerializeField] private float throwDuration = 1f;  // 投げてから着地までの時間

    [SerializeField] private float bossSpawnHeight = 5f;   // ボスの出現高さ
    [SerializeField] private float bossForwardOffset = 100f; // プレイヤーの前方オフセット
    [SerializeField] private float followSpeed = 5f;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // プレイヤーとの距離を計算
                Vector3 bossPosition = playerTransform.position + 
                              (playerTransform.forward * bossForwardOffset) + 
                              (Vector3.up * bossSpawnHeight);


        // ボスの位置を更新
        transform.position = Vector3.Lerp(
            transform.position, 
            bossPosition, 
            Time.deltaTime * playerTransform.GetComponent<LaneMovement>().moveSpeed
        );

        // プレイヤーの方向を向く
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0f; // Y軸回転のみ
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                Time.deltaTime * followSpeed
            );
        }
    }

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }
    public void ThrowObjects()
    {
        if (playerTransform == null || attackPrefab == null) return;

        // プレイヤーの前方位置を計算
        Vector3 centerPoint = playerTransform.position + (playerTransform.forward * forwardOffset);

        for (int i = 0; i < objectCount; i++)
        {
            // ランダムな着地位置を計算
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(0f, spreadRadius);
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
            Vector3 targetPosition = centerPoint + offset;

            // オブジェクトを生成して投げる
            GameObject proj = Instantiate(attackPrefab, throwPoint.position, Quaternion.identity);
            StartCoroutine(ThrowProjectile(proj, targetPosition));
        }
    }

    private IEnumerator ThrowProjectile(GameObject projectile, Vector3 targetPos)
    {
        Vector3 startPos = projectile.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < throwDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / throwDuration;

            // 放物線を描くように位置を計算
            float parabolicHeight = Mathf.Sin(normalizedTime * Mathf.PI) * throwHeight;
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, normalizedTime);
            currentPos.y += parabolicHeight;

            projectile.transform.position = currentPos;
            yield return null;
        }

        // 着地位置に到達
        projectile.transform.position = targetPos;

        // ここで着地時のエフェクトなどを再生
        // Instantiate(landingEffectPrefab, targetPos, Quaternion.identity);
    }
}
