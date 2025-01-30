using System.Collections;
using UnityEngine;

public class ShootParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float _speed = 0.15f;
    Coroutine _coroutine;

    public void StartShooting()
    {
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(Shoot());
        }
    }

    public void StopShooting()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _particleSystem.Stop();
            _coroutine = null;
        }
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
