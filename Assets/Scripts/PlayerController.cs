using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    public float moveSpeed = 5f; // Speed of movement
    public float laneDistance = 3f; // Distance between lanes
    public GameObject bulletPrefab; // Bullet prefab to be assigned in the inspector
    public Transform bulletSpawnPoint; // The point where bullets will spawn
    public float bulletSpeed = 10f; // Speed of the bullet
    public float fireRate = 0.5f; // Time interval between shots

    private int targetLane = -1; // Start with left lane (-1 = left, 1 = right)
    private Vector2 startTouchPosition, endTouchPosition; // For swipe detection
    private bool swipeDetected;
    private float nextFireTime = 0f; // Time until the next shot


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MovePlayer(); // Initialize player position to the left lane at the start
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        KeyboardInput(); // Detect keyboard input for left and right movement
#else
        DetectSwipe(); // Detect swipe input for mobile
#endif
        Shoot();       // Handle shooting
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // Move to the left lane
        {
            targetLane = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // Move to the right lane
        {
            targetLane = 1;
        }
        MovePlayer();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeDetected = false;
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && !swipeDetected)
            {
                endTouchPosition = touch.position;
                Vector2 swipe = endTouchPosition - startTouchPosition;

                // Horizontal swipe detection
                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y) && Mathf.Abs(swipe.x) > 50f)
                {
                    swipeDetected = true;
                    if (swipe.x > 0) // Swipe Right
                    {
                        targetLane = 1;
                    }
                    else if (swipe.x < 0) // Swipe Left
                    {
                        targetLane = -1;
                    }
                    MovePlayer();
                }
            }
        }
    }

    private void MovePlayer()
    {
        // Calculate target position based on the lane
        Vector3 targetPosition = new Vector3(targetLane * laneDistance, transform.position.y, transform.position.z);

        // Smoothly move player to the target lane position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        // Check if it's time to fire the next bullet
        if (Time.time >= nextFireTime)
        {
            // Instantiate the bullet at the spawn point and set its forward direction
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = transform.forward * bulletSpeed;

            // Set the time for the next shot
            nextFireTime = Time.time + fireRate;
        }
    }

    public void ModifyFireRate(float newFireRate)
    {
        fireRate = newFireRate;
        Debug.Log($"Fire rate updated to: {fireRate}");
    }


}
