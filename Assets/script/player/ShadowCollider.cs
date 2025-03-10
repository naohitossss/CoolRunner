﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShadowCollider : MonoBehaviour
{
    public Transform lightTarget;
    public bool ifShadow;
    // 影の状態を更新する
    void Update()
    {
        Vector3 target = (Quaternion.Euler(lightTarget.eulerAngles) * Vector3.forward).normalized * -1;
        Ray ray = new Ray(transform.position, target);
        RaycastHit hit;
        // レイキャストで影の状態をチェック
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log("碰撞对象" + hit.collider.name);
            //Debug.DrawLine(ray.origin, hit.point, Color.red);
            ifShadow = true;
        }
        else
        {
            ifShadow = false;
        }
        
    }
    public IEnumerator SetIfShadowForDuration(float time)
    {
        ifShadow = true;
        // 指定した秒数だけ待機
        yield return new WaitForSeconds(time);

        // その後 `ifShadow` を `false` に戻す
        ifShadow = false;

    }

}

