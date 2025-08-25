using UnityEngine;

public class GroundMovement : IMovement
{
    public void MoveToward(Vector2 target, float speed, float acceleration, Rigidbody2D rb)
    {
        float direction = Mathf.Sign(target.x - rb.position.x);
        float targetVelX = direction * speed;

        float newVelX = Mathf.Lerp(rb.linearVelocity.x, targetVelX, acceleration * Time.deltaTime);
        rb.linearVelocity = new Vector2(newVelX, rb.linearVelocity.y);
    }
}
