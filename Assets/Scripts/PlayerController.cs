using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    //TODO: fix the region and enum bullshit
    //TODO: vertical and horizontal walking animation
    //TODO: attack
    //TODO: pickups
    
    public float moveSpeed = 250f;
    public int maxHealth = 100;

    public float attackCooldown = 0.2f;
    public float attackRange = 0.5f;

    public bool visible = true;
    
    public bool dashAbility = false;
    
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [HideInInspector] public int health;

    private bool _canDash;
    private bool _dashCdown;
    private bool _attackCdown;
    
    private Vector2 _moveDir = Vector2.zero;
    
    private readonly int _animMoveRight = Animator.StringToHash("anim_player_move_right");
    private readonly int _animIdleRight = Animator.StringToHash("anim_player_idle_right");

    

    private void Update()
    {
        GatherInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }
    

    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void MovementUpdate()
    {
        _rb.velocity = _moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
    }
    
    private void UpdateAnimation()
    {
        if (_moveDir.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_moveDir.x > 0)
        {
            _spriteRenderer.flipX = false;
        }

        if (_moveDir.sqrMagnitude > 0)
        {
            _animator.CrossFade(_animMoveRight, 0);
        }
        else
        {
            _animator.CrossFade(_animIdleRight, 0);
        }
    }
    
    
}