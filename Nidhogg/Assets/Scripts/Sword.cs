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

    [SerializeField] GameObject Holder;
    [SerializeField] GameObject HolderPrefab;

    [SerializeField] float knockBackForce;


    private void Start()
    {
        manager = FindObjectOfType<Manager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.layer = 7;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        isInAir = false;
        gameObject.transform.parent = null;
        Destroy(Holder);
        
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

                    gameObject.transform.parent = null;
                    
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
            SwordHolder.GetComponent<Rigidbody2D>().AddForce(-transform.forward * 500);
            Debug.Log("KncokBack");
        }

    }

    private void Update()
    {
        if (isInAir)
        {
            Holder = Instantiate(HolderPrefab, gameObject.transform);
            Holder.transform.parent = null;
            transform.parent = Holder.transform;
            Holder.transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
            gameObject.transform.parent = null;
            Destroy(Holder);
        }
    }
}
