using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private float _currentHp;
    [SerializeField] private int _biscuitsAmount;
    [Header("Knockback")]
    [SerializeField] private float _knockbackAmount = 10f;
    [SerializeField] private float _knockbackDuration = 1f;
    [Header("References")]
    [SerializeField] private GameObject _biscuit;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(float dmg, Transform player)
    {
        _currentHp -= dmg;
        Vector2 dir = ((Vector2)player.position - _rb.position).normalized;
        _rb.AddForce(-dir * _knockbackAmount, ForceMode2D.Impulse);
        StartCoroutine(gameObject.GetComponent<Enemy>().Stun(_knockbackDuration));
        _animator.SetTrigger("isHurt");
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject biscuitInstance = Instantiate(_biscuit, transform.position, Quaternion.identity);
        biscuitInstance.GetComponent<Collectables>().biscuits = _biscuitsAmount;
        Destroy(gameObject);
    }
}
