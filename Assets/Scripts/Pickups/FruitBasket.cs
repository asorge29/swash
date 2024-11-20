using UnityEngine;

namespace Pickups
{
    public class FruitBasket : MonoBehaviour
    {
        public float regenRateIncrease = 20f;
        
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.regenerationRate += regenRateIncrease;
                Destroy(gameObject);
            }
        }
    }
}