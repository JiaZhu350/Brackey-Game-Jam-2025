using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : IAttack
{
    public bool rbRestricted { get; set; } = false;
    private bool isAttacking = false;

    public IEnumerator AttackPlayer(Transform player, float dmg, float windup, float cd, Rigidbody2D rb)
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;

        // Wind-up
        yield return new WaitForSeconds(windup);

        Debug.Log($"Enemy dealt {dmg} melee dmg to player");
        /*var health = player.GetComponent<>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }*/

        // Cooldown
        yield return new WaitForSeconds(cd);
        isAttacking = false;
    }
}
