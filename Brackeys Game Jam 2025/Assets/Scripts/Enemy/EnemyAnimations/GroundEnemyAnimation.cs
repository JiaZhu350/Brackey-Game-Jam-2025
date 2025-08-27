using UnityEngine;

public class GroundEnemyAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;

    private void Update()
    {
        UpdateAirborne();
        UpdateHorizontalMovement();
    }

    private void UpdateHorizontalMovement()
    {
        if (Mathf.Abs(_rb.linearVelocity.x) >= 0.1f)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    private void UpdateAirborne()
    {
        if (!_enemy.grounded)
        {
            _animator.SetBool("isAirborne", true);
        }
        else
        {
            _animator.SetBool("isAirborne", false);
        }
    }
}
