using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum GateType { Duplication, Upgrade}
    public GateType gateType;
    //public int requiredShots = 3;
    //public int currentShots = 0;
    public float moveSpeed = 3f;
    //public int health = 3;
    //public int upgradeHealth = 5;
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
        //currentShots++;
        currentHealth--;

        if(currentHealth <= 0)
        {
            if(gateType == GateType.Duplication)
            {
                DuplicatePlayer();
            }
            else if (gateType == GateType.Upgrade)
            {
                UpgradeWeapon();
            }
        }
        Destroy(gameObject);
    }

    void DuplicatePlayer()
    {
        //need to add some logic here
        Debug.Log("Player Duplicated!");
    }

    private void UpgradeWeapon()
    {
       
          Debug.Log("Upgraded Weapon!");
          PlayerController.Instance.ModifyFireRate(0.2f); // Example: Reduce fire rate to shoot faster.
          
       

    }
    public void MoveGate()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (transform.position.z < -10f) //this is for if it goes behind the player
        {
            Destroy(gameObject);
        }
    }

    /*public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }*/

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            OnHitByBullet();
            Destroy(other.gameObject);
        }
    }

}
