using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject[] sevenSquares;

    [SerializeField]GameObject PlayerToFollow;
    public GameObject playerThatLost;
    public int gameState = 4;

    Camera cam;
    [SerializeField] float followSpeed;
    [SerializeField] float NotQuickSpeed;

    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;

    public float[] endPoints;
    [SerializeField] float endOfStage;
    int stage = 0;
    public int lessThanEndPoint;
    public int moreThanEndPoint;

    [SerializeField] MusicHandler[] musicHandlers;
    public int previousGamestate;
    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }
    private void Update()
    {
       
    }
    private void FixedUpdate()
    {
        followDude();
    }
    void followDude()
    {
        if (PlayerToFollow != null )
        {
            LessThanMoreThanState();
            
            if(PlayerToFollow.transform.position.x <= endPoints[lessThanEndPoint] || PlayerToFollow.transform.position.x >= endPoints[moreThanEndPoint])
            {
                if(PlayerToFollow.transform.position.x > endPoints[moreThanEndPoint])
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(endPoints[moreThanEndPoint], cam.transform.position.y, cam.transform.position.z), followSpeed);
                    if (PlayerToFollow.transform.position.x > endPoints[moreThanEndPoint] + endOfStage)
                    {
                        previousGamestate = gameState;
                        gameState++;
                        ChangeState(PlayerToFollow, playerThatLost);
                        //changeScene();
                        if((previousGamestate < 4 && previousGamestate > gameState) || (previousGamestate > 4 && previousGamestate < gameState) || previousGamestate == 4)
                        {
                            stage += 2;
                        }
                        else
                        {
                            stage -= 2;
                        }
                        Debug.Log(stage);
                        LessThanMoreThanState();
                        PlayerToFollow.transform.position += new Vector3(2, 0, 0);
                         cam.transform.position = new Vector3(endPoints[lessThanEndPoint], cam.transform.position.y, cam.transform.position.z);

                        
                        playerThatLost.GetComponent<PlayerMovement>().PlayerRespawn();
                        foreach (MusicHandler m in musicHandlers)
                        {
                             m.music();
                            
                        }
                        
                    }
                }
                else 
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(endPoints[lessThanEndPoint], cam.transform.position.y, cam.transform.position.z), followSpeed);
                    if (PlayerToFollow.transform.position.x < endPoints[lessThanEndPoint] - endOfStage)
                    {
                        previousGamestate = gameState;
                        gameState--;
                        ChangeState(PlayerToFollow, playerThatLost);
                        // changeScene();
                        if ((previousGamestate < 4 && previousGamestate > gameState) || (previousGamestate > 4 && previousGamestate < gameState) || previousGamestate == 4)
                        {
                            stage += 2;
                        }
                        else
                        {
                            stage -= 2;
                        }
                        Debug.Log(stage);
                        LessThanMoreThanState();
                          PlayerToFollow.transform.position += new Vector3(-2, 0, 0);
                          cam.transform.position = new Vector3(endPoints[moreThanEndPoint], cam.transform.position.y, cam.transform.position.z);

                        foreach (MusicHandler m in musicHandlers)
                        {
                            m.music();
                        }
                        
                        playerThatLost.GetComponent<PlayerMovement>().PlayerRespawn();
                    }
                }
            }
            else if((Vector2.Distance(new Vector2(PlayerToFollow.transform.position.x, 0), new Vector2(playerThatLost.transform.position.x, 0)) > maxDistance) || playerThatLost.GetComponent<PlayerMovement>().anim.GetBool("IsDead"))
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(PlayerToFollow.transform.position.x, cam.transform.position.y, cam.transform.position.z), followSpeed );
                
            }
            else if(Vector2.Distance(new Vector2(PlayerToFollow.transform.position.x, 0), new Vector2(playerThatLost.transform.position.x, 0)) > minDistance)
            {
                if(PlayerToFollow.transform.position.x < playerThatLost.transform.position.x)
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(PlayerToFollow.transform.position.x + (Vector2.Distance(PlayerToFollow.transform.position, playerThatLost.transform.position) / 2), cam.transform.position.y, cam.transform.position.z), followSpeed );
                }
                else
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(PlayerToFollow.transform.position.x - (Vector2.Distance(PlayerToFollow.transform.position, playerThatLost.transform.position) / 2), cam.transform.position.y, cam.transform.position.z), followSpeed );
                }
            }
            else
            {
                if (PlayerToFollow.transform.position.x < playerThatLost.transform.position.x)
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(PlayerToFollow.transform.position.x + (Vector2.Distance(PlayerToFollow.transform.position, playerThatLost.transform.position) / 2), cam.transform.position.y, cam.transform.position.z), NotQuickSpeed);
                }
                else
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(PlayerToFollow.transform.position.x - (Vector2.Distance(PlayerToFollow.transform.position, playerThatLost.transform.position) / 2), cam.transform.position.y, cam.transform.position.z), NotQuickSpeed);
                }
            }
        }
    }
    public void ChangeState(GameObject Player, GameObject playerLost)
    {
        PlayerToFollow = Player;
        playerThatLost = playerLost;

        if (Player.GetComponent<PlayerMovement>().isPlayer1)
        {
            foreach (GameObject g in sevenSquares)
            {
                g.GetComponent<Image>().color = Color.white;
            }
            for (int i = sevenSquares.Length - 1; i > gameState-2; i--)
            {
                //Debug.Log(i);
                //Debug.Log(sevenSquares.Length);
                sevenSquares[i].GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            foreach (GameObject g in sevenSquares)
            {
                g.GetComponent<Image>().color = Color.white;
            }
            for (int i = gameState-1; i > -1; i--)
            {
                
                sevenSquares[i].GetComponent<Image>().color = Color.blue;
            }
        }
    }
    /*public void PlusGameStat()
    {
        gameState++;
    }*/
    void changeScene()
    {
        Debug.Log("change Scene");
        //int previousGameState = ;
        //if(previousGameState)
        //stage += 2;
        
        
    }
   

   

    void LessThanMoreThanState()
    {
        if (gameState == 4)
        {
            lessThanEndPoint = stage;
            moreThanEndPoint = stage + 1;

            if (endPoints[lessThanEndPoint] > 0)
            {
                endPoints[lessThanEndPoint] = endPoints[lessThanEndPoint] * -1;
            }
            if (endPoints[moreThanEndPoint] < 0)
            {
                endPoints[moreThanEndPoint] = endPoints[moreThanEndPoint] * -1;
            }

        }
        else if (gameState < 4)
        {
            lessThanEndPoint = stage + 1;
            moreThanEndPoint = stage;

            if (endPoints[lessThanEndPoint] > 0)
            {
                endPoints[lessThanEndPoint] = endPoints[lessThanEndPoint] * -1;
            }
            if (endPoints[moreThanEndPoint] > 0)
            {
                endPoints[moreThanEndPoint] = endPoints[moreThanEndPoint] * -1;
            }
        }
        else if (gameState > 4)
        {
            lessThanEndPoint = stage;
            moreThanEndPoint = stage + 1;
            if (endPoints[lessThanEndPoint] < 0)
            {
                endPoints[lessThanEndPoint] = endPoints[lessThanEndPoint] * -1;
            }
            if (endPoints[moreThanEndPoint] < 0)
            {
                endPoints[moreThanEndPoint] = endPoints[moreThanEndPoint] * -1;
            }
        }
    }
}
