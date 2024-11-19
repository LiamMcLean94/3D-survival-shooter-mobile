using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum GateType { Duplication, Upgrade }
    public GateType gateType;

    public float moveSpeed = 3f;
    public int maxHealth = 3;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        MoveGate();
    }

    public void OnHitByBullet()
    {
        currentHealth--;

        Debug.Log($"Gate hit! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log($"GateType: {gateType}");

            if (gateType == GateType.Duplication)
            {
                Debug.Log("Attempting to duplicate player...");
                DuplicatePlayer();
            }
            else if (gateType == GateType.Upgrade)
            {
                Debug.Log("Attempting to upgrade weapon...");
                UpgradeWeapon();
            }

            Destroy(gameObject); // Destroy the gate only after performing its action
        }
    }

    private void DuplicatePlayer()
    {
        if (PlayerController.Instance != null)
        {
            GameObject player = PlayerController.Instance.gameObject;
            Vector3 spawnPosition = player.transform.position + new Vector3(1.11f, 0, 0); // Offset to avoid overlap
            GameObject newPlayer = Instantiate(player, spawnPosition, player.transform.rotation);
            Debug.Log($"Player duplicated at position: {spawnPosition}");
        }
        else
        {
            Debug.LogError("PlayerController.Instance is null in DuplicatePlayer!");
        }
    }

    private void UpgradeWeapon()
    {
        if (PlayerController.Instance != null)
        {
            Debug.Log("Upgrading weapon... Increasing fire rate temporarily.");
            StartCoroutine(TemporaryFireRateIncrease(0.2f, 5f));
        }
        else
        {
            Debug.LogError("PlayerController.Instance is null in UpgradeWeapon!");
        }
    }

    private IEnumerator TemporaryFireRateIncrease(float newFireRate, float duration)
    {
        PlayerController.Instance.ModifyFireRate(newFireRate);
        Debug.Log($"Fire rate set to {newFireRate} for {duration} seconds.");

        yield return new WaitForSeconds(duration);

        PlayerController.Instance.ModifyFireRate(0.5f); // Reset to default fire rate
        Debug.Log("Fire rate reset to default.");
    }

    public void MoveGate()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (transform.position.z < -10f) // Destroy if it moves behind the player
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected, calling OnHitByBullet.");
            OnHitByBullet();
            Destroy(other.gameObject); // Destroy bullet after hitting the gate
        }
    }

}
