using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : IAttack
{
    public bool rbRestricted { get; set; } = false;
    private bool isAttacking = false;
    private EnemyAttackAnimation _atkAnim;
    private AudioClip _atkSound;
    private float _volume;

    public MeleeAttack(EnemyAttackAnimation atkAnim, AudioClip atkSound = null, float volume = 0)
    {
        this._atkAnim = atkAnim;
        this._atkSound = atkSound;
        this._volume = volume;
    }

    public IEnumerator AttackPlayer(Transform player, float dmg, float windup, float cd, Rigidbody2D rb)
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        _atkAnim.StartAttack();
        if (_atkSound != null) SoundFXManager.instance.PlaySoundFXClip(_atkSound, rb.transform, _volume, regulated: false);
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
