using System.Collections;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    private bool _ready = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _ready)
        {
            StartCoroutine(DealDamage());
        }
    }

    private IEnumerator DealDamage()
    {
        _ready = false;
        Debug.Log($"Enemy dealt {_damage} collision damage to player");
        // TODO: Add dmg logic here
        yield return new WaitForSeconds(_cooldown);
        _ready = true;
    }
}
