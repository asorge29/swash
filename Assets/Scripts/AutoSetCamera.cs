using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AutoSetCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = GetComponent<CinemachineVirtualCamera>();
        _camera.Follow = _player.transform;
    }
}