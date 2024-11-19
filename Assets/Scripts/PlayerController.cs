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
    public int maxHealth = 100; // Maximum health of the player
    public int currentHealth; // Current health of the player

    public GameObject duplicatePlayer;
    public Transform duplicatePlayerSpawnPoint;
    public bool duplicateActive = false;
    public bool duplicateDestroyed = false;
    private bool canActivateDuplicate = false;

    private Rigidbody rb; // Reference to the player's Rigidbody
    private int targetLane = -1; // Start in the left lane (-1 = left, 1 = right)
    private Vector2 startTouchPosition, endTouchPosition; // For swipe detection
    private bool swipeDetected;
    private float nextFireTime = 0f; // Time until the next shot

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing!");
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if(duplicatePlayer != null)
        {
            duplicatePlayer.SetActive(false); //make sure that the duplicate starts off
        }
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        KeyboardInput(); // Detect keyboard input for left and right movement
#else
        DetectSwipe(); // Detect swipe input for mobile
#endif
        Shoot(); // Handle shooting
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
        Vector3 targetPosition = new Vector3(targetLane * laneDistance, rb.position.y, rb.position.z);
        Vector3 direction = (targetPosition - rb.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = transform.forward * bulletSpeed;

            nextFireTime = Time.time + fireRate;

            Bullet bulletScript = bullet.AddComponent<Bullet>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player is Dead!");
            // Add player death logic here
        }
    }

    public void ActivateDuplicate()
    {
        if(!duplicateActive && canActivateDuplicate && duplicatePlayer != null)
        {
            duplicatePlayer.SetActive(true);
            duplicatePlayer.transform.position = duplicatePlayerSpawnPoint.position;
            duplicateActive = true;
            canActivateDuplicate = false;
        }
    }

    public void DestroyDuplicate()
    {
        if(duplicatePlayer != null)
        {
            duplicatePlayer.SetActive(false);
            duplicateActive = false;
            canActivateDuplicate = true;
        }
    }

    public void ReactivateDuplicateFromGate()
    {
        if (duplicateDestroyed && !duplicateActive)
        {
            duplicatePlayer.SetActive(true);
            duplicateActive = true;
            duplicateDestroyed = false; // Reset flag
            Debug.Log("Duplicate player reactivated from gate.");
        }
    }

    public void ModifyFireRate(float newFireRate)
    {
        fireRate = newFireRate;
        Debug.Log($"Fire rate updated to: {fireRate}");
    }

    public class Bullet : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 4f);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Gate") || other.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if(duplicateActive && collision.gameObject != duplicatePlayer)
                {
                    DestroyDuplicate();
                }
                else
                {
                    TakeDamage(enemy.damageAmount);
                }

                Destroy(collision.gameObject);
                
            }
        }
    }

}
