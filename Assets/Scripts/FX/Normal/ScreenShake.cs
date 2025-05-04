using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    [SerializeField] float shakeForce;
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(Vector2 shakeDir)
    {
        impulseSource.m_DefaultVelocity = new Vector3(shakeDir.x * shakeForce, shakeDir.y * shakeForce);
        impulseSource.GenerateImpulse();
    }
}
