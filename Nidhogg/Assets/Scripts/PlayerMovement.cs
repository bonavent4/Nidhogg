using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    int swordPlace = 1;
    [SerializeField] GameObject swordPlacement;
    [SerializeField] Transform swordOverHeadPlace;
    [SerializeField] GameObject sword;
    bool hasSword;

    [SerializeField] float throwForce;
    [SerializeField] bool canThrow;

    [SerializeField] KeyCode[] inputs;
    [SerializeField] string walkAxis;

    //RaycastHit2D hit;
    public bool isOnGround;

    [SerializeField] float jumpForce;
    //bool done;
    bool canUse;
    float timer;
    [SerializeField] float timeBeforeUse;

    [SerializeField] GameObject sprite;

    public bool isPlayer1;

    bool isSwinging;
    public bool swordGoingBack;
    Vector3 previousPosition;
    Vector3 goToPosition;
    [SerializeField] float swordSwingOffset;
    [SerializeField] float swordSwingTime;

    [SerializeField] GameObject playerSprite;

    
    private void Update()
    {
        JumpAndDuck();
        MoveSword();
        PickUpAndThrowSword();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        if (Input.GetAxis(walkAxis) != 0)
        {
            gameObject.transform.position += new Vector3(Input.GetAxis(walkAxis) * speed * Time.deltaTime, 0, 0);
            if (Input.GetAxis(walkAxis) > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);
            }
        }
    }
    void JumpAndDuck()
    {
        if (isOnGround && Input.GetKeyDown(inputs[3]))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
            isOnGround = false;
        }
    }
    void MoveSword()
    {
        if (sword != null && hasSword)
        {
            if (Input.GetKey(inputs[0]))
            {
                if (timer >= timeBeforeUse)
                {
                    canUse = true;
                }
                if (swordPlace != 2 && canUse)
                {
                    sword.transform.position += new Vector3(0, 0.5f, 0);

                    swordPlace++;
                    canUse = false;
                    timer = 0;
                }
                else if (swordPlace == 2 && canUse)
                {
                    sword.transform.position = swordOverHeadPlace.position;
                    sword.transform.rotation = swordOverHeadPlace.rotation;

                    canThrow = true;
                }
                timer += Time.deltaTime;
            }
            else if (Input.GetKey(inputs[1]))
            {
                if(timer >= timeBeforeUse)
                {
                    canUse = true;
                }
                if (swordPlace != 0 && canUse)
                {
                    sword.transform.position += new Vector3(0, -0.5f, 0);

                    swordPlace--;
                    canUse = false;
                    timer = 0;
                }
                else if(swordPlace == 0 && canUse)
                {
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.08f);
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.412f);
                    playerSprite.transform.localPosition = new Vector3(0, -0.4153f, 0);
                    playerSprite.transform.localScale = new Vector3(1, 1.074617f, 1);
                }
                timer += Time.deltaTime;
            }
            else
            {
                canUse = true;
                timer = 0;

                if(sword.transform.position == swordOverHeadPlace.transform.position)
                {
                    
                    sword.transform.position = new Vector3(swordPlacement.transform.position.x, swordPlacement.transform.position.y + 0.5f, swordPlacement.transform.position.z);
                    sword.transform.rotation = swordPlacement.transform.rotation;
                    canThrow = false;
                }
                

                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.9f);
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                playerSprite.transform.localPosition = new Vector3(0, 0, 0);
                playerSprite.transform.localScale = new Vector3(1, 1.905f, 1);

            }
        }
        else if(!hasSword && Input.GetKey(inputs[1]))
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.08f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.412f);

            playerSprite.transform.localPosition = new Vector3(0, -0.4153f, 0);
            playerSprite.transform.localScale = new Vector3(1, 1.074617f, 1);
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.9f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            playerSprite.transform.localPosition = new Vector3(0, 0, 0);
            playerSprite.transform.localScale = new Vector3(1, 1.905f, 1);
        }
    }
    void PickUpAndThrowSword()
    {
        if(sword != null && !hasSword && Input.GetKeyDown(inputs[2]) && !sword.GetComponent<Sword>().isInHands && !sword.GetComponent<Sword>().isInAir)
        {
            sword.transform.parent = swordPlacement.transform;
            sword.transform.position = swordPlacement.transform.position;
            sword.transform.rotation = swordPlacement.transform.rotation;

            sword.GetComponent<Sword>().isInHands = true;

            sword.GetComponent<Rigidbody2D>().isKinematic = true;
            sword.GetComponent<Sword>().SwordHolder = gameObject;

            hasSword = true;

            swordPlace = 1;
        }
        if(hasSword && Input.GetKeyDown(inputs[2]) && !isSwinging)
        {
            if (canThrow && (swordPlace == 2))
            {
                sword.transform.parent = null;
                sword.GetComponent<Rigidbody2D>().isKinematic = false;
                sword.GetComponent<Rigidbody2D>().gravityScale = 0;
                sword.GetComponent<Rigidbody2D>().AddForce(-gameObject.transform.right * throwForce);
                sword.GetComponent<Sword>().isInAir = true;

                sword.GetComponent<Sword>().isInHands = false;

                sword = null;
                hasSword = false;
                canThrow = false;
            }
            else
            {
                previousPosition = sword.transform.localPosition;
                goToPosition = new Vector3(sword.transform.localPosition.x - swordSwingOffset, sword.transform.localPosition.y, sword.transform.localPosition.z);
                isSwinging = true;
            }
        }
        if (isSwinging)
        {
            if(sword.transform.localPosition != goToPosition && !swordGoingBack)
            {
                sword.transform.localPosition = Vector3.MoveTowards(sword.transform.localPosition, goToPosition, swordSwingTime);
            }
            else
            {
                swordGoingBack = true;
                if(sword.transform.localPosition != previousPosition)
                {
                    sword.transform.localPosition = Vector3.MoveTowards(sword.transform.localPosition, previousPosition, swordSwingTime);
                }
                else
                {
                    isSwinging = false;
                    swordGoingBack = false;
                }
                
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            if (!collision.gameObject.transform.parent.GetComponent<Sword>().isInAir)
            {
                sword = collision.gameObject.transform.parent.gameObject;
            }
            else
            {
                
                Die();
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == sword)
        {
            sword = null;
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " Died");


    }
}
