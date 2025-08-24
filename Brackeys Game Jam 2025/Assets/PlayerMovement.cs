using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    private float _horizontal;
    

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("up");
            _rb.AddForce(transform.up * 400f);
        }
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontal * 5f, _rb.linearVelocityY);
    }
}
