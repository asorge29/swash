using UnityEngine;
using UnityEngine.Serialization;

public class EnemyColor : MonoBehaviour
{
    public Color hairColor = new Color(123, 24, 0);
    public Color shirtColor = new Color(229, 0, 0);
    
    [SerializeField] private SpriteRenderer shirtRenderer;
    [SerializeField] private SpriteRenderer hairRenderer;

    private void Start()
    {
        hairRenderer.color = new Color(hairColor.r, hairColor.g, hairColor.b, 1);
        shirtRenderer.color = new Color(shirtColor.r, shirtColor.g, shirtColor.b, 1);
    }
}