using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        private PlayerController _player;
    
        private Slider _slider;
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _slider = GetComponent<Slider>();
            _slider.maxValue = _player.startingHealth;
            _slider.value = _player.GetHealth();
        }
    
        void Update()
        {
            if (_player.GetHealth() != _slider.value)
            {
                _slider.value = _player.GetHealth();
            }

            if (_player.startingHealth != _slider.maxValue)
            {
                _slider.maxValue = _player.startingHealth;
            }
        }
    }
}
