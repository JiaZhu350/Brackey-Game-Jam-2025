using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rb;

    private void Update()
    {
        UpdateHorizontalMovement();
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
}
