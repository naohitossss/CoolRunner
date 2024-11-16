using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item: MonoBehaviour
{
    public ItemData itemData;

    public float rotationSpeed = 50f;  // アイテムの回転速度

    void Update()
    {
        // アイテムを45度傾けて回転させる
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    // アイテムデータを取得するメソッド
    public ItemData GetitemData (){
        return itemData;
    }

    // アイテムがスタック可能かどうかを取得するメソッド
    public bool GetisStackable(){
        return itemData.isStackable;
    }

    // アイテム名を取得するメソッド
    public string GetitemName(){
        return itemData.itemName;
    }

    public float GetitemValue(){
        return itemData.value;

    }




}
