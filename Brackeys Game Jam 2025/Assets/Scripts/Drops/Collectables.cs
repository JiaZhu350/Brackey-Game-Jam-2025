using UnityEngine;

public class Collectables : MonoBehaviour
{
    public int biscuits;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Give player biscuits here
            Destroy(gameObject);
        }
    }
}
