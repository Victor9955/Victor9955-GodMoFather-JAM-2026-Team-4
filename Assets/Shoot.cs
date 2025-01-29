using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] InputAction shootAction;
    [SerializeField] float increaseSpeed = 0.01f;
    [SerializeField] float decreaseMultiplier = 4f;
    ShootParticle particles;
    bool isShooting = false;
    bool oldIsShooting = false;
    bool canShoot = false;
    [SerializeField,Range(0, 1)] float size = 1f;

    private void Start()
    {
        shootAction.Enable();
        shootAction.performed += ShootAction_performed;
        shootAction.canceled += ShootAction_canceled;
    }
    
    public void SetParticle(ShootParticle m_particles)
    {
        particles = m_particles;
    }

    private void ShootAction_canceled(InputAction.CallbackContext obj)
    {
        isShooting = false;
    }

    private void ShootAction_performed(InputAction.CallbackContext obj)
    {
        isShooting = true;
    }

    private void Update()
    {
        size = Mathf.Clamp(size + increaseSpeed, 0f, 1f);
        if(isShooting)
        {
            size = Mathf.Clamp(size - increaseSpeed * decreaseMultiplier, 0f, 1f);
        }

        canShoot = size < 0;

        if (oldIsShooting != canShoot)
        {
            oldIsShooting = canShoot;
            if(!canShoot)
            {
                particles.StartShooting();
            }
            else
            {
                particles.StopShooting();
            }
        }

        if(canShoot)
        {
            DoShoot();
        }
    }

    public void DoShoot()
    {
        //TODO : Implement shooting behavior
    }
}
