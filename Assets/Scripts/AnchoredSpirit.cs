using System;
using UnityEngine;

[SelectionBase]
public class AnchoredSpirit : MonoBehaviour
{
    public Color soulColor;
    public Health health;
    
    private bool _anchored;

    private void Start()
    {
        _anchored = true;
        health.MakeAnchored();
    }

    public void ReleaseAnchor()
    {
        _anchored = false;
        health.NotAnchored();
    }

    public bool CheckAnchored()
    {
        return _anchored;
    }
}