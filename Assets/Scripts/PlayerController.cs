using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    //TODO: fix the region and enum bullshit
    //TODO: vertical and horizontal walking animation
    //TODO: attack
    //TODO: health
    //TODO: pickups
    //TODO: Talk to archer

    
    #region Enums
    
    private enum Directions { UP, DOWN, LEFT, RIGHT }
    
    #endregion
    
    #region Editor Data
    
    [Header("Movement Attributes")]
    [SerializeField] public float moveSpeed = 50f;
    
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    
    #endregion
    
    #region Internal Data
    
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;
    
    private readonly int _animMoveRight = Animator.StringToHash("anim_player_move_right");
    private readonly int _animIdleRight = Animator.StringToHash("anim_player_idle_right");
    
    #endregion
    
    #region Tick

    private void Update()
    {
        GatherInput();
        CalculateFacingDirection();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }
    
    #endregion
    
    #region Input Logic

    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }
    
    #endregion
    
    #region Movement Logic

    private void MovementUpdate()
    {
        _rb.velocity = _moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
    }
    
    #endregion
    
    #region Animation Logic

    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
        {
            if (_moveDir.x > 0) // moving right
            {
                _facingDirection = Directions.RIGHT;
            }
            else if (_moveDir.x < 0) // moving left
            {
                _facingDirection = Directions.LEFT;
            }
        }
    }

    private void UpdateAnimation()
    {
        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        if (_moveDir.sqrMagnitude > 0) // moving
        {
            _animator.CrossFade(_animMoveRight, 0);
        }
        else
        {
            _animator.CrossFade(_animIdleRight, 0);
        }
    }
    
    #endregion
    
}