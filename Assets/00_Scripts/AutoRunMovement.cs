using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoRunMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float stopTimer;
    public Transform orientation;
    Vector3 moveDirection;

    Rigidbody rb;

    bool canMove = true;

    [Header("UI")]
    [SerializeField] Image circle;

    AudioSource audioSource;
    [SerializeField] ClientGlobalInfo info;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        audioSource = GetComponent<AudioSource>();
        enabled = info.movementNormal == false;
    }

    private void Update()
    {
        SpeedControl();
        if(Input.GetMouseButtonDown(0))
        {
            if(timer != null)
            {
                StopCoroutine(timer);
            }
            timer = StartCoroutine(Timer());
        }

        if (rb.linearVelocity.x == 0 && rb.linearVelocity.y == 0)
        {
            if(audioSource.isPlaying && audioSource.loop == false) return;
            audioSource.loop = false;
            audioSource.Play();
        }
        else
        {
            if (audioSource.loop == true) return;
            audioSource.loop = true;
        }
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    Coroutine timer;

    IEnumerator Timer()
    {
        canMove = false;
        float timer = stopTimer;
        rb.linearVelocity = Vector3.zero;
        while (timer > 0f)
        {
            yield return null;
            circle.fillAmount = timer / stopTimer;
            timer -= Time.deltaTime;
        }
        timer = 0f;
        circle.fillAmount = 0f;
        canMove = true;
    }
}
