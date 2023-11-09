using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    /*notes: to Do
     * make sword swing around middle while in air at -0.53(middle of sword)
     * change scene and update Ui to state
     * 
     */

    [SerializeField] float Walkspeed;
    [SerializeField] float CrouchSpeed;
    float speed;
      

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

    [SerializeField] float WalkjumpForce;
    [SerializeField] float CrouchJumpForce;
    float jumpForce;
    [SerializeField] float thrustForce;
    //bool done;
    bool canUse;
    float timer;
    [SerializeField] float timeBeforeUse;

    [SerializeField] GameObject sprite;

    public bool isPlayer1;

    [SerializeField]bool isSwinging;
    public bool swordGoingBack;
    Vector3 previousPosition;
    Vector3 goToPosition;
    [SerializeField] float swordSwingOffset;
    [SerializeField] float swordSwingTime;

    [SerializeField] GameObject playerSprite;

    public Animator anim;
    [SerializeField] AudioSource LightsaberSound;

    Explode explode;

    Manager manager;

    [SerializeField] GameObject OtherPlayer;
    [SerializeField] float offset;
    [SerializeField] float BehindOffset;
    [SerializeField] float maxDistance;

    [SerializeField] GameObject[] SwordVarients;

    public bool isGettingKnockedBack;

    float knockBackTimer;
    [SerializeField] float TimeBeforeConcience;

    bool hasSlid;
    [SerializeField] float slidePower;

    float CrouchTimer;
     float MaxTimeBeforeSlide = 0.1f;

    [SerializeField] GameObject CrouchSwordPosition;

    bool haveResetCrouch;

    float UnSlideTimer;
    [SerializeField] float TimeBeforUnSlide;
    bool CanStandFromSlide = true;
    private void Start()
    {
        speed = Walkspeed;
        anim = GetComponentInChildren<Animator>();
        explode = GetComponentInChildren<Explode>();
        manager = FindObjectOfType<Manager>();

        sword = Instantiate(SwordVarients[Random.Range(0, SwordVarients.Length)], gameObject.transform.position, gameObject.transform.rotation); ;
        //sword.transform.parent = null;
        PickupSword();
    }


    private void Update()
    {
        if (isGettingKnockedBack)
        {
            if(knockBackTimer < TimeBeforeConcience)
            {

                knockBackTimer += Time.deltaTime;
            }
            else
            {
                isGettingKnockedBack = false;
                knockBackTimer = 0;
            }
        }


        if (anim.GetBool("Crouching"))
        {
            if (!hasSlid)
            {
                haveResetCrouch = false;
                CrouchTimer += Time.deltaTime;
                if (anim.GetBool("IsRunning") && CrouchTimer < MaxTimeBeforeSlide)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * slidePower);
                    hasSlid = true;
                    CrouchTimer = 0;
                    CanStandFromSlide = false;
                }
                
            }
            else if(!CanStandFromSlide)
            {
                if(UnSlideTimer < TimeBeforUnSlide)
                {
                    UnSlideTimer += Time.deltaTime;
                }
                else
                {
                    CanStandFromSlide = true;
                    UnSlideTimer = 0;
                }
            }
            speed = CrouchSpeed;
            jumpForce = CrouchJumpForce;
        }
        else
        {
            speed = Walkspeed;
            jumpForce = WalkjumpForce;
            hasSlid = false;
            CrouchTimer = 0;
            if (!haveResetCrouch)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                haveResetCrouch = true;
            }
        }
        

        if (!anim.GetBool("IsDead") && !isGettingKnockedBack)
        {
            JumpAndDuck();
            if (!isSwinging)
            {
                MoveSword();
            }

            PickUpAndThrowSword();
            CheckIfTooFarApart();
        }
        
    }
    private void FixedUpdate()
    {
        if (!anim.GetBool("IsDead") && !isGettingKnockedBack && CanStandFromSlide)
        {
            Movement();
        }
    }
    void Movement()
    {
        if (!isSwinging)
        {
            anim.SetInteger("SwordPlace", swordPlace);
            if (Input.GetAxis(walkAxis) != 0)
            {

                anim.SetBool("IsRunning", true);

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
            else
            {
                //Debug.Log("Not Running");
                anim.SetBool("IsRunning", false);
            }
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
        
    }
    void JumpAndDuck()
    {
        if (isOnGround && Input.GetKeyDown(inputs[3]))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
           // isOnGround = false;
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
                if (swordPlace != 2 && canUse && !anim.GetBool("Crouching"))
                {
                    sword.transform.position += new Vector3(0, 0.35f, 0);

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
                if (swordPlace != 0 && canUse && !anim.GetBool("Crouching"))
                {
                    sword.transform.position += new Vector3(0, -0.35f, 0);

                    swordPlace--;
                    canUse = false;
                    timer = 0;
                }
                else if(swordPlace == 0 && canUse)
                {
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.1717f);
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.37410f);
                   
                    anim.SetBool("Crouching", true);

                    sword.transform.position = CrouchSwordPosition.transform.position;
                    sword.transform.rotation = CrouchSwordPosition.transform.rotation;
                }
                timer += Time.deltaTime;
            }
            else if(CanStandFromSlide)
            {
                canUse = true;
                timer = 0;

                if(sword.transform.position == swordOverHeadPlace.transform.position)
                {
                    
                    sword.transform.position = new Vector3(swordPlacement.transform.position.x, swordPlacement.transform.position.y + 0.35f, swordPlacement.transform.position.z);
                    sword.transform.rotation = swordPlacement.transform.rotation;
                    canThrow = false;
                }
                

                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.9f);
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
              
                anim.SetBool("Crouching", false);
                

                if (swordPlace == 0)
                {
                    sword.transform.position = new Vector3(swordPlacement.transform.position.x, swordPlacement.transform.position.y - 0.35f, swordPlacement.transform.position.z);
                    sword.transform.rotation = swordPlacement.transform.rotation;
                }
            }
        }
        else if(!hasSword && Input.GetKey(inputs[1]))
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.1717f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.37410f);


            anim.SetBool("Crouching", true);
            if(sword != null)
            {
                sword.transform.position = CrouchSwordPosition.transform.position;
                sword.transform.rotation = CrouchSwordPosition.transform.rotation;
            }
            
        }
        else if(CanStandFromSlide)
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.9f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            
            anim.SetBool("Crouching", false);
            if(swordPlace == 0)
            {
                sword.transform.position = new Vector3(swordPlacement.transform.position.x, swordPlacement.transform.position.y - 0.35f, swordPlacement.transform.position.z);
                sword.transform.rotation = swordPlacement.transform.rotation;
            }
            
        }
    }
    void PickUpAndThrowSword()
    {
        if(sword != null && !hasSword && anim.GetBool("Crouching") && !sword.GetComponent<Sword>().isInHands && !sword.GetComponent<Sword>().isInAir)
        {
            PickupSword();
        }
        if (hasSword && Input.GetKeyDown(inputs[2]) && !isSwinging && !anim.GetBool("Crouching"))
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
                anim.SetBool("IsThrusting", true);
                gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * thrustForce);
                LightsaberSound.Play();
            }
        }
        if (isSwinging)
        {
            if(sword.transform.localPosition != goToPosition && !swordGoingBack)
            {
                sword.transform.localPosition = Vector3.MoveTowards(sword.transform.localPosition, goToPosition, swordSwingTime * Time.deltaTime);
            }
            else
            {
                swordGoingBack = true;
                if(sword.transform.localPosition != previousPosition)
                {
                    sword.transform.localPosition = Vector3.MoveTowards(sword.transform.localPosition, previousPosition, swordSwingTime * Time.deltaTime);
                }
                else
                {
                    isSwinging = false;
                    anim.SetBool("IsThrusting", false);
                    swordGoingBack = false;
                }
                
            }
        }
        
    }
    void PickupSword()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            if (!collision.gameObject.transform.parent.GetComponent<Sword>().isInAir)
            {
                if (!hasSword && !collision.gameObject.transform.parent.gameObject.GetComponent<Sword>().isInHands)
                {
                    sword = collision.gameObject.transform.parent.gameObject;
                }
                
            }
           /* else
            {
                
                Die();
            }*/
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == sword && !hasSword && !collision.gameObject.GetComponent<Sword>().isInHands)
        {
            sword = null;
        }
    }
    
    public void Die()
    {
        if (!anim.GetBool("IsDead"))
        {
            Debug.Log(gameObject.name + " Died");
            //sword.gameObject.layer = 10;
            if(sword!= null)
            {
                sword.transform.parent = null;
                sword.GetComponent<Rigidbody2D>().isKinematic = false;
                sword.GetComponent<Sword>().isInHands = false;
            }
            
            hasSword = false;
            
            sword = null;

            explode.ExplodeInPieces();
            anim.SetBool("IsDead", true); 
        }
    }

    public void PlayerRespawn()
    {
        if(manager.gameState != 1 && manager.gameState != 7)
        {
            Vector3 Landing = new Vector3(OtherPlayer.transform.position.x + offset, OtherPlayer.transform.position.y + 10, OtherPlayer.transform.position.z);
            if (Landing.x > manager.endPoints[manager.moreThanEndPoint])
            {
                Landing = new Vector3(OtherPlayer.transform.position.x + BehindOffset, OtherPlayer.transform.position.y + 10, OtherPlayer.transform.position.z);
            }
            else if (Landing.x < manager.endPoints[manager.lessThanEndPoint])
            {
                Landing = new Vector3(OtherPlayer.transform.position.x - BehindOffset, OtherPlayer.transform.position.y + 10, OtherPlayer.transform.position.z);
            }

            gameObject.transform.position = Landing;
            anim.SetBool("IsDead", false);

            if(sword != null && hasSword)
            {
                Destroy(sword);
            }

            sword = Instantiate(SwordVarients[Random.Range(0, SwordVarients.Length)], gameObject.transform.position, gameObject.transform.rotation); ;
            //sword.transform.parent = null;
            PickupSword();
        }
    }

    void CheckIfTooFarApart()
    {
        if(gameObject == manager.playerThatLost && Vector2.Distance(new Vector2(gameObject.transform.position.x,0), new Vector2(OtherPlayer.transform.position.x,0)) > maxDistance)
        {
            
            PlayerRespawn();
        }
    }
}
