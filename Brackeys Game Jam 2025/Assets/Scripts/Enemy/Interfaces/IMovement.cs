using UnityEngine;

public interface IMovement
{
    void MoveToward(Vector2 target, float speed, float acceleration, Rigidbody2D rb);
}
