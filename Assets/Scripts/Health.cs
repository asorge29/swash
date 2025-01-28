using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Health : MonoBehaviour
{
    public int startingHealth = 100;
    public bool anchor;
    
    public List<GameObject> lootPrefabs;    
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
            if (lootPrefabs.Count > 0)
            {
                var rnd = new Random();
                var i = rnd.Next(lootPrefabs.Count);
                Instantiate(lootPrefabs[i], transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void NotAnchored()
    {
        _anchored = false;
    }
    
    public void MakeAnchored()
    {
        _anchored = true;
    }
        

    public void TakeDamage(float damage, string from = "")
    {
        if (_anchored) return;
        if (anchor && from != "grenade") return;
        _health -= damage;
    }
}