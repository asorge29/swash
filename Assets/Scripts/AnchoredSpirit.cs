using System;
using UnityEngine;

[SelectionBase]
public class AnchoredSpirit : MonoBehaviour
{
    public Color soulColor;
    
    private bool _anchored;

    private void Start()
    {
        _anchored = true;
    }

    public void ReleaseAnchor()
    {
        _anchored = false;
    }

    public bool CheckAnchored()
    {
        return _anchored;
    }
}