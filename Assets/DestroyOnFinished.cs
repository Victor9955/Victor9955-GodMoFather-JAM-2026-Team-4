using UnityEngine;

public class DestroyOnFinished : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;
    float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
    }
    void Update()
    {
        if(Time.time > spawnTime + _particles.main.duration)
        {
            Destroy(gameObject);
        }
    }
}
