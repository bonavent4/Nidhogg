using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject[] sevenSquares;

    [SerializeField]GameObject PlayerToFollow;
    [SerializeField]GameObject playerThatLost;
    [SerializeField]int gameState = 4;

    Camera cam;
    [SerializeField] float followSpeed;
    [SerializeField] float NotQuickSpeed;

    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;

    [SerializeField] float[] endPoints;
    [SerializeField] float endOfStage;
    int stage = 0;
    [SerializeField]int lessThanEndPoint;
    [SerializeField]int moreThanEndPoint;

    [SerializeField] AudioSource CantinaMusic;
    [SerializeField] AudioSource outsideMusic;

    AudioSource theMusic;
    float goal;
    bool isSlwingMusic;
    bool isSpeedingMusic;
    [SerializeField] float SlowSpeed;
    

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        SlowlyStopMusic(theMusic, goal);
    }
    private void FixedUpdate()
    {
        followDude();
    }
    void followDude()
    {
        if (PlayerToFollow != null)
        {
            LessThanMoreThanState();
            
            if(PlayerToFollow.transform.position.x <= endPoints[lessThanEndPoint] || PlayerToFollow.transform.position.x >= endPoints[moreThanEndPoint])
            {
                if(PlayerToFollow.transform.position.x > endPoints[moreThanEndPoint])
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(endPoints[moreThanEndPoint], cam.transform.position.y, cam.transform.position.z), followSpeed);
                    if (PlayerToFollow.transform.position.x > endPoints[moreThanEndPoint] + endOfStage)
                    {
                        int previousGamestate = gameState;
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

                        music();
                    }
                }
                else 
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(endPoints[lessThanEndPoint], cam.transform.position.y, cam.transform.position.z), followSpeed);
                    if (PlayerToFollow.transform.position.x < endPoints[lessThanEndPoint] - endOfStage)
                    {
                        int previousGamestate = gameState;
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


                        music();
                    }
                }
            }
            else if(Vector2.Distance(new Vector2(PlayerToFollow.transform.position.x, 0), new Vector2(playerThatLost.transform.position.x, 0)) > maxDistance)
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
                sevenSquares[i].GetComponent<Image>().color = Color.blue;
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
                
                sevenSquares[i].GetComponent<Image>().color = Color.red;
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
    void music()
    {
        if(gameState == 3 || gameState == 5)
        {
            CantinaMusic.Play();
        }
        else 
        {
            isSlwingMusic = true;
            theMusic = CantinaMusic;
            goal = 0;
        }
        if( gameState == 4 || gameState == 2||gameState == 6)
        {
            outsideMusic.Play();
        }
        else
        {
            isSlwingMusic = true;
            theMusic = outsideMusic;
            goal = 0;
            
        }
    }

    void SlowlyStopMusic(AudioSource audio, float goal)
    {
        if (isSlwingMusic)
        {
            if(audio.volume > goal)
            {
                audio.volume -= SlowSpeed * Time.deltaTime;
            }
            else
            {
                audio.volume = goal;
                isSlwingMusic = false;
            }
        }
        if (isSpeedingMusic)
        {

        }
        
    }
    void SlowlyStartMusic(AudioSource audio, float goal)
    {
        if (isSpeedingMusic)
        {
            if (audio.volume > goal)
            {
                audio.volume -= SlowSpeed * Time.deltaTime;
            }
            else
            {
                audio.volume = goal;
                isSlwingMusic = false;
            }
        }
        if (isSpeedingMusic)
        {

        }

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
