using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUPanelEnter : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panel.SetActive(true);
        }
    }
}
