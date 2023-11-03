using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] GameObject Player;
    GameObject ground;

    [SerializeField] List<GameObject> objectsHit = new List<GameObject>();
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Player)
        {
            objectsHit.Add(collision.gameObject);
            Player.GetComponent<PlayerMovement>().isOnGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject != Player)
        {
            objectsHit.Remove(collision.gameObject);
            if(objectsHit.Count == 0)
            {
                Player.GetComponent<PlayerMovement>().isOnGround = false;
            }
        }
    }


}
