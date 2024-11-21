using UnityEngine;

namespace Pickups
{
    public class ParrotsFeather : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.dashAbility = true;
                Destroy(gameObject);
            }
        }
    }
}