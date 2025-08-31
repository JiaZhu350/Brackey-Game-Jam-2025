using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Player _player;

    private void Update()
    {
        UpdateHorizontalMovement();
        UpdateVerticalMovement();
    }

    private void UpdateHorizontalMovement()
    {
        if (Mathf.Abs(_rb.linearVelocity.x) >= 0.1f)
        {
            _anim.SetBool("isWalking", true);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }
    }

    private void UpdateVerticalMovement()
    {
        if (_player.IsGrounded())
        {
            _anim.SetBool("isGrounded", true);
        }
        else
        {
            _anim.SetBool("isGrounded", false);
        }
    }

    public void StartAttackAnimation()
    {
        _anim.SetTrigger("isAttacking");
    }

    public void StartDash()
    {
        _anim.SetBool("isDashing", true);
    }

    public void StopDash()
    {
        _anim.SetBool("isDashing", false);
    }

    public void PlayerHurt()
    {
        _anim.SetTrigger("isHurt");
    }
}
