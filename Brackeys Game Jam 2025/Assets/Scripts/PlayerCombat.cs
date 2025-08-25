using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] private float _maxHeath;
    private float _currentHealth;
    private float _healthModifier;


    [Header("Attack")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackCooldown;
    private float _damageModifider;
    private bool _canAttack;

    private void Awake()
    {
        _canAttack = true;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && _canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        _canAttack = false;
        Collider2D[] _hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _whatIsEnemy);

        foreach (Collider2D _enemy in _hitEnemies)
        {
            Debug.Log(_enemy.name + " is hit!"); //Logic for dealing damage
        }

        yield return new WaitForSeconds(_attackCooldown);

        _canAttack = true;
        
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
    
    private void TakeDamage(float _damageTaken)
    {
        _currentHealth -= _damageTaken;
    }
}
