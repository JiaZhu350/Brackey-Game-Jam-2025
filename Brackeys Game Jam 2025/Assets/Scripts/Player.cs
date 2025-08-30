using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private UIController _uiController;

    [Header("Camera Stuff")]
    [SerializeField] private GameObject _cameraFollow;
    private CameraFollowObject _cameraFollowObject;


    //Movement Stats
    [Header("Movement Components")]
    [SerializeField] private LayerMask _whatIsGround;


    [Header("Movement Logic")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _speedModifierDivisor = 1f;
    [SerializeField] private float _minSpeed = 2.5f;
    private float _horizontalDirection;
    private float _speed;
    public float SpeedModifier { private set; get; }


    [Header("Jump Logic")]
    [SerializeField] private float _raycastLength;
    [SerializeField] private float _jumpForce;
    private bool _canJumpAgain;


    [Header("Dashing Logic")]
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    private bool _canDash;
    private bool _isDashing;


    //Combat Stats

    [Header("Combat Components")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _whatIsEnemy;


    [Header("Health")]
    [SerializeField] private float _baseMaxHeath;
    [SerializeField] private float _invincibleCooldown;
    private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _minHealth = 50.0f;
    [SerializeField] private float _healthModDivisor = 1f;
    private bool _isInvincible;
    public float HealthModifier { private set; get; } // Reference to modified max health rather than current health


    [Header("Attack")]
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _minDamage = 5f;
    [SerializeField] private float _damageModDivisor = 1f;
    private bool _canAttack;
    public float DamageModifier { private set; get; }


    [Header("Resistance")]
    [SerializeField] private float _baseResistance;
    [SerializeField] private float _minResistance = -30f;
    [SerializeField] private float _resistanceModDivisor = 1f;
    public float ResistanceModifier { private set; get; }

    [Header("Inventory")]
    [SerializeField] private int _biscuitAmount = 0;

    public bool _isFacingRight; //Sprite flip logic
    private bool _isPlayerDead;


    private void Awake()
    {
        ResetAllModifiers();
        ResetStats();
        _uiController.UpdateBiscuitUI();
    }

    private void Start()
    {
        _cameraFollowObject = _cameraFollow.GetComponent<CameraFollowObject>();
    } 

    private void Update()
    {
        if (_isDashing || _isPlayerDead) { return; }

        _horizontalDirection = Input.GetAxisRaw("Horizontal");

        if (Keyboard.current.spaceKey.wasPressedThisFrame && CanJump())
        {
            PlayerJump();
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame && _canDash)
        {
            StartCoroutine(Dashing());
        }

        if (Mouse.current.leftButton.isPressed && _canAttack)
        {
            StartCoroutine(Attacking());
        }
    }

    void FixedUpdate()
    {
        if (_isDashing || _isPlayerDead) { return; }

        _rigidbody.linearVelocity = new Vector2(_horizontalDirection * Mathf.Max(_speed + SpeedModifier / _speedModifierDivisor, _minSpeed), _rigidbody.linearVelocityY); //Player movement

        FlippingPlayer();
    }

    private void ResetStats()
    {
        _isPlayerDead = false;
        _isInvincible = false;
        _isFacingRight = true;
        _isDashing = false;
        
        _canJumpAgain = false;
        _canDash = true;
        _canAttack = true;

        _speed = _normalSpeed;
        _currentHealth = _maxHealth = _baseMaxHeath + Mathf.Max(HealthModifier,_minHealth - _baseMaxHeath);
    }


    //Movement System
    private void FlippingPlayer()
    {
        if (_isFacingRight && _horizontalDirection < 0 || !_isFacingRight && _horizontalDirection > 0)
        {
            Vector3 localScale = transform.localScale;
            if (_isFacingRight)
            {
                Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.x);
                transform.rotation = Quaternion.Euler(rotator);

                // turn the camera follow object
                _cameraFollowObject.CallTurn();
            }
            else
            {
                Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.x);
                transform.rotation = Quaternion.Euler(rotator);

                // turn the camera follow object
                _cameraFollowObject.CallTurn();
            }
            _isFacingRight = !_isFacingRight;
            // localScale.x *= -1f;
            // transform.localScale = localScale;
        }
    }

    private bool CanJump()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _raycastLength, _whatIsGround) || _canJumpAgain;
    }

    private void PlayerJump()
    {
        _rigidbody.linearVelocityY = 0f; // Reset the linear Y velocity to allow for better jumping logic
        _rigidbody.AddForce(transform.up * _jumpForce);
        _canJumpAgain = !_canJumpAgain;
    }

    private IEnumerator Dashing()
    {
        _isDashing = true;
        _canDash = false;
        float originalGravity = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0f; //Makes gravity unable to affect the Dash
        if (_isFacingRight)
        {
            _rigidbody.linearVelocity = new Vector2(Mathf.Max(_dashSpeed + SpeedModifier / _speedModifierDivisor, _minSpeed), 0f); //Dashing Speed
        }
        else
        {
            _rigidbody.linearVelocity = new Vector2(-1 * Mathf.Max(_dashSpeed + SpeedModifier/_speedModifierDivisor, _minSpeed), 0f); //Dashing Speed
        }
        
        yield return new WaitForSeconds(_dashTime);
        _rigidbody.gravityScale = originalGravity;
        _isDashing = false;

        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }


    //Combat Systems
    private IEnumerator Attacking()
    {
        _canAttack = false;
        Collider2D[] _hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _whatIsEnemy);

        foreach (Collider2D _enemy in _hitEnemies)
        {
            Debug.Log(_enemy.name + " is hit for " + Mathf.Max(_attackDamage + DamageModifier/_damageModDivisor, _minDamage) + " damage"); //Logic for dealing damage
            _enemy.GetComponent<EnemyHealth>().TakeDamage(Mathf.Max(_attackDamage + DamageModifier/_damageModDivisor, _minDamage), transform);
        }

        yield return new WaitForSeconds(_attackCooldown);

        _canAttack = true;
    }

    public void TakeDamage(float _damageTaken)
    {
        if (_isInvincible){ return; }

        Debug.Log($"Player took {_damageTaken} dmg");

        float resistanceCalculation = Mathf.Max(_baseResistance + ResistanceModifier/_resistanceModDivisor, _minResistance) / 100f;
        if (resistanceCalculation > 0.9f)
        {
            resistanceCalculation = 0.9f;
        }

        _currentHealth -= _damageTaken * (1 - resistanceCalculation);


        StartCoroutine(InvincibleTimer());

        if (_currentHealth <= 0)
        {
            PlayDeath();
        }
    }

    public void PlayerHeal(float _amountHealed)
    {
        _currentHealth += _amountHealed;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    private IEnumerator InvincibleTimer()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(_invincibleCooldown);
        _isInvincible = false;
    }

    private void PlayDeath()
    {
        _isPlayerDead = true;
        Debug.Log("Player died");
        // Player Death Animation
    }

    public void ModifyBiscuit(int amount)
    {
        _biscuitAmount += amount;
        if (_biscuitAmount < 0) _biscuitAmount = 0;
        _uiController.UpdateBiscuitUI();
    }

    public int GetBiscuit()
    {
        return _biscuitAmount;
    }

    //Modified Values
    private void ResetAllModifiers()
    {
        SpeedModifier = 0;
        HealthModifier = 0;
        DamageModifier = 0;
        ResistanceModifier = 0;
    }

    public void AddSpeed(float _modValue)
    {
        SpeedModifier += _modValue;
    }

    public void AddHealth(float _modValue)
    {
        _modValue /= _healthModDivisor;
        HealthModifier += _modValue;

        if (_modValue < 0)
        {
            _maxHealth += _modValue;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
        else
        {
            _maxHealth += _modValue;
            _currentHealth += _modValue;
        }
        if (_maxHealth < _minHealth)
        {
            _currentHealth = _maxHealth = _minHealth;
        }
    }

    public void AddDamage(float _modValue)
    {
        DamageModifier += _modValue;
    }

    public void AddResistance(float _modValue)
    {
        ResistanceModifier += _modValue;
    }
}


