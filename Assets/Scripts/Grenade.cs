using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    
    public float damage = 15f;
    public float timer = 0.1f;

    public Rigidbody2D rb;
    
    private float _stopTime = 0f;
    private List<GameObject> _enemiesInRange = new();
    
    void Update()
    {
        if (rb.velocity.sqrMagnitude < 1f && _stopTime == 0f)
        {
            _stopTime = Time.time;
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
            var erb = e.GetComponent<Rigidbody2D>();
            var enemyAi = e.GetComponent<EnemyAi>();
            var anchor = e.GetComponent<SoulAnchor>();

            if (enemyAi && enemyAi.anchored) return;

            if (enemyAi)
            {
                enemyAi.health -= damage;
            }
            else if (anchor)
            {
                anchor.health -= damage;
            }
        }
        Destroy(gameObject);
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
