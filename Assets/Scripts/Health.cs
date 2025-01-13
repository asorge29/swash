using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startingHealth = 100;
    
    private float _health;

    private void Start()
    {
        _health = startingHealth;
    }

    private void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }
}