using UnityEngine;

namespace Pickups
{
    public class RumBottle : MonoBehaviour
    {
        public float speedIncrease = 2f;
        
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.moveSpeed += speedIncrease;
                Destroy(gameObject);
            }
        }
    }
}