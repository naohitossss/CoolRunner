using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Idle : StateMachineBehaviour
{
    private float timer = 0f;
    [SerializeField] private float minTime = 2f; // 最小待機時間
    [SerializeField] private float maxTime = 4f; // 最大待機時間
    private float attackTime; // 実際の攻撃までの待機時間
    private Boss_Attack boss_Attack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss_Attack = animator.GetComponent<Boss_Attack>();
        timer = 0f;
        // ランダムな待機時間を設定
        attackTime = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // タイマーを更新
        timer += Time.deltaTime;

        // 設定時間が経過したらAttackトリガーを設定
        if (timer >= attackTime)
        {
            timer = 0f;
            animator.SetTrigger("Attack");
            boss_Attack.ThrowObjects();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ステート終了時にタイマーをリセット
        animator.ResetTrigger("Attack");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
