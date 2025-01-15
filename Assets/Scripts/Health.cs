using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startingHealth = 100;
    public bool anchor;
    
    public GameObject lootPrefab;
    
    private float _health;
    private bool _anchored;

    private void Start()
    {
        _health = startingHealth;
    }

    private void Update()
    {
        if (_health <= 0)
        {
            if (lootPrefab)
            {
                Instantiate(lootPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, string from = "")
    {
        if (_anchored) return;
        if (anchor && from != "grenade") return;
        _health -= damage;
    }
}