using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int health = 2;
    public int damageAmount = 10;
    //rb ref here
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.back * moveSpeed *Time.deltaTime); // this is not picking up collision , move through Rb velocity
        rb.velocity = Vector3.back * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(damageAmount);
            }

            Destroy(gameObject); //this destroys enemy on collision with player
        }
    }


}
