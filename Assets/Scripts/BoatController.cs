using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public bool flip;
    
    private SpriteRenderer _sr;
    private Animator _anim;
    private bool _empty = true;
    
    private readonly int _animBoatIdle = Animator.StringToHash("anim_boat_idle");
    private readonly int _animBoatRow = Animator.StringToHash("anim_boat_row");
    
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.flipX = flip;
    }
    
    void Update()
    {
        UpdateAnimation();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _empty = false;
        }
    }

    private void UpdateAnimation()
    {
        if (_empty)
        {
            _anim.CrossFade(_animBoatIdle, 0);
        }
        else
        {
            _anim.CrossFade(_animBoatRow, 0);
        }
    }
}
