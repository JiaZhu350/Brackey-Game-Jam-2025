using UnityEngine;

public class JumpingMovement : IMovement
{
    private float jumpForce;
    private float jumpCooldown;
    private float lastJumpTime;

    public JumpingMovement(float jumpForce = 7f, float jumpCooldown = 1f)
    {
        this.jumpForce = jumpForce;
        this.jumpCooldown = jumpCooldown;
        this.lastJumpTime = -jumpCooldown;
    }
    public void MoveToward(Vector2 target, float speed, float acceleration, Rigidbody2D rb)
    {
        if (Time.time >= lastJumpTime + jumpCooldown)
        {
            // Vertical jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            lastJumpTime = Time.time;
        }
        else if (Mathf.Abs(rb.linearVelocityY) >= 0.1f)
        {
            float direction = Mathf.Sign(target.x - rb.position.x);
            float targetVelX = direction * speed;

            // Smooth horizontal drift
            float newVelX = Mathf.Lerp(rb.linearVelocity.x, targetVelX, acceleration * Time.deltaTime);

            rb.linearVelocity = new Vector2(newVelX, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(
                Mathf.Lerp(rb.linearVelocity.x, 0f, acceleration * 5f * Time.deltaTime),
                rb.linearVelocity.y
            );
        }
    }
}
