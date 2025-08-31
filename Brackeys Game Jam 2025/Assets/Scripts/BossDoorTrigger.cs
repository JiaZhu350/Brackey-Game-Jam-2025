using UnityEngine;

public class BossDoorTrigger : MonoBehaviour
{
    [SerializeField] public Collider2D _door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _door != null)
        {
            _door.enabled = true;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>()._canTeleport = false;
        }
    }
}
