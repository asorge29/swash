using System;
using UnityEngine;

[SelectionBase]
public class EnemyAi : MonoBehaviour
{
    //TODO: attack
    
    public float health = 100;
    
    public float moveSpeed = 3f;
    public float acceleration = 50f;

    public float detectRange = 6f;
    
    public int attackDamage = 5;
    public float attackCooldown = 0.5f;
    public float attackRange = 0.5f;
    public float damageMultiplier = 1f;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    private bool _attackCdown;

    private string _facing = "side";
    
    private GameObject _player;

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
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CheckDead();
        UpdateAnimation();
        TrackPlayer();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void CheckDead()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void TrackPlayer()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        
        if (distance < detectRange && distance > 0.8)
        {
            _moveDir = (_player.transform.position - transform.position).normalized;
        }
        else
        {
            _moveDir = Vector2.zero;
        }
    }
    
    private void MovementUpdate()
    {
        Vector2 force = _moveDir * acceleration;
        _rb.AddForce(force);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, moveSpeed);
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