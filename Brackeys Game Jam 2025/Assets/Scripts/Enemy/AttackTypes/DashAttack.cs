using System.Collections;
using UnityEngine;

public class DashAttack : IAttack
{
    public bool rbRestricted { get; set; } = true;
    private bool _isAttacking = false;
    private bool _isDashing = false;
    private float _dashPower;
    private float _dashDuration;
    private float _deceleration;
    private bool _grounded;
    private EnemyAttackAnimation _atkAnim;
    private AudioClip _atkSound;
    private float _volume;

    public DashAttack(float dashPower, float dashDuration, float deceleration, bool grounded, EnemyAttackAnimation atkAnim, AudioClip atkSound = null, float volume = 0)
    {
        this._dashPower = dashPower;
        this._dashDuration = dashDuration;
        this._deceleration = deceleration;
        this._grounded = grounded;
        this._atkAnim = atkAnim;
        this._atkSound = atkSound;
        this._volume = volume;
    }
    public IEnumerator AttackPlayer(Transform player, float dmg, float windup, float cd, Rigidbody2D rb)
    {
        if (_isAttacking)
        {
            if (!_isDashing)
            {
                if (_grounded)
                {
                    rb.linearVelocity = new Vector2(
                        Mathf.Lerp(rb.linearVelocity.x, 0f, _deceleration * Time.deltaTime),
                        rb.linearVelocity.y
                    );
                }
                else
                {
                    rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, _deceleration * Time.deltaTime);
                }
            }
            yield break;
        }
        _isAttacking = true;
        _atkAnim.StartAttack();
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (_atkSound != null) SoundFXManager.instance.PlaySoundFXClip(_atkSound, rb.transform, _volume, regulated: false);

        // Wind-up
        yield return new WaitForSeconds(windup);
        _isDashing = true;

        if (_grounded)
        {
            Vector2 dir = new Vector2(player.position.x - rb.position.x, 0).normalized;
            rb.linearVelocity = dir * _dashPower;
        }
        else
        {
            Vector2 dir = ((Vector2)player.position - rb.position).normalized;
            rb.linearVelocity = dir * _dashPower;
        }
        _atkAnim.EndAttack();
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
        // Cooldown
        yield return new WaitForSeconds(cd);
        _isAttacking = false;
    }
}
