using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    int swordPlace = 1;
    [SerializeField] GameObject swordPlacement;
    [SerializeField] GameObject sword;
    bool hasSword;

    [SerializeField] float throwForce;

    [SerializeField] KeyCode[] inputs;
    
    private void Update()
    {
        Movement();
        MoveSword();
        PickUpAndThrowSword();
        Debug.Log(Input.GetAxis("Debug Horizontal"));
    }
    void Movement()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
            if (Input.GetAxis("Horizontal") > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);
            }
        }
    }
    void MoveSword()
    {
        if (sword != null && hasSword)
        {
            if (Input.GetKeyDown(inputs[0]))
            {
                if (swordPlace != 2)
                {
                    sword.transform.position += new Vector3(0, 0.5f, 0);

                    swordPlace++;
                }
            }
            if (Input.GetKeyDown(inputs[1]))
            {
                if (swordPlace != 0)
                {
                    sword.transform.position += new Vector3(0, -0.5f, 0);

                    swordPlace--;
                }
            }
        }
    }
    void PickUpAndThrowSword()
    {
        if(sword != null && !hasSword && Input.GetKeyDown(inputs[2]) && !sword.GetComponent<Sword>().isInHands)
        {
            sword.transform.parent = swordPlacement.transform;
            sword.transform.position = swordPlacement.transform.position;
            sword.transform.rotation = swordPlacement.transform.rotation;

            sword.GetComponent<Sword>().isInHands = true;

            sword.GetComponent<Rigidbody2D>().isKinematic = true;
            hasSword = true;

            swordPlace = 1;
        }
        if(hasSword && Input.GetKeyDown(inputs[3]) && (swordPlace == 2))
        {
            sword.transform.parent = null;
            sword.GetComponent<Rigidbody2D>().isKinematic = false;
            sword.GetComponent<Rigidbody2D>().gravityScale = 0;
            sword.GetComponent<Rigidbody2D>().AddForce(-gameObject.transform.right * throwForce);
            sword.GetComponent<Sword>().isInAir = true;

            sword.GetComponent<Sword>().isInHands = false;

            sword = null;
            hasSword = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword") && !hasSword)
        {
            
            sword = collision.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == sword)
        {
            sword = null;
        }
    }
}
