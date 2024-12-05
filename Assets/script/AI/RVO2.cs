using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO2 : MonoBehaviour
{
    public float wanderRadius = 15f;          // �����_���ړ��̔��a
    public float m_NpcSpeed = 3.5f;             // NPC�̈ړ����x
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

    // �����_���ȐV�����ړI�n��ݒ肷��֐�

    private void OnTriggerEnter(Collider other)
    {
        // ���̃I�u�W�F�N�g�ɁuPlayer�v�^�O���t���Ă���ꍇ
        if (other.CompareTag("Enemy"))
        {
            RVO2 rvo2 = other.GetComponent<RVO2>();
            float npcSpeed = rvo2.m_NpcSpeed; 
        }
    }


    // �ړ������Ɋ�Â��ăL�����N�^�[����]������֐�

    // �A�j���[�V�����̍X�V
}
