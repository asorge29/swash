using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float damage = 15f;
    public float timer = 1f;

    public Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private readonly int _animExplode = Animator.StringToHash("anim_grenade_explode");
    private float _stopTime = 0f;
    private bool _stopped;
    private List<GameObject> _enemiesInRange = new();

    void Update()
    {
        if (!_stopped)
        {
            if (rb.velocity.sqrMagnitude < 1f && _stopTime == 0f)
            {
                _stopTime = Time.time;
                _stopped = true;
            }
            else
            {
                _stopTime = Time.time;
            }
        }
        
        if (Time.time >= _stopTime + timer)
        {
            Explode();
        }
    }

    private void Explode()
    {
        foreach (var e in _enemiesInRange)
        {
            var health = e.GetComponent<Health>();

            health.TakeDamage(damage, "grenade");
        }

        animator.CrossFade(_animExplode, 0);
        //Destroy(gameObject);
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
        else if (collision.gameObject.CompareTag("Anchor"))
        {
            var anchor = collision.gameObject;
            if (anchor != null && !_enemiesInRange.Contains(anchor))
            {
                _enemiesInRange.Add(anchor);
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
        else if (collision.gameObject.CompareTag("Anchor"))
        {
            var anchor = collision.gameObject;
            if (anchor != null && _enemiesInRange.Contains(anchor))
            {
                _enemiesInRange.Remove(anchor);
            }
        }
    }
}