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
    [Header("Boss")]
    [SerializeField] private bool _isBoss = false;
    [SerializeField] private float _speedMultiplier = 1f;
    [SerializeField] private float _attackSpeedMultiplier = 1f;
    [SerializeField] private float _damageMultiplier = 1f;
    [SerializeField] private bool _hasBoost = false;
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
        Enemy enemy = gameObject.GetComponent<Enemy>();
        Vector2 dir = ((Vector2)player.position - _rb.position).normalized;
        _rb.AddForce(-dir * _knockbackAmount, ForceMode2D.Impulse);
        StartCoroutine(enemy.Stun(_knockbackDuration));
        _animator.SetTrigger("isHurt");
        if (_isBoss && _currentHp <= (_maxHp / 2) && !_hasBoost)
        {
            enemy.ModifyStats(_speedMultiplier, _attackSpeedMultiplier, _damageMultiplier);
            _hasBoost = true;
        }
        if (_currentHp <= 0)
        {
            if (_isBoss)
            {
                Player playerChar = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                playerChar._canTeleport = true;
                playerChar.PlayerHeal(playerChar.GetMaxHealth / 2);
            }
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
