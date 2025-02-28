using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObstacleAvoid : MonoBehaviour
{
    // 障害物リスト
    public List<GameObject> obstacleList { get; private set; }
    private float dDboxWidth; // 検出ボックスの幅
    private float dDboxLength; // 検出ボックスの長さ
    
    private Vector3 adjustedSpeed = Vector3.zero;
    // 進行方向の判定

    void Start()
    {
        // 障害物リストの初期化
        obstacleList = new List<GameObject>();
        Collider collider = GetComponent<Collider>();
        // 検出ボックスの幅と長さを取得
        dDboxWidth = collider.bounds.size.x;
        dDboxLength = collider.bounds.size.z;
    }


    // 障害物に衝突したときの処理
    void OnTriggerEnter(Collider other)
    {
        GetColliderLegth getColliderLegth = other.GetComponent<GetColliderLegth>();
        // 障害物の検出
        if (getColliderLegth != null)
        {
            if (!obstacleList.Contains(other.gameObject)) // オブジェクトがリストにない場合追加
            {
                obstacleList.Add(other.gameObject);
            }
        }
    }

    // 障害物から離れたときの処理
    void OnTriggerExit(Collider other)
    {
        // 障害物の検出解除
        if (obstacleList.Contains(other.gameObject))
        {
            obstacleList.Remove(other.gameObject);
        }
    }

    // 回避速度を計算するメソッド
    public Vector3 GetAvoidVelocity(Vector3 currentSpeed, float speed)
    {
        Vector3 adjustedSpeed = currentSpeed;
        float closestIP = float.MaxValue;
        GameObject closestObstacle = null;
        Vector2 closestObstacleLocalPos = Vector2.zero;
        
        foreach (GameObject obstacle in obstacleList)
        {
            if (obstacle == null) continue;

            // 障害物のローカル座標を取得
            Vector3 localPos3D = transform.InverseTransformPoint(obstacle.transform.position);
            Vector2 localPos2D = new Vector2(localPos3D.x, localPos3D.z);

            // 前方にある障害物のみを対象とする
            if (localPos2D.y > 0)
            {
                GetColliderLegth getColliderLegth = obstacle.GetComponent<GetColliderLegth>();
                if (getColliderLegth != null)
                {
                    float obstacleRadius = getColliderLegth.GetObstacleRadius();
                    float expandedRadius = obstacleRadius + dDboxWidth / 2;

                    // 障害物が検出ボックス内にあるか確認
                    if (Mathf.Abs(localPos2D.y) < expandedRadius)
                    {
                        float sqrtPart = Mathf.Sqrt(expandedRadius * expandedRadius - localPos2D.y * localPos2D.y);
                        float ip = localPos2D.x - sqrtPart;
                        
                        if (ip < closestIP)
                        {
                            closestIP = ip;
                            closestObstacle = obstacle;
                            closestObstacleLocalPos = localPos2D;
                        }
                    }
                }
                else
                {
                    Debug.Log("getColliderLegth is null");
                }
            }
        }

        if (closestObstacle != null)
        {
            // 回避速度の計算
            float multiplier = speed + (dDboxLength - Mathf.Abs(closestObstacleLocalPos.y)) / dDboxLength;
            float avoidDirection = -Mathf.Sign(closestObstacleLocalPos.x);
            
            // 後進時は回避方向を反転
            if (currentSpeed.z <= 0)
            {
                avoidDirection *= -1;
            }

            adjustedSpeed.x += avoidDirection * multiplier;
        }

        return adjustedSpeed;
    }

    // デバッグ用のGizmosを描画
    void OnDrawGizmos()
    {
        // 基本の検出範囲（Box）
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(dDboxWidth, 0.1f, dDboxLength));
    }
}
