using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CharactorMove : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5;
    public float sprintMultiplier = 3; // 冲刺速度倍增器
    public float rotationSpeed = 6; // 转向速度
    public float slowRotationSpeed = 3; // 缓慢转向速度
    public float fastRotationSpeed = 12; // 跑步时快速转向速度
    public Animator anim;
    private Vector3 moveDirection;
    

    public Transform cameraTransform; // 相机的Transform引用
    

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 将输入方向转换为相机的方向空间
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 确保方向向量仅在水平面上
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // 计算移动方向
        moveDirection = forward * vertical + right * horizontal;
        moveDirection.Normalize();

        // 如果有移动输入
        if (moveDirection != Vector3.zero)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // 判断当前的转向速度
            float currentRotationSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentRotationSpeed = fastRotationSpeed; // 跑步时转向更快
            }
            else if (vertical == 0 && horizontal != 0)
            {
                currentRotationSpeed = slowRotationSpeed; // 只有水平输入时转向较慢
            }
            else
            {
                currentRotationSpeed = rotationSpeed; // 普通转向速度
            }

            // 插值旋转角色
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
        }

        // 按住左Shift键进行冲刺
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection *= sprintMultiplier;
        }

        // 移动角色
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // 更新动画
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        anim.SetFloat("Speed", moveDirection.magnitude);
    }
}