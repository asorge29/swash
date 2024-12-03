using System;
using UnityEngine;

[SelectionBase]
public class AnchoredSpirit : MonoBehaviour
{
    public Color soulColor;
    
    [HideInInspector]public bool anchored;

    private void Start()
    {
        anchored = true;
    }
}