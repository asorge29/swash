using System;
using UnityEngine;
//color is 168 168 168 105
[SelectionBase]
public class AnchoredEnemyController : MonoBehaviour
{
    public float health = 100;

    public float moveSpeed = 3f;
    public float acceleration = 50f;

    public float detectRange = 6f;

    public int attackDamage = 5;
    public float attackCooldown = 0.5f;
    public float damageMultiplier = 1f;
    
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _bodyAnimator;
    [SerializeField] Animator _shirtAnimator;
    [SerializeField] Animator _hairAnimator;
    [SerializeField] SpriteRenderer _shirtRenderer;
    [SerializeField] SpriteRenderer _hairRenderer;
    [SerializeField] SpriteRenderer _bodyRenderer;
    [SerializeField] AnchoredSpirit _anchoredSpirit;

    private float _lastAttack;

    [HideInInspector] public bool knockedBack;
    [HideInInspector] public float knockbackTimer;

    private Facing _facing = Facing.Right;
    private float _facingAngle;

    private GameObject _player;
    private PlayerController _playerController;
    private Vector2 _moveDir = Vector2.zero;
    
    private Color _spiritColor;
    private bool _anchored;
    
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

    private readonly int _animShirtMoveRight = Animator.StringToHash("anim_enemy_shirt_walk_right");
    private readonly int _animShirtIdleRight = Animator.StringToHash("anim_enemy_shirt_idle_right");
    private readonly int _animShirtMoveLeft = Animator.StringToHash("anim_enemy_shirt_walk_left");
    private readonly int _animShirtIdleLeft = Animator.StringToHash("anim_enemy_shirt_idle_left");
    private readonly int _animShirtMoveUp = Animator.StringToHash("anim_enemy_shirt_walk_up");
    private readonly int _animShirtIdleUp = Animator.StringToHash("anim_enemy_shirt_idle_up");
    private readonly int _animShirtMoveDown = Animator.StringToHash("anim_enemy_shirt_walk_down");
    private readonly int _animShirtIdleDown = Animator.StringToHash("anim_enemy_shirt_idle_down");

    private readonly int _animHairMoveRight = Animator.StringToHash("anim_enemy_hair_walk_right");
    private readonly int _animHairIdleRight = Animator.StringToHash("anim_enemy_hair_idle_right");
    private readonly int _animHairMoveLeft = Animator.StringToHash("anim_enemy_hair_walk_left");
    private readonly int _animHairIdleLeft = Animator.StringToHash("anim_enemy_hair_idle_left");
    private readonly int _animHairMoveUp = Animator.StringToHash("anim_enemy_hair_walk_up");
    private readonly int _animHairIdleUp = Animator.StringToHash("anim_enemy_hair_idle_up");
    private readonly int _animHairMoveDown = Animator.StringToHash("anim_enemy_hair_walk_down");
    private readonly int _animHairIdleDown = Animator.StringToHash("anim_enemy_hair_idle_down");

    #endregion

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _spiritColor = new Color(_anchoredSpirit.soulColor.r, _anchoredSpirit.soulColor.g, _anchoredSpirit.soulColor.b, 1);
        _bodyRenderer.color = _spiritColor;
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

        if (_anchored)
        {
            _anchored = _anchoredSpirit.CheckAnchored();
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
        _playerController.TakeDamage(attackDamage * damageMultiplier);
            
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
                    _shirtAnimator.CrossFade(_animShirtMoveUp, 0);
                    _hairAnimator.CrossFade(_animHairMoveUp, 0);
                    break;
                case Facing.Down:
                    _bodyAnimator.CrossFade(_animBodyMoveDown, 0);
                    _shirtAnimator.CrossFade(_animShirtMoveDown, 0);
                    _hairAnimator.CrossFade(_animHairMoveDown, 0);
                    break;
                case Facing.Right:
                    _bodyAnimator.CrossFade(_animBodyMoveRight, 0);
                    _shirtAnimator.CrossFade(_animShirtMoveRight, 0);
                    _hairAnimator.CrossFade(_animHairMoveRight, 0);
                    break;
                case Facing.Left:
                    _bodyAnimator.CrossFade(_animBodyMoveLeft, 0);
                    _shirtAnimator.CrossFade(_animShirtMoveLeft, 0);
                    _hairAnimator.CrossFade(_animHairMoveLeft, 0);
                    break;
            }
        }
        else
        {
            switch (_facing)
            {
                case Facing.Up:
                    _bodyAnimator.CrossFade(_animBodyIdleUp, 0);
                    _shirtAnimator.CrossFade(_animShirtIdleUp, 0);
                    _hairAnimator.CrossFade(_animHairIdleUp, 0);
                    break;
                case Facing.Down:
                    _bodyAnimator.CrossFade(_animBodyIdleDown, 0);
                    _shirtAnimator.CrossFade(_animShirtIdleDown, 0);
                    _hairAnimator.CrossFade(_animHairIdleDown, 0);
                    break;
                case Facing.Right:
                    _bodyAnimator.CrossFade(_animBodyIdleRight, 0);
                    _shirtAnimator.CrossFade(_animShirtIdleRight, 0);
                    _hairAnimator.CrossFade(_animHairIdleRight, 0);
                    break;
                case Facing.Left:
                    _bodyAnimator.CrossFade(_animBodyIdleLeft, 0);
                    _shirtAnimator.CrossFade(_animShirtIdleLeft, 0);
                    _hairAnimator.CrossFade(_animHairIdleLeft, 0);
                    break;
            }
        }
    }
}