using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlowMovement : MonoBehaviour
{
    Camera cam;
    [SerializeField] float dividedBy;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(cam.transform.position.x / dividedBy, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
