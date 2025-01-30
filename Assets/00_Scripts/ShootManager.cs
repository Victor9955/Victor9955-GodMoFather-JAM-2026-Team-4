using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] NetworkClient networkClient;
    [SerializeField] float increaseSpeed = 10f;
    [SerializeField] float decreaseSpeed = 10f;
    [SerializeField] float shootRate = 100f;
    public event Action ShootEvent;
    ShootParticle particles;
    bool inputPressed = false;
    float lastShoot = 0;

    [SerializeField,Range(0, 1)] float amount = 1f;

    private void Start()
    {
        inputManager.inputActions.Player.Attack.performed += ShootAction_performed;
        inputManager.inputActions.Player.Attack.canceled += ShootAction_canceled;
    }
    
    public void SetupShoot(ShootParticle m_particles)
    {
        particles = m_particles;
    }

    private void ShootAction_canceled(InputAction.CallbackContext obj)
    {
        inputPressed = false;
    }

    private void ShootAction_performed(InputAction.CallbackContext obj)
    {
        inputPressed = true;
    }

    private void Update()
    {
        
        if(inputPressed)
        {
            amount = Mathf.Clamp01(amount - decreaseSpeed * Time.deltaTime);
        }
        else
        {
            amount = Mathf.Clamp01(amount + increaseSpeed * Time.deltaTime);
        }

        if (amount > 0 && inputPressed)
        {
            DoShoot();
            particles.StartShooting();
        }
        else
        {
            particles.StopShooting();
        }

        UIManager.Instance.shootScrollbar.size = amount;
    }

    public void DoShoot()
    {
        if(Time.time > lastShoot + shootRate / 60f)
        {
            lastShoot = Time.time;
            ShootEvent?.Invoke();
            //Debug.DrawRay(transform.position, transform.forward * UIManager.Instance.crosshairFollow.amountForward, Color.red, 10f);
        }
    }
}
