using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [Header("Configuration")]
    public float shootForce;
    private float shootForceBackup;
    private ForceMode forceMode;

    [SerializeField] private float shootChargeSpeed;
    [SerializeField] private float shootMaxCharge;
    [SerializeField] private float shootReloadTime;

    [Header("Debug")]
    public bool isReloading;
    private bool isCharging;
    [SerializeField] private float chargeTime;

    [Header("Bow Image")]
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float moveDuration = 2.0f;
    [SerializeField] private RectTransform rectTransform;
    private Vector3 initialPos;
    private bool bowIsMoving = false;
    private float moveTimer = 0.0f;

    [Header("References")]
    [SerializeField] private GameObject bowObject;
    private Camera cam;

    

    [Header("Prefabs")]
    [SerializeField] private Rigidbody arrowPrefab;

    void Start()
    {
        cam = Camera.main;
        if (bowObject == null) bowObject = GameObject.Find("BowTemplate");
        rectTransform = bowObject.GetComponent<RectTransform>();
        initialPos = rectTransform.position;

        shootForceBackup = shootForce;
        isReloading = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2));
        if (Input.GetMouseButtonDown(1));


        if (Input.GetMouseButtonDown(0))
        {
            chargeTime = 0;
            moveTimer = 0.0f;
            bowIsMoving = true;
        }
        if (Input.GetMouseButton(0))
        {
            isCharging = true;
            if (isCharging && !isReloading && chargeTime < shootMaxCharge)
            {
                chargeTime += Time.deltaTime * shootChargeSpeed;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            bowIsMoving = false;
            rectTransform.position = initialPos;

            if (chargeTime <= shootMaxCharge * 0.5)
            {
                //print("No Shoot");
                chargeTime = 0;
            }
            else
            if (chargeTime >= shootMaxCharge * 0.5 && chargeTime <= shootMaxCharge * 0.75)
            {
                //print("Weak Shoot");
                StartCoroutine(nameof(ShootArrow), 1);
            }
            else
            if (chargeTime >= shootMaxCharge * 0.75 && chargeTime <= shootMaxCharge + 1)
            {
                //print("Hard Shoot");
                StartCoroutine(nameof(ShootArrow), 2);
            }
        }

        if (bowIsMoving)
        {
            if (moveTimer < moveDuration)
            {
                float distance = rectTransform.sizeDelta.y * 0.5f;
                float step = moveSpeed * Time.deltaTime;
                Vector3 targetPosition = initialPos - new Vector3(0, distance, 0);
                rectTransform.position = Vector3.MoveTowards(rectTransform.position, targetPosition, step);
                moveTimer += Time.deltaTime;
            }
            else
            {
                bowIsMoving = false;
            }
        }
    }

    IEnumerator ShootArrow(int shootDamage)
    {
        isCharging = false;
        chargeTime = 0;
        isReloading = true;

        //Shoot Direction
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Quaternion rotation = Quaternion.LookRotation(ray.direction);

        Rigidbody arrow = arrowPrefab;
        Vector3 spawnPosition = Vector3.zero;

        //spawnPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + .5f));
        spawnPosition = cam.transform.position;

        Rigidbody newArrow = Instantiate(arrow, spawnPosition, rotation) as Rigidbody;
        newArrow.AddForce(ray.direction * shootForce, ForceMode.VelocityChange);
        //newArrow.transform.Rotate(0, 0, Random.Range(0, 180), Space.Self);

        yield return new WaitForSeconds(shootReloadTime);
        shootForce = shootForceBackup;
        isReloading = false;
    }
}
