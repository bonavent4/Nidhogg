using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartSpin : MonoBehaviour
{

    float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = Random.Range(-100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
