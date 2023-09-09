using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    float multiplier = 0.01f;    
    float mouseX;
    float mouseY;
    float xRotation;
    float yRotation;
    
    [Header("References")]
    [SerializeField] Transform orientationObject;
    Camera cam;
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MoveInput();
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientationObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void MoveInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
