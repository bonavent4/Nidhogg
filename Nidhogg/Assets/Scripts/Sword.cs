using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool isInAir;
    public bool isInHands;

    [SerializeField] float speed;

    public GameObject SwordHolder;

    Manager manager;



    private void Start()
    {
        manager = FindObjectOfType<Manager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        isInAir = false;

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject != SwordHolder)
            {

                if (isInAir)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().Die();

                    gameObject.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
                    gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    manager.ChangeState(SwordHolder, collision.gameObject);

                    isInAir = false;
                }
                else if(isInHands)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().Die();
                    manager.ChangeState(SwordHolder, collision.gameObject);
                }
            }
        }
        if (collision.gameObject.tag == "SwordHolder" && isInHands && collision.gameObject.GetComponent<Sword>().isInHands)
        {
            Debug.Log("Hit Sword ");
            SwordHolder.GetComponent<PlayerMovement>().swordGoingBack = true;
        }

    }

    private void Update()
    {
        if (isInAir)
        {
            transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
        }
    }
}
