using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public int startingHealth = 100;
    public float moveSpeed = 5f;
    public float acceleration = 50f;

    public float attackCooldown = 0.5f;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    public float damage = 20f;
    public float damageMultiplier = 1f;

    public float coins = 0f;
    public float goldEarnRate = 1f;
    
    public GameObject grenadePrefab;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    //[SerializeField] AudioSource _audioSource;

    private float _health;
    private float _lastAttack;

    private GameObject _boat;
    private bool _inBoat;
    private BoatController _boatController;

    private readonly List<GameObject> _enemiesInRange = new();

    private Facing _facing = Facing.Left;
    private Vector3 _mousePos;
    private Vector2 _moveDir = Vector2.zero;
    private float _facingAngle;

    private bool _enabled = true;

    private enum Facing
    {
        Up,
        Down,
        Left,
        Right
    }

    #region Animation References

    private readonly int _animMoveRight = Animator.StringToHash("anim_erik_walk_right");
    private readonly int _animIdleRight = Animator.StringToHash("anim_erik_idle_right");
    private readonly int _animMoveLeft = Animator.StringToHash("anim_erik_walk_left");
    private readonly int _animIdleLeft = Animator.StringToHash("anim_erik_idle_left");
    private readonly int _animMoveUp = Animator.StringToHash("anim_erik_walk_up");
    private readonly int _animIdleUp = Animator.StringToHash("anim_erik_idle_up");
    private readonly int _animMoveDown = Animator.StringToHash("anim_erik_walk_down");
    private readonly int _animIdleDown = Animator.StringToHash("anim_erik_idle_down");

    #endregion

    private void Start()
    {
        _lastAttack = Time.time - attackCooldown;
        _health = startingHealth;
        _boat = GameObject.FindGameObjectWithTag("Boat");
        if (_boat) _boatController = _boat.GetComponent<BoatController>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnPoint != null)
        {
            gameObject.transform.position = respawnPoint.transform.position;
        }
        _enabled = true;
        _inBoat = false;
    }
    
    private void Update()
    {
        CheckDead();
        Escape();
        if (_enabled)
        {
            GatherInput();
        }
        else if (_inBoat)
        {
            RideBoat();
        }
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    public float GetHealth()
    {
        return _health;
    }
    
    public void TakeDamage(float incomingDamage)
    {
        _health -= incomingDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;
            if (enemy != null && !_enemiesInRange.Contains(enemy))
            {
                _enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;
            if (enemy != null && _enemiesInRange.Contains(enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }
    }

    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        if (Camera.main != null) _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _facingAngle = Vector2.SignedAngle(Vector2.right, (Vector2)_mousePos - (Vector2)transform.position);

        _facing = _facingAngle switch
        {
            >= 0 and < 45 => Facing.Right,
            >= 45 and < 135 => Facing.Up,
            >= 135 and <= 180 => Facing.Left,
            < 0 and > -45 => Facing.Right,
            <= -45 and > -135 => Facing.Down,
            <= -135 and >= -180 => Facing.Left,
            _ => _facing
        };

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Attack();
        }
    }

    private void MovementUpdate()
    {
        Vector2 force = _moveDir * acceleration;
        _rb.AddForce(force);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, moveSpeed);
    }

    private void CheckDead()
    {
        if (_health > 0) return;
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
    }

    private static void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }

    private void RideBoat()
    {
        Vector3 newPosition = _boat.transform.position;
        newPosition.y += 0.5f;
        gameObject.transform.position = newPosition;
        _facing = _boatController.flip ? Facing.Left : Facing.Right;
    }

    public void EnterBoat()
    {
        _inBoat = true;
        _enabled = false;
    }

    private void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
        var grenadeRb = grenade.GetComponent<Rigidbody2D>();
        grenadeRb.velocity = Vector2.zero;
        var throwDirection = ((Vector2)_mousePos - (Vector2)transform.position).normalized;
        var throwForce = throwDirection * 5;
        grenadeRb.AddForce(throwForce, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        if (!(Time.time > attackCooldown + _lastAttack)) return;
        if (_enemiesInRange.Count <= 0) return;
        _lastAttack = Time.time;
        foreach (var e in _enemiesInRange)
        {
            var enemyAi = e.GetComponent<EnemyAI>();
            var enemyPos = (Vector2)e.transform.position;
            
            var angle = Vector2.SignedAngle(Vector2.right, enemyPos - (Vector2)transform.position);
//TODO: make this dependent on cursor rather than facing status
            switch (_facing)
            {
                case Facing.Up:
                    if (angle is < 45 or > 135) continue;
                    break;
                case Facing.Down:
                    if (angle is > -45 or < -135) continue;
                    break;
                case Facing.Left:
                    if (angle is < 135 or > -135) continue;
                    break;
                case Facing.Right:
                    if (angle is > 45 or < -45) continue;
                    break;
            } 
            
            var enemyHealth = e.GetComponent<Health>();
            if (enemyHealth.CheckAnchored()) return;
            enemyHealth.TakeDamage(damage * damageMultiplier);
            Vector2 knockBackDirection = (enemyPos - _rb.position).normalized;
            Vector2 knockBackForce = knockBackDirection * (knockbackForce + _rb.velocity.magnitude);
            enemyAi.TakeKnockback(knockBackForce, knockbackTime);
        }
    }

    //TODO: play swash animation on attack
    private void UpdateAnimation()
    {
        if (_moveDir.sqrMagnitude > 0 && !_inBoat)
        {
            switch (_facing)
            {
                case Facing.Up:
                    _animator.CrossFade(_animMoveUp, 0);
                    break;
                case Facing.Down:
                    _animator.CrossFade(_animMoveDown, 0);
                    break;
                case Facing.Right:
                    _animator.CrossFade(_animMoveRight, 0);
                    break;
                case Facing.Left:
                    _animator.CrossFade(_animMoveLeft, 0);
                    break;
            }
        }
        else
        {
            switch (_facing)
            {
                case Facing.Up:
                    _animator.CrossFade(_animIdleUp, 0);
                    break;
                case Facing.Down:
                    _animator.CrossFade(_animIdleDown, 0);
                    break;
                case Facing.Right:
                    _animator.CrossFade(_animIdleRight, 0);
                    break;
                case Facing.Left:
                    _animator.CrossFade(_animIdleLeft, 0);
                    break;
            }
        }
    }
}