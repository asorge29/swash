using UnityEngine;

namespace Pickups
{
    public class SpeedPickup : MonoBehaviour
    {
        public float speed = 50f;
        
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.moveSpeed += speed;
                Destroy(gameObject);
            }
        }
    }
}