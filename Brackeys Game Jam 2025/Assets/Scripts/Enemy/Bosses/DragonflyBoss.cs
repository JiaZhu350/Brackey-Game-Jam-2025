using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class DragonflyBoss : Enemy
{
    [Header("Boss Special Attack")]
    [SerializeField] private Transform[] _xPoints;
    [SerializeField] private float _specialAttackCd = 5f;
    [SerializeField] private float _specialAcceleration = 20f;
    [SerializeField] private float _specialSpeed = 10f;
    [SerializeField] private CollisionDamage _dmgCollision;

    [SerializeField] private bool _isPerformingSpecial = false;
    [SerializeField] private float _currentCd;
    private int _specialIndex;

    private void Start()
    {
        _currentCd = _specialAttackCd;
    }

    public override void ModifyStats(float speed, float attackSpd, float damage)
    {
        base.ModifyStats(speed, attackSpd, damage);
        _specialSpeed *= speed;
    }

    protected override void Follow()
    {
        if (_currentCd > 0)
        {
            base.Follow();
            _currentCd -= Time.deltaTime;
        }
        else
        {
            if (_isPerformingSpecial) return;
            StartCoroutine(PerformSpecialAttack());
        }
    }
    protected override void Attack()
    {
        if (_currentCd > 0)
        {
            base.Attack();
            _currentCd -= Time.deltaTime;
        }
        else
        {
            if (_isPerformingSpecial) return;
            StartCoroutine(PerformSpecialAttack());
        }
    }

    private IEnumerator PerformSpecialAttack()
    {
        _isPerformingSpecial = true;
        _specialIndex = 0;

        while (_specialIndex < _xPoints.Length)
        {
            Transform targetPoint = _xPoints[_specialIndex];

            // Keep moving toward this point until close enough
            while (Vector2.Distance(_rb.position, targetPoint.position) > 0.1f)
            {
                base._movement.MoveToward(targetPoint.position, _specialSpeed, _specialAcceleration, _rb);
                yield return null; // wait until next frame
            }

            // Move to next waypoint
            _specialIndex++;
        }

        // Reset
        _isPerformingSpecial = false;
        _currentCd = _specialAttackCd;
    }

    protected override void HandleStates()
    {
        if (_isPerformingSpecial)
        {
            _dmgCollision.enabled = true;
            return;
        }
        else
        {
            _dmgCollision.enabled = false;
        }
        base.HandleStates();
    }
}
