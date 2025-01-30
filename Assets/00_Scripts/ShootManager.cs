using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] NetworkClient networkClient;
    [SerializeField] float increaseSpeed = 10f;
    [SerializeField] float decreaseSpeed = 10.5f;
    public event Action ShootEvent;
    ShootParticle particles;
    float amount = 1f;

    private void Start()
    {
        inputManager.inputActions.Player.Attack.performed += ShootAction_performed;
    }
    
    public void SetupShoot(ShootParticle m_particles)
    {
        particles = m_particles;
    }

    private void ShootAction_performed(InputAction.CallbackContext obj)
    {
        amount = Mathf.Clamp01(amount - decreaseSpeed * Time.deltaTime);
    }

    private void Update()
    {
        amount = Mathf.Clamp01(amount + increaseSpeed * Time.deltaTime);
        DoShoot();
        UIManager.Instance.shootScrollbar.size = amount;
    }

    public void DoShoot()
    {
        if(amount >= 0f)
        {
            ShootEvent?.Invoke();
        }
    }
}
