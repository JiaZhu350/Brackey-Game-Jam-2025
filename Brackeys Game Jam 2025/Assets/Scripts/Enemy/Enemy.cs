using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum MovementType { Ground, Flying, Jumping, None }
    private enum AttackType { Melee, Dash, None}
    private enum State { Idle, Patrol, Follow, Attack }

    [Header("Movement")]
    [SerializeField] private MovementType _movementType;
    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _acceleration = 3f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _stoppingDistance = 0.2f;
    [SerializeField] private float _idleTime = 2f;
    [SerializeField] private bool _isFacingLeft = true;
    [SerializeField] private float _duskRate = 20f;

    [Header("Jump (if selected)")]
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _jumpCd = 1f;

    [Header("Attack")]
    [SerializeField] private AttackType _attackType;
    [SerializeField] private float _atkDamage;
    [SerializeField] private float _atkWindup;
    [SerializeField] private float _atkCd;
    [SerializeField] private float _attackRange = 2f;

    [Header("Dash (if selected)")]
    [SerializeField] private float _dashPower;
    [SerializeField] private float _dashDuration;

    [Header("Audio")]
    [SerializeField] private float _audioRange = 8f;
    [SerializeField] private float _audioVolume = 0.5f;
    [SerializeField] private AudioClip _idleAudio;
    [SerializeField] private AudioClip _followAudio;
    [SerializeField] private AudioClip _attackAudio;

    [Header("References")]
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] private Transform _player;
    [SerializeField] private BoxCollider2D _groundCheck;
    [SerializeField] private ParticleSystem _dustVFX;
    [SerializeField] private EnemyAttackAnimation _enemyAtkAnim;

    [SerializeField] private State _currentState;
    protected IMovement _movement;
    protected IAttack _attack;
    private int _patrolIndex;
    private float _idleTimer = 0f;
    [SerializeField] private bool _isStunned = false;
    public bool grounded;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;
        var duskEmission = _dustVFX.emission;
        duskEmission.rateOverTime = _duskRate;
        switch (_movementType)
        {
            case MovementType.Ground:
                _movement = new GroundMovement();
                break;
            case MovementType.Flying:
                _movement = new FlyingMovement();
                break;
            case MovementType.Jumping:
                _movement = new JumpingMovement(_jumpForce, _jumpCd);
                break;
            case MovementType.None:
                _movement = null;
                break;
        }
        bool groundedType = (_movementType != MovementType.Flying);
        switch (_attackType)
        {
            case AttackType.Melee:
                _attack = new MeleeAttack(_enemyAtkAnim, _attackAudio, _audioVolume);
                break;
            case AttackType.Dash:
                _attack = new DashAttack(_dashPower, _dashDuration, _deceleration, groundedType, _enemyAtkAnim, _attackAudio, _audioVolume);
                break;
            case AttackType.None:
                _attack = null;
                break;
        }
    }

    private void Update()
    {
        CheckGround();
        if (_isStunned)
        {
            if (_movement != null) _movement.MoveToward(transform.position, 0f, _deceleration, _rb);
            return;
        }
        if (grounded && Mathf.Abs(_rb.linearVelocity.x) >= 0.1f)
        {
            _dustVFX.Play();
        }
        UpdateFacingDirection();
       switch (_currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Follow:
                Follow();
                break;
            case State.Attack:
                Attack();
                break;
        }
        HandleStates();
    }

    public virtual void ModifyStats(float speed, float attackSpd, float damage)
    {
        _chaseSpeed *= speed;
        _atkCd *= attackSpd; 
        _atkDamage *= damage;
    }


    // Idle for a few seconds before resuming patrol
    private void Idle()
    {
        if (_movement == null) return;
        if (_idleAudio != null && PlayerInAudioRange()) SoundFXManager.instance.PlaySoundFXClip(_idleAudio, transform, _audioVolume / 2);
        _movement.MoveToward(transform.position, 0f, _deceleration, _rb); // stop moving
        _idleTimer += Time.deltaTime;
        if (_idleTimer >= _idleTime)
        {
            _idleTimer = 0f;
            _currentState = State.Patrol;
        }
    }

    // Move between patrol points
    private void Patrol()
    {
        if (_movement == null) return;
        if (_patrolPoints == null || _patrolPoints.Length == 0) return; // no points to patrol
        if (_followAudio != null && PlayerInAudioRange()) SoundFXManager.instance.PlaySoundFXClip(_followAudio, transform, _audioVolume / 2);
        Transform targetPoint = _patrolPoints[_patrolIndex];
        _movement.MoveToward(targetPoint.position, _patrolSpeed, _acceleration, _rb);

        if (Vector2.Distance(transform.position, targetPoint.position) < _stoppingDistance)
        {
            _patrolIndex = (_patrolIndex + 1) % _patrolPoints.Length;
            _currentState = State.Idle;
        }
    }

    // Follows the player
    protected virtual void Follow()
    {
        if (_movement == null) return;
        if (_followAudio != null && PlayerInAudioRange()) SoundFXManager.instance.PlaySoundFXClip(_followAudio, transform, _audioVolume);
        _movement.MoveToward(_player.position, _patrolSpeed, _acceleration, _rb);
    }

    // Attacks the player
    protected virtual void Attack()
    {
        if (_attack != null)
        {
            if (!_attack.rbRestricted && _movement != null) _movement.MoveToward(transform.position, 0f, _deceleration, _rb); // stop moving
            if (_followAudio != null && PlayerInAudioRange()) SoundFXManager.instance.PlaySoundFXClip(_followAudio, transform, _audioVolume);
            StartCoroutine(_attack.AttackPlayer(_player, _atkDamage, _atkWindup, _atkCd, _rb));
        }
        else Idle();
    }

    public IEnumerator Stun(float duration)
    {
        if (_isStunned) yield break;
        _isStunned = true;
        yield return new WaitForSeconds(duration);
        _isStunned = false;
    }

    protected virtual void HandleStates()
    {
        float playerDistance = Vector2.Distance(transform.position, _player.position);
        if (playerDistance <= _attackRange) // attacks the player when in range
        {
            _currentState = State.Attack;
        }
        else if (playerDistance <= _detectionRange) // follows the player in range
        {
            _currentState = State.Follow;
        }
        else if (_currentState == State.Attack || _currentState == State.Follow) // else idle
        {
            _currentState = State.Idle;
        }
    }
    private void UpdateFacingDirection()
    {
        if ((_rb.linearVelocity.x > 0.1f && _isFacingLeft) || (_rb.linearVelocity.x < -0.1f && !_isFacingLeft))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1,
                transform.localScale.y, transform.localScale.z);
            _dustVFX.gameObject.transform.localScale = new Vector3(transform.localScale.x * -1,
                transform.localScale.y, transform.localScale.z);
            _isFacingLeft = !_isFacingLeft;
        }
    }

    private void CheckGround()
    {
        // Check if groundCheck collider overlaps with groundMask, return true if does
        grounded = Physics2D.OverlapAreaAll(_groundCheck.bounds.min, _groundCheck.bounds.max, LayerMask.GetMask("Ground")).Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    private bool PlayerInAudioRange()
    {
        float playerDistance = Vector2.Distance(transform.position, _player.position);
        if (playerDistance < _audioRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
