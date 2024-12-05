using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObstacleAvoid : MonoBehaviour
{
    private List<GameObject> obstacleList = new List<GameObject>();
    [SerializeField] private Vector2 speed;
    private float m_dDboxWidth; // 検出ボックスの幅
    private float m_dDboxLength; // 検出ボックスの長さ

    void Start()
    {
        Collider collider = GetComponent<Collider>();
        m_dDboxWidth = collider.bounds.size.x;
        m_dDboxLength = collider.bounds.size.z;
    }

    public void SetSpeed(Vector2 speed)
    {
        this.speed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            obstacleList.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        obstacleList.Remove(other.gameObject);
    }

    public Vector2 GetAvoidSpeed()
    {
        float closestIP = float.MaxValue;
        GameObject closestObstacle = null;
        Vector2 closestObstacleLocalPos = Vector2.zero;
        Vector2 adjustedSpeed = speed;
        float obstacleRadius = 0;

        foreach (GameObject obstacle in obstacleList)
        {
            Vector2 LocalPos = PointToLocalSpace(obstacle.transform.position.x, obstacle.transform.position.z);

            if (LocalPos.x >= 0)
            {
                Collider collider = obstacle.GetComponent<Collider>();
                if (collider != null)
                {
                    float ExpandedRadius = 0;

                    if (collider is SphereCollider sphere)
                    {
                        ExpandedRadius = sphere.radius + m_dDboxWidth / 2;
                        obstacleRadius = sphere.radius;
                    }
                    else if (collider is BoxCollider box)
                    {
                        Vector3 boxSize = box.size * 0.5f;
                        ExpandedRadius = Mathf.Max(boxSize.x, boxSize.z) + m_dDboxWidth / 2;
                        obstacleRadius = Mathf.Max(box.size.x, box.size.z) / 2.0f;
                    }
                    else if (collider is CapsuleCollider capsule)
                    {
                        ExpandedRadius = capsule.radius + m_dDboxWidth / 2;
                        obstacleRadius = capsule.radius;
                    }
                    else if (collider is MeshCollider mesh)
                    {
                        Bounds bounds = mesh.bounds;
                        ExpandedRadius = Mathf.Max(bounds.extents.x, bounds.extents.z) + m_dDboxWidth / 2;
                        obstacleRadius = Mathf.Max(bounds.extents.x, bounds.extents.z);
                    }

                    if (Mathf.Abs(LocalPos.y) < ExpandedRadius)
                    {
                        float SqrtPart = Mathf.Sqrt(ExpandedRadius * ExpandedRadius - LocalPos.y * LocalPos.y);
                        float ip = LocalPos.x - SqrtPart;
                        if (ip < 0)
                        {
                            ip = LocalPos.x + SqrtPart;
                        }

                        if (ip < closestIP)
                        {
                            closestIP = ip;
                            closestObstacle = obstacle;
                            closestObstacleLocalPos = LocalPos;
                        }
                    }
                }
            }
        }

        if (closestObstacle != null)
        {
            float multiplier = 1.0f + (m_dDboxLength - closestObstacleLocalPos.x) / m_dDboxLength;
            adjustedSpeed.y += (obstacleRadius - closestObstacleLocalPos.y) * multiplier;
        }

        return adjustedSpeed;
    }

    private Vector2 PointToLocalSpace(float worldX, float worldZ)
    {
        Vector3 localPoint = transform.InverseTransformPoint(new Vector3(worldX, 0, worldZ));
        return new Vector2(localPoint.x, localPoint.z);
    }

    private Vector2 VectorToWorldSpace(Vector2 localVector, Vector3 forward, Vector3 side)
    {
        Vector3 worldVector = forward * localVector.x + side * localVector.y;
        return new Vector2(worldVector.x, worldVector.z);
    }
}
