using UnityEngine;
using UnityEngine.Serialization;

public class AnchoredEnemyColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shirtRenderer;
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private AnchoredSpirit anchoredSpirit;

    private void Start()
    {
        hairRenderer.color = new Color(anchoredSpirit.soulColor.r, anchoredSpirit.soulColor.g, anchoredSpirit.soulColor.b, 1);
        shirtRenderer.color = new Color(168, 168, 168, 105);
        bodyRenderer.color = new Color(168, 168, 168, 105);
    }
}