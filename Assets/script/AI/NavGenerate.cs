using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class NavGenerate : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    void Awake(){
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.enabled = false;
        
    }

    void Start()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.enabled = true;
            navMeshSurface.BuildNavMesh();
        }
        
    }
}