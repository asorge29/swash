using UnityEngine;

namespace Pickups
{
    public class Coin : MonoBehaviour
    {
        public int value = 1;
        
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.coins += value;
                Destroy(gameObject);
            }
        }
    }
}