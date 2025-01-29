using System.Collections;
using UnityEngine;

public class ShootParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float _speed = 0.15f;
    Coroutine _coroutine;

    public void StartShooting()
    {
        _coroutine = StartCoroutine(Shoot());
    }

    public void StopShooting()
    {
        StopCoroutine(_coroutine);
        _particleSystem.Stop();
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            _particleSystem.Play();
            yield return new WaitForSeconds(_speed);
        }
    }
}
