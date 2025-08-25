using UnityEngine;

public class FlyingMovement : IMovement
{
    public void MoveToward(Vector2 target, float speed, float acceleration, Rigidbody2D rb)
    {
        Vector2 dir = (target - rb.position).normalized;
        Vector2 targetVel = dir * speed;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVel, acceleration * Time.deltaTime);
    }
}
