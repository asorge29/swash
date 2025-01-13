using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public PlayerController player;
    
        private Slider _slider;
        void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.maxValue = player.startingHealth;
            _slider.value = player.GetHealth();
        }
    
        void Update()
        {
            if (player.GetHealth() != _slider.value)
            {
                _slider.value = player.GetHealth();
            }

            if (player.startingHealth != _slider.maxValue)
            {
                _slider.maxValue = player.startingHealth;
            }
        }
    }
}
