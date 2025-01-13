using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAI: MonoBehaviour
{
    public float detectRange = 6f;
    
    public float moveSpeed = 2f;
    public float acceleration = 50f;

    public int attackDamage = 5;
    public float attackCooldown = 0.5f;
    public float damageMultiplier = 1f;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator shirtAnimator;
    [SerializeField] private Animator hairAnimator;

    private float _lastAttack;
    private bool _knockedBack;
    private float _knockBackTimer;
    private Facing _facing = Facing.Down;
    private float _facingAngle;

    private GameObject _player;
    private PlayerController _playerController;
    private Vector2 _moveDir = Vector2.zero;
    
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
    }
    
    private void Update()
    {
        TrackPlayer();
        UpdateAnimation();
    }
    
    private void FixedUpdate()
    {
        MovementUpdate();
    }
    
    private void MovementUpdate()
    {
        if (_knockedBack)
        {
            if (_knockBackTimer > 0f)
            {
                _knockBackTimer -= Time.deltaTime;
            }
            else
            {
                _knockedBack = false;
            }
        }
        else
        {
            Vector2 force = _moveDir * acceleration;
            rb.AddForce(force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, moveSpeed);
        }
    }
    
    private void TrackPlayer()
    {
        var distance = Vector2.Distance(transform.position, _player.transform.position);

        if (distance < detectRange)
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
    
    private void UpdateAnimation()
    {
        if (_moveDir.sqrMagnitude > 0)
        {
            switch (_facing)
            {
                case Facing.Up:
                    bodyAnimator.CrossFade(_animBodyMoveUp, 0);
                    shirtAnimator.CrossFade(_animShirtMoveUp, 0);
                    hairAnimator.CrossFade(_animHairMoveUp, 0);
                    break;
                case Facing.Down:
                    bodyAnimator.CrossFade(_animBodyMoveDown, 0);
                    shirtAnimator.CrossFade(_animShirtMoveDown, 0);
                    hairAnimator.CrossFade(_animHairMoveDown, 0);
                    break;
                case Facing.Right:
                    bodyAnimator.CrossFade(_animBodyMoveRight, 0);
                    shirtAnimator.CrossFade(_animShirtMoveRight, 0);
                    hairAnimator.CrossFade(_animHairMoveRight, 0);
                    break;
                case Facing.Left:
                    bodyAnimator.CrossFade(_animBodyMoveLeft, 0);
                    shirtAnimator.CrossFade(_animShirtMoveLeft, 0);
                    hairAnimator.CrossFade(_animHairMoveLeft, 0);
                    break;
            }
        }
        else
        {
            switch (_facing)
            {
                case Facing.Up:
                    bodyAnimator.CrossFade(_animBodyIdleUp, 0);
                    shirtAnimator.CrossFade(_animShirtIdleUp, 0);
                    hairAnimator.CrossFade(_animHairIdleUp, 0);
                    break;
                case Facing.Down:
                    bodyAnimator.CrossFade(_animBodyIdleDown, 0);
                    shirtAnimator.CrossFade(_animShirtIdleDown, 0);
                    hairAnimator.CrossFade(_animHairIdleDown, 0);
                    break;
                case Facing.Right:
                    bodyAnimator.CrossFade(_animBodyIdleRight, 0);
                    shirtAnimator.CrossFade(_animShirtIdleRight, 0);
                    hairAnimator.CrossFade(_animHairIdleRight, 0);
                    break;
                case Facing.Left:
                    bodyAnimator.CrossFade(_animBodyIdleLeft, 0);
                    shirtAnimator.CrossFade(_animShirtIdleLeft, 0);
                    hairAnimator.CrossFade(_animHairIdleLeft, 0);
                    break;
            }
        }
    }
}