using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Assign in the Inspector for UI Text
    public TextMeshPro textMeshPro3D;  // Assign in the Inspector for 3D Text
    public float typingSpeed = 0.05f;  // Delay between each character

    private string fullText; // Stores the full text
    private bool isTyping = false;

    private void Start()
    {
        // Use either UI or 3D text based on what is assigned
        fullText = textMeshPro ? textMeshPro.text : textMeshPro3D.text;
        ClearText();
        StartCoroutine(TypeText());
    }

    private void ClearText()
    {
        if (textMeshPro) textMeshPro.text = "";
        if (textMeshPro3D) textMeshPro3D.text = "";
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            if (textMeshPro) textMeshPro.text = fullText.Substring(0, i);
            if (textMeshPro3D) textMeshPro3D.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    // Optional: Call this to restart typing
    public void RestartTyping()
    {
        StopAllCoroutines();
        ClearText();
        StartCoroutine(TypeText());
    }
}