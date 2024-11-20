using UnityEngine;

namespace Pickups
{
    public class SharksTooth : MonoBehaviour
    {
        public float attackRateIncrease = 0.1f;
        
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.attackCooldown -= attackRateIncrease;
                Destroy(gameObject);
            }
        }
    }
}