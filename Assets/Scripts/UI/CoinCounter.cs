using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CoinCounter : MonoBehaviour
    {
        public PlayerController player;
    
        private TMP_Text _textMesh;
        void Start()
        {
            _textMesh = GetComponent<TMP_Text>();
            _textMesh.text = player.coins.ToString();
        }
    
        void Update()
        {
            if (player.coins.ToString() != _textMesh.text)
            {
                _textMesh.text = player.coins.ToString();
            }
        }
    }
}