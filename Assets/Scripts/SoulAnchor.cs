using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAnchor : MonoBehaviour
{
    public float health = 1f;
    
    public AnchoredSpirit AnchoredSpirit;
    public SpriteRenderer SpriteRenderer;
    
    private Color soulColor;
    private void Start()
    {
        soulColor = new Color(AnchoredSpirit.soulColor.r, AnchoredSpirit.soulColor.g, AnchoredSpirit.soulColor.b, 1f);
        SpriteRenderer.color = soulColor;
    }
    
    private void Update()
    {
        CheckDead();
    }

    private void CheckDead()
    {
        if (health <= 0)
        {
            AnchoredSpirit.anchored = false;
            Destroy(gameObject);
        }
    }
}
