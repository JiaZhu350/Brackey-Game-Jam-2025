using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private float _currentHp;
    [SerializeField] private int _biscuitsAmount;
    [Header("References")]
    [SerializeField] private GameObject _biscuit;

    private void Start()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(float dmg, Transform player)
    {
        _currentHp -= dmg;
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(_biscuit);
        _biscuit.GetComponent<Collectables>().biscuits = _biscuitsAmount;
        Destroy(gameObject);
    }
}
