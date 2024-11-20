using UnityEngine;

namespace Pickups
{
    public class SharksTooth : MonoBehaviour
    {
        public float attackRateIncrease = 5f;
        
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.moveSpeed += speedIncrease; //FIXME
                Destroy(gameObject);
            }
        }
    }
}