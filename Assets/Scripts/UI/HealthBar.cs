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
            _slider.maxValue = player.maxHealth;
            _slider.value = player.health;
        }
    
        void Update()
        {
            if (player.health != _slider.value)
            {
                _slider.value = player.health;
            }

            if (player.maxHealth != _slider.maxValue)
            {
                _slider.maxValue = player.maxHealth;
            }
        }
    }
}
