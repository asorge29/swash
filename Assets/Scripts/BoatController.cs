using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BoatController : MonoBehaviour
{
    public bool flip;
    public float speed = 0.02f;
    public float delay = 3f;
    
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    
    private bool _empty = true;
    private float _enteredTime;
    
    private readonly int _animBoatIdle = Animator.StringToHash("anim_boat_idle");
    private readonly int _animBoatRow = Animator.StringToHash("anim_boat_row");
    
    void Start()
    {
        sr.flipX = flip;
    }
    
    void Update()
    {
        UpdateAnimation();
        if (!_empty)
        {
            Move();
            ChangeScene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _empty = false;
            var controller = other.GetComponent<PlayerController>();
            controller.EnterBoat();
            _enteredTime = Time.time;
        }
    }

    private void ChangeScene()
    {
        if (Time.time - _enteredTime >= delay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    private void Move()
    {
        Vector3 newPos = transform.position;
        if (flip)
        {
            newPos.x -= speed;
        }
        else
        {
            newPos.x += speed;
        }
        transform.position = newPos;
    }

    private void UpdateAnimation()
    {
        if (_empty)
        {
            anim.CrossFade(_animBoatIdle, 0);
        }
        else
        {
            anim.CrossFade(_animBoatRow, 0);
        }
    }
}
