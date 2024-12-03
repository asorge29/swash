using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public int lives = 3;
    public int maxHealth = 100;
    public float regenerationRate = 0.5f;

    public float moveSpeed = 5f;
    public float acceleration = 50f;
    public bool dashAbility = false;

    public float attackCooldown = 0.5f;
    public float attackRange = 0.5f;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    public float damage = 20f;
    public float damageMultiplier = 1f;
    public bool visible = true;

    public float coins = 0f;
    public float goldEarnRate = 1f;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] AudioSource _audioSource;

    [HideInInspector] public float health;

    private bool _canDash;
    private bool _dashCdown;
    private float _lastAttack;

    private List<GameObject> _enemiesInRange = new List<GameObject>();

    private Facing _facing = Facing.Left;
    private Vector3 _mousePos;
    private Vector2 _moveDir = Vector2.zero;
    private float _facingAngle = 0f;

    private enum Facing
    {
        Up,
        Down,
        Left,
        Right
    }

    #region Animation References

    private readonly int _animMoveRight = Animator.StringToHash("anim_player_move_right");
    private readonly int _animIdleRight = Animator.StringToHash("anim_player_idle_right");
    private readonly int _animMoveUp = Animator.StringToHash("anim_player_move_up");
    private readonly int _animIdleUp = Animator.StringToHash("anim_player_idle_up");
    private readonly int _animMoveDown = Animator.StringToHash("anim_player_move_down");
    private readonly int _animIdleDown = Animator.StringToHash("anim_player_idle_down");

    #endregion

    private void Start()
    {
        health = maxHealth;
        _lastAttack = Time.time - attackCooldown;
    }

    private void Update()
    {
        CheckDead();
        Escape();
        GatherInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
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

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        if (health <= 0)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
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

    private void Attack()
    {
        if (!(Time.time > attackCooldown + _lastAttack)) return;
        if (_enemiesInRange.Count <= 0) return;
        _lastAttack = Time.time;
        _audioSource.Play();
        foreach (var e in _enemiesInRange)
        {
            var erb = e.GetComponent<Rigidbody2D>();
            
            var angle = Vector2.SignedAngle(Vector2.right, erb.position - (Vector2)transform.position);

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
            
            var enemyAi = erb.GetComponent<EnemyAi>();

            enemyAi.health -= damage * damageMultiplier;

            Vector2 knockbackDirection = (erb.position - _rb.position).normalized;
            enemyAi.knockedBack = true;
            enemyAi.knockbackTimer = knockbackTime;
            erb.AddForce(knockbackDirection * (knockbackForce + _rb.velocity.magnitude), ForceMode2D.Impulse);
        }
    }

    private void UpdateAnimation()
    {
        if (_facing is Facing.Left or Facing.Right)
        {
            _spriteRenderer.flipX = _facing != Facing.Right;
        }

        if (_moveDir.sqrMagnitude > 0)
        {
            switch (_facing)
            {
                case Facing.Up:
                    _animator.CrossFade(_animMoveUp, 0);
                    break;
                case Facing.Down:
                    _animator.CrossFade(_animMoveDown, 0);
                    break;
                default:
                    _animator.CrossFade(_animMoveRight, 0);
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
                default:
                    _animator.CrossFade(_animIdleRight, 0);
                    break;
            }
        }
    }
}