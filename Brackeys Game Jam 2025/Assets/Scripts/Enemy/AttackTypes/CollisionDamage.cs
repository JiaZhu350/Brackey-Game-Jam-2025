using System.Collections;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    private bool _ready = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _ready)
        {
            StartCoroutine(DealDamage(collision.gameObject));
        }
    }

    private IEnumerator DealDamage(GameObject player)
    {
        _ready = false;
        Debug.Log($"Enemy dealt {_damage} collision damage to player");
        player.GetComponent<Player>().TakeDamage(_damage);
        yield return new WaitForSeconds(_cooldown);
        _ready = true;
    }
}
