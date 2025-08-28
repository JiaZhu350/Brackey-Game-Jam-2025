using UnityEngine;

public class Collectables : MonoBehaviour
{
    public int biscuits;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().ModifyBiscuit(biscuits);
            Destroy(gameObject);
        }
    }
}
