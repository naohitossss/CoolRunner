using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDisappear : MonoBehaviour
{
    [Header("Disappear Settings")]
    [SerializeField] private float upForce = 5f;        // 上向きの力
    [SerializeField] private float backForce = 3f;      // 後ろ向きの力
    [SerializeField] private float torque = 10f;        // 回転の力
    [SerializeField] private float afterhittime = 2f;   // 消えるまでの時間
    [SerializeField] private float lifetime = 10f;

    private bool isFlying = false;
    private float hittimer = 0f;
    private float lifetimer = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = true; 
        }
    }

    private void Update()
    {
        lifetimer += Time.deltaTime;
        if (lifetimer >= lifetime)
        {
            Destroy(gameObject);
        }

        if (isFlying)
        {
            hittimer += Time.deltaTime;
            if (hittimer >= afterhittime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player") && !isFlying)
        {
            Debug.Log("Hit player");
            // 物理演算を有効化
            rb.isKinematic = false;

            // プレイヤーの進行方向の逆向きに力を加える
            Vector3 playerForward = hit.gameObject.transform.forward;
            Vector3 force = (-playerForward * backForce + Vector3.up * upForce);


            rb.AddForce(force, ForceMode.Impulse);

            // ランダムな回転を加える
            Vector3 randomTorque = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );

            // NaNチェックを追加
            if (float.IsNaN(randomTorque.x) || float.IsNaN(randomTorque.y) || float.IsNaN(randomTorque.z))
            {
                Debug.LogError("Invalid torque vector: " + randomTorque);
                return;
            }

            rb.AddTorque(randomTorque * torque, ForceMode.Impulse);

            isFlying = true;

            // コライダーを無効化して他のオブジェクトとの衝突を防ぐ
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
}

