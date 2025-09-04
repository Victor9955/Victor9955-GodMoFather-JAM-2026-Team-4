using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;
    [SerializeField] Animation placeClip;
    [SerializeField] private AudioClip audioTir = null;

    private void LateUpdate()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (audioTir != null && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(audioTir);
            }

            placeClip.Play();
           

        }
    }
}
