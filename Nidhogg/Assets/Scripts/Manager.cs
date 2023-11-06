using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject[] sevenSquares;

    GameObject PlayerToFollow;
    GameObject playerThatLost;
    [SerializeField]int gameState = 4;

    Camera cam;
    [SerializeField] float followSpeed;
    [SerializeField] float NotQuickSpeed;

    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;

    [SerializeField] float[] endPoints;
    [SerializeField] float endOfStage;
    int stage = 0;
    int lessThanEndPoint;
    int moreThanEndPoint;

    

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
        if (PlayerToFollow != null)
        {
            if(gameState >= 4)
            {
                lessThanEndPoint = stage;
                moreThanEndPoint = stage + 1;
            }
            else 
            {
                lessThanEndPoint = stage + 1;
                moreThanEndPoint = stage;
            }
            
            if(PlayerToFollow.transform.position.x <= -endPoints[lessThanEndPoint] || PlayerToFollow.transform.position.x >= endPoints[moreThanEndPoint])
            {
                if(cam.transform.position.x > 0)
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(endPoints[moreThanEndPoint], cam.transform.position.y, cam.transform.position.z), followSpeed);
                    if (PlayerToFollow.transform.position.x > endPoints[moreThanEndPoint] + endOfStage)
                    {
                        gameState++;
                        changeScene();
                    }
                }
                else
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(-endPoints[lessThanEndPoint], cam.transform.position.y, cam.transform.position.z), followSpeed);
                    if (PlayerToFollow.transform.position.x < -endPoints[lessThanEndPoint] - endOfStage)
                    {
                        gameState--;
                        changeScene();
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

        if (!Player.GetComponent<PlayerMovement>().isPlayer1)
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
        stage += 2;
        
    }
}
