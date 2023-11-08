using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] GameObject[] bodyParts;
    GameObject CurrentPart;
    [SerializeField] float ThrowForce;
    

    private void Update()
    {
        
    }
    public void ExplodeInPieces()
   {
        foreach (GameObject g in bodyParts)
        {
            CurrentPart = Instantiate(g, transform.position,Quaternion.Euler(0,0,Random.Range(0f,360f)));
             //CurrentPart.transform.Rotate(new Vector3(0, 0, Random.Range(-180, 180)));
            CurrentPart.GetComponent<Rigidbody2D>().AddForce((transform.up * ThrowForce) + (transform.right * Random.Range(-1f,1f) * ThrowForce));

            CurrentPart.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-100, 100); 

        }
   }
}
