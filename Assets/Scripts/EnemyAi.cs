using System;
using UnityEngine;

[SelectionBase]
public class EnemyAi : MonoBehaviour
{
    public float health = 100;

    public float moveSpeed = 3f;
    public float acceleration = 50f;

    public float detectRange = 6f;

    public int attackDamage = 5;
    public float attackCooldown = 0.5f;
    public float attackRange = 0.5f;
    public float damageMultiplier = 1f;
    
    public bool anchored = false;

    public Color hairColor;
    public Color coatColor;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _bodyAnimator;
    [SerializeField] Animator _clothesAnimator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] AnchoredSpirit _anchoredSpirit;

    private float _lastAttack;

    [HideInInspector] public bool knockedBack = false;
    [HideInInspector] public float knockbackTimer = 0f;

    private Facing _facing = Facing.Right;
    private float _facingAngle;

    private GameObject _player;
    private PlayerController _playerController;
    private Vector2 _moveDir = Vector2.zero;
    
    private Color _spiritColor;
    
    private enum Facing
    {
        Up,
        Down,
        Left,
        Right
    }

    #region Animation References

    private readonly int _animBodyMoveRight = Animator.StringToHash("anim_enemy_body_walk_right");
    private readonly int _animBodyIdleRight = Animator.StringToHash("anim_enemy_body_idle_right");
    private readonly int _animBodyMoveLeft = Animator.StringToHash("anim_enemy_body_walk_left");
    private readonly int _animBodyIdleLeft = Animator.StringToHash("anim_enemy_body_idle_left");
    private readonly int _animBodyMoveUp = Animator.StringToHash("anim_enemy_body_walk_up");
    private readonly int _animBodyIdleUp = Animator.StringToHash("anim_enemy_body_idle_up");
    private readonly int _animBodyMoveDown = Animator.StringToHash("anim_enemy_body_walk_down");
    private readonly int _animBodyIdleDown = Animator.StringToHash("anim_enemy_body_idle_down");

    #endregion

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        if (!anchored) return;
        _spiritColor = new Color(_anchoredSpirit.soulColor.r, _anchoredSpirit.soulColor.g, _anchoredSpirit.soulColor.b, 1);
        _spriteRenderer.color = _spiritColor;
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

        if (anchored)
        {
            anchored = _anchoredSpirit.anchored;
        }
    }

    private void TrackPlayer()
    {
        _facingAngle = Vector2.SignedAngle(Vector2.right, (Vector2)_player.transform.position - (Vector2)transform.position);
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
        
        var distance = Vector2.Distance(transform.position, _player.transform.position);

        if (distance < detectRange)
        {
            if (distance > 0.8)
            {
                _moveDir = (_player.transform.position - transform.position).normalized;
            }
            else
            {
                _moveDir = Vector2.zero;
                Attack();
            }
        }
        else
        {
            _moveDir = Vector2.zero;
        }
    }

    private void Attack()
    {
        if (!(Time.time > attackCooldown + _lastAttack)) return;
        
        _lastAttack = Time.time;
        _playerController.health -= attackDamage * damageMultiplier;
            
        //TODO: work out player knockback without stunlocking
        //Vector2 knockbackDirection = (erb.position - _rb.position).normalized;
        //enemyAi.knockedBack = true;
        //enemyAi.knockbackTimer = knockbackTime;
        //erb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void MovementUpdate()
    {
        if (knockedBack)
        {
            if (knockbackTimer > 0f)
            {
                knockbackTimer -= Time.deltaTime;
            }
            else
            {
                knockedBack = false;
            }
        }
        else
        {
            Vector2 force = _moveDir * acceleration;
            _rb.AddForce(force);
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, moveSpeed);
        }
    }

    //TODO: public take damage function
    
    private void UpdateAnimation()
    {
        if (_moveDir.sqrMagnitude > 0)
        {
            switch (_facing)
            {
                case Facing.Up:
                    _bodyAnimator.CrossFade(_animBodyMoveUp, 0);
                    break;
                case Facing.Down:
                    _bodyAnimator.CrossFade(_animBodyMoveDown, 0);
                    break;
                case Facing.Right:
                    _bodyAnimator.CrossFade(_animBodyMoveRight, 0);
                    break;
                case Facing.Left:
                    _bodyAnimator.CrossFade(_animBodyMoveLeft, 0);
                    break;
            }
        }
        else
        {
            switch (_facing)
            {
                case Facing.Up:
                    _bodyAnimator.CrossFade(_animBodyIdleUp, 0);
                    break;
                case Facing.Down:
                    _bodyAnimator.CrossFade(_animBodyIdleDown, 0);
                    break;
                case Facing.Right:
                    _bodyAnimator.CrossFade(_animBodyIdleRight, 0);
                    break;
                case Facing.Left:
                    _bodyAnimator.CrossFade(_animBodyIdleLeft, 0);
                    break;
            }
        }
    }
}