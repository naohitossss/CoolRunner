using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public void GetColliderOFF(){
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders) {
            collider.enabled = false;
        }
    }
}