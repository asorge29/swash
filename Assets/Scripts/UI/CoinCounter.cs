using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CoinCounter : MonoBehaviour
    {
        private PlayerController _player;
    
        private TMP_Text _textMesh;
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _textMesh = GetComponent<TMP_Text>();
            _textMesh.text = _player.coins.ToString();
        }
    
        void Update()
        {
            if (_player.coins.ToString() != _textMesh.text)
            {
                _textMesh.text = _player.coins.ToString();
            }
        }
    }
}