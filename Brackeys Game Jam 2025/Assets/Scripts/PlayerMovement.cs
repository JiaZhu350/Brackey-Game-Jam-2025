using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _raycastLength;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _sprintSpeed;
    private float _horizontalDirection;
    private float _speed;
    private bool _canJumpAgain;


    private void Awake()
    {
        _canJumpAgain = false;
        _speed = _normalSpeed;
    }


    private void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");

        if (Keyboard.current.spaceKey.wasPressedThisFrame && CanJump())
        {
            PlayerJump();
        }
    }


    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontalDirection * _speed, _rb.linearVelocityY);
    }


    private bool CanJump()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _raycastLength, _whatIsGround) || _canJumpAgain;
    }


    private void PlayerJump()
    {
        _rb.linearVelocityY = 0f; // Reset the linear Y velocity to allow for better jumping logic
        _rb.AddForce(transform.up * _jumpForce);
        _canJumpAgain = !_canJumpAgain;
    }
}
