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

    [SerializeField] AudioSource LightsaberHitSound;
    public GameObject LightsaberEnd;
    private void Start()
    {
        manager = FindObjectOfType<Manager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //gameObject.layer = 7;
        gameObject.transform.parent = null;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        isInAir = false;
        
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
                    collision.gameObject.GetComponent<PlayerMovement>().isHeadShot = true;
                    collision.gameObject.GetComponent<PlayerMovement>().Die();
                    

                    gameObject.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
                    gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    manager.ChangeState(SwordHolder, collision.gameObject);


                    isInAir = false;

                    gameObject.transform.parent = null;

                   
                    
                }
                else if(isInHands)
                {
                    
                    if(SwordHolder.GetComponent<PlayerMovement>().swordPlace == 2)
                    {
                        collision.gameObject.GetComponent<PlayerMovement>().isHeadShot = true;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<PlayerMovement>().isHeadShot = false;

                    }
                    collision.gameObject.GetComponent<PlayerMovement>().Die();
                    manager.ChangeState(SwordHolder, collision.gameObject);
                }
            }
        }
        if (collision.gameObject.tag == "SwordHolder")
        {
            if (isInHands && collision.gameObject.GetComponent<Sword>().isInHands)
            {
                if (SwordHolder.GetComponent<PlayerMovement>().isSwinging)
                {
                    SwordHolder.GetComponent<PlayerMovement>().swordGoingBack = true;
                }
                

                SwordHolder.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                SwordHolder.GetComponent<Rigidbody2D>().AddForce(transform.right * 265);
                SwordHolder.GetComponent<PlayerMovement>().isGettingKnockedBack = true;
                Debug.Log("KncokBack");
                LightsaberHitSound.Play();
            }
            else if (isInAir)
            {
                isInAir = false;
                gameObject.transform.parent = null;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                
                
                
            }
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
