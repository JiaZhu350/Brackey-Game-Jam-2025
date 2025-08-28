using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : IAttack
{
    public bool rbRestricted { get; set; } = false;
    private bool isAttacking = false;
    private EnemyAttackAnimation _atkAnim;

    public MeleeAttack(EnemyAttackAnimation atkAnim)
    {
        this._atkAnim = atkAnim;
    }

    public IEnumerator AttackPlayer(Transform player, float dmg, float windup, float cd, Rigidbody2D rb)
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        _atkAnim.StartAttack();
        // Wind-up
        yield return new WaitForSeconds(windup);

        Debug.Log($"Enemy dealt {dmg} melee dmg to player");
        player.gameObject.GetComponent<Player>().TakeDamage(dmg);

        _atkAnim.EndAttack();
        // Cooldown
        yield return new WaitForSeconds(cd);
        isAttacking = false;
    }
}
