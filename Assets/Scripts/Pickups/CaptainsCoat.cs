using UnityEngine;

namespace Pickups
{
    public class CaptainsCoat : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.damageMultiplier *= 2;
                Destroy(gameObject);
            }
        }
    }
}