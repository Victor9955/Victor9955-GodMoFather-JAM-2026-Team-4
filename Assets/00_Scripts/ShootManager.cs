using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] float increaseSpeed = 0.1f;
    [SerializeField] float decreaseSpeed = 0.1f;
    public event Action ShootEvent;
    ShootParticle particles;
    float amount = 1f;
    float interval = 0.25f;
    float lastTime = 0f;

    private void Start()
    {
        inputManager.inputActions.Player.Attack.performed += ShootAction_performed;
    }
    
    public void SetupShoot(ShootParticle m_particles)
    {
        particles = m_particles;
        foreach (RotateToward particleDirection in particles.transform.GetComponentsInChildren<RotateToward>())
        {
            particleDirection.forward = transform;
        }
    }

    private void ShootAction_performed(InputAction.CallbackContext obj)
    {
        DoShoot();
        amount = Mathf.Clamp01(amount - decreaseSpeed);
    }

    private void Update()
    {
        if(Time.time >= lastTime + interval)
        {
            lastTime = Time.time;
            amount = Mathf.Clamp01(amount + increaseSpeed);
        }
        UIManager.instance.shootScrollbar.size = amount;
    }

    public void DoShoot()
    {
        if(amount > 0f)
        {
            ShootEvent?.Invoke();
            particles.PlayShoot();
        }
    }
}
