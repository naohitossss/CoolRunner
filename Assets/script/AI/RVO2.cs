using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO2 : MonoBehaviour
{
    public float wanderRadius = 15f;          // ランダム移動の半径
    public float m_NpcSpeed = 3.5f;             // NPCの移動速度
    private Animator anim;                   // Animator
    private Vector3 newPos;
    private Rigidbody rb;
    private Vector3 m_moveDirection;



    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void SetMoveDirection(Vector3 playerTransform,Vector3 direction) 
    {
        m_moveDirection =  (direction - playerTransform).normalized;
    }

    public Vector3 GetMoveDirection() 
    {
        return m_moveDirection;
    }

    void Update()
    {

    }

    // ランダムな新しい目的地を設定する関数

    private void OnTriggerEnter(Collider other)
    {
        // 他のオブジェクトに「Player」タグが付いている場合
        if (other.CompareTag("Enemy"))
        {
            RVO2 rvo2 = other.GetComponent<RVO2>();
            float npcSpeed = rvo2.m_NpcSpeed; 
        }
    }


    // 移動方向に基づいてキャラクターを回転させる関数

    // アニメーションの更新
}
