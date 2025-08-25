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
        float direction = Mathf.Sign(target.x - rb.position.x);
        if (Time.time >= lastJumpTime + jumpCooldown && IsGroundAhead(rb, direction))
        {
            // Vertical jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            lastJumpTime = Time.time;
        }
        else if (Mathf.Abs(rb.linearVelocityY) >= 0.1f)
        {
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

    private bool IsGroundAhead(Rigidbody2D rb, float dir, float checkDistance = 1f)
    {
        Vector2 origin = rb.position + new Vector2(dir * 0.3f, 0f);

        // Raycast straight down to look for ground
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, checkDistance, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}
