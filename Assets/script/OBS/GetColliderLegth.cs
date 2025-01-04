using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetColliderLegth : MonoBehaviour
{
    public float GetObstacleRadius()
    {
        float obstacleRadius = 0;
        if (GetComponent<Collider>() != null)
        {
            switch (GetComponent<Collider>())
            {
                case SphereCollider sphere:
                    obstacleRadius = sphere.radius;
                    break;

                case BoxCollider box:
                    obstacleRadius = Mathf.Max(box.size.x, box.size.z) / 2.0f;
                    break;

                case CapsuleCollider capsule:
                    obstacleRadius = capsule.radius;
                    break;

                case MeshCollider mesh:
                    Bounds bounds = mesh.bounds;
                    obstacleRadius = Mathf.Max(bounds.extents.x, bounds.extents.z);
                    break;
                default:
                    Debug.Log("nocollider"); 
                    break;
                    
            }
                
        }
        return obstacleRadius;
    }
}



