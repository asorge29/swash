using UnityEngine;

public class SoulAnchor : MonoBehaviour
{
    public float health = 1f;
    
    public AnchoredSpirit anchoredSpirit;
    public SpriteRenderer hazeRenderer;
    public SpriteRenderer stringRenderer;
    
    private Color _soulColor;
    private void Start()
    {
        _soulColor = new Color(anchoredSpirit.soulColor.r, anchoredSpirit.soulColor.g, anchoredSpirit.soulColor.b, 1f);
        hazeRenderer.color = _soulColor;
        stringRenderer.color = _soulColor;
    }
    
    private void Update()
    {
        CheckDead();
    }

    private void CheckDead()
    {
        if (health >= 0) return;
        anchoredSpirit.anchored = false;
        Destroy(gameObject);
    }
}
