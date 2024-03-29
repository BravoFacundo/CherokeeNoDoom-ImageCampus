using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBillboardRenderer : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool hasDebugCollider;

    [Header("Configuration")]
    public float maxRotationX = 0f;
    public Texture2D dataTexture;

    [Header("Prefabs")]
    [SerializeField] GameObject spriteCollider;

    [Header("References")]
    [SerializeField] Transform target;

    [Header("Local References")]
    [SerializeField] MeshRenderer spriteColliderRenderer;

    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("MainCamera").transform;

        if (hasDebugCollider)
        {
            var newSpriteCollider = Instantiate(spriteCollider, transform);
            newSpriteCollider.transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
    }

    void Update()
    {
        transform.LookAt(target);
        var angleX = transform.eulerAngles.x;
        if (angleX >= maxRotationX && angleX < 360 - maxRotationX)
        {
            angleX = Mathf.Clamp(angleX, 360 - maxRotationX, 360);
            transform.eulerAngles = new Vector3(angleX, transform.eulerAngles.y, 0);
        }
    }
}
