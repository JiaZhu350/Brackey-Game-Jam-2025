using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private LayerMask _whatIsGround;
    

    [Header("Movement Logic")]
    [SerializeField] private float _normalSpeed;
    private float _horizontalDirection;
    private float _speed;


    [Header("Jump Logic")]
    [SerializeField] private float _raycastLength;
    [SerializeField] private float _jumpForce;
    private bool _canJumpAgain;


    [Header("Dashing Logic")]
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    private bool _canDash;
    private bool _isDashing;


    private bool _isFacingRight; //Sprite flip logic


    private void Awake()
    {
        _canJumpAgain = false;
        _isFacingRight = true;
        _canDash = true;
        _isDashing = false;
        _speed = _normalSpeed;
    }


    private void Update()
    {
        if (_isDashing)
        {
            return;
        }
        _horizontalDirection = Input.GetAxisRaw("Horizontal");

        if (Keyboard.current.spaceKey.wasPressedThisFrame && CanJump())
        {
            PlayerJump();
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame && _canDash)
        {
            PlayerDash();
        }

        FlippingPlayer();
    }

    void FixedUpdate()
    {   
        if (_isDashing)
        {
            return;
        }
        _rigidbody.linearVelocity = new Vector2(_horizontalDirection * _speed, _rigidbody.linearVelocityY);
    }

    private void FlippingPlayer()
    {
        if (_isFacingRight && _horizontalDirection < 0 || !_isFacingRight && _horizontalDirection > 0)
        {
            Vector3 localScale = transform.localScale;
            _isFacingRight = !_isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }        
    }

    private bool CanJump()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _raycastLength, _whatIsGround) || _canJumpAgain;
    }


    private void PlayerJump()
    {
        _rigidbody.linearVelocityY = 0f; // Reset the linear Y velocity to allow for better jumping logic
        _rigidbody.AddForce(transform.up * _jumpForce);
        _canJumpAgain = !_canJumpAgain;
    }

    private void PlayerDash()
    {
        StartCoroutine(Dashing());
    }

    private IEnumerator Dashing()
    {
        _isDashing = true;
        _canDash = false;
        float originalGravity = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0f; //Makes gravity unable to affect the Dash
        _rigidbody.linearVelocity = new Vector2(transform.localScale.x * _dashSpeed, 0f); //Dashing Speed
        yield return new WaitForSeconds(_dashTime);
        _rigidbody.gravityScale = originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
}
