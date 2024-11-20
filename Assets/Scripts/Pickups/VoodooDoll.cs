using UnityEngine;

namespace Pickups
{
    public class VoodooDoll : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.lives += 1;
                Destroy(gameObject);
            }
        }
    }
}