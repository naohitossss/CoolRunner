using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float wanderRadius = 15f;          // _ړ̔a
    public float NpcSpeed = 10.0f;             // NPC̈ړx
    private Animator anim;                   // Animator
    [SerializeField] private Vector3 newPos;
    private Rigidbody rb;
    private Vector3 moveDirection;



    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        SetMoveDirection(newPos);
    }

    public void SetMoveDirection(Vector3 direction) 
    {
        moveDirection =  (direction - transform.position).normalized;
    }

    public Vector3 GetMoveDirection() 
    {
        return moveDirection;
    }

    void Update()
    {
        rb.MovePosition(transform.position + moveDirection * NpcSpeed * Time.deltaTime);
    }
}
