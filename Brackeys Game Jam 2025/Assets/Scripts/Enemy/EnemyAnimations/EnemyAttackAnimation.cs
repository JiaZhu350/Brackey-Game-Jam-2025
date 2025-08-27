using UnityEngine;

public class EnemyAttackAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void StartAttack()
    {
        _animator.SetBool("isAttacking", true);
    }

    public void EndAttack()
    {
        _animator.SetBool("isAttacking", false);
    }
}
