using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnim : MonoBehaviour
{
    public void Respawn()
    {
        gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>().PlayerRespawn();
    }
}
