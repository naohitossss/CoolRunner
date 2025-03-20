using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SunMoveControl : MonoBehaviour
{

     [SerializeField] private Light sunLight;
     [SerializeField] private float dayLength = 60f; // 1日を120秒で再現
     [SerializeField] private Vector3 startRotation = new Vector3(50f, -100f, 45f); // 初期角度の設定
    private void Awake()
    {
        sunLight = GetComponent<Light>();
    }


    private void Update()
    {
        SunMove();
    }
    private void SunMove()
    {
        if(SunManager.Instance.GetIisSunMoving())
        {
            float angle = Time.time / dayLength * 360f + startRotation.x;
            sunLight.transform.rotation = Quaternion.Euler(angle, startRotation.y, startRotation.z);
        }
    }
}

