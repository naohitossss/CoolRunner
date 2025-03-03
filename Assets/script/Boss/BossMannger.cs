using UnityEngine;

public class BossMannger : MonoBehaviour
{
    private Animator animator;
    private Transform playerTransform;
    private ThrowAttackPattern attackPattern;
    private ThrowOBSAttackPattern obsAttackPattern;
    private BossMove move;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackPattern = GetComponent<ThrowAttackPattern>();
        move = GetComponent<BossMove>();
        obsAttackPattern = GetComponent<ThrowOBSAttackPattern>();
        playerTransform = GameObject.FindWithTag("Player")?.transform;
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    private void Update()
    {
        if (playerTransform == null) return;
        move.UpdatePosition(transform, playerTransform);
    }

    public void ThrowAttack()
    {
        if (playerTransform != null && attackPattern != null)
        {
            attackPattern.ThrowAttack(playerTransform);
        }
    }
    public void ThrowOBSAttack()
    {
        if (playerTransform != null && obsAttackPattern != null)
        {
            obsAttackPattern.Attack(playerTransform);
        }
    }
}