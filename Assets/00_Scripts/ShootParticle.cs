using System.Collections;
using UnityEngine;

public class ShootParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    public void PlayShoot()
    {
        _particleSystem.Play();
    }
}
