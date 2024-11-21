using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public int lives = 3;
    public int maxHealth = 100;
    public float regenerationRate = 0.5f;
    
    public float moveSpeed = 250f;
    public bool dashAbility = false;

    public float attackCooldown = 0.5f;
    public float attackRange = 0.5f;
    public float knockbackForce = 10f;
    public float damage = 20f;
    public float damageMultiplier = 1f;
    public bool visible = true;

    public float coins = 0f;
    public float goldEarnRate = 1f;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [HideInInspector] public int health;

    private bool _canDash;
    private bool _dashCdown;
    private float _lastAttack;

    private List<EnemyAi> _enemiesInRange = new List<EnemyAi>();

    private string _facing = "side";

    private Vector2 _moveDir = Vector2.zero;

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
            var enemyAI = collision.gameObject.GetComponent<EnemyAi>();
            if (enemyAI != null && !_enemiesInRange.Contains(enemyAI))
            {
                _enemiesInRange.Add(enemyAI);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Enemy"))
        {
            var enemyAI = collision.gameObject.GetComponent<EnemyAi>();
            if (enemyAI != null && _enemiesInRange.Contains(enemyAI))
            {
                _enemiesInRange.Remove(enemyAI);
            }
        }
    }
    
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void MovementUpdate()
    {
        _rb.velocity = _moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
    }

    private void Attack()
    {
        if (Time.time > attackCooldown + _lastAttack)
        {
            _lastAttack = Time.time;
            if (_enemiesInRange.Count > 0)
            {
                foreach (var e in _enemiesInRange)
                {
                    e.health -= (damage * damageMultiplier);
                    Rigidbody2D erb = e.GetComponent<Rigidbody2D>();
                    print(erb);
                    Vector2 knockbackDirection = (erb.position - _rb.position).normalized;
                    erb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void UpdateAnimation()
    {
        if (_moveDir.x != 0)
        {
            if (_moveDir.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (_moveDir.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
        }

        if (_moveDir.sqrMagnitude > 0)
        {
            if (_moveDir.x == 0)
            {
                if (_moveDir.y < 0)
                {
                    _facing = "down";
                    _animator.CrossFade(_animMoveDown, 0);
                }
                else
                {
                    _facing = "up";
                    _animator.CrossFade(_animMoveUp, 0);
                }
            }
            else
            {
                _facing = "side";
                _animator.CrossFade(_animMoveRight, 0);
            }
        }
        else
        {
            switch (_facing)
            {
                case "up":
                    _animator.CrossFade(_animIdleUp, 0);
                    break;
                case "down":
                    _animator.CrossFade(_animIdleDown, 0);
                    break;
                default:
                    _animator.CrossFade(_animIdleRight, 0);
                    break;
            }
        }
    }
}