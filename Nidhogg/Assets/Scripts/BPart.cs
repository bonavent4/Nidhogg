using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPart : MonoBehaviour
{
    float Timer = 0;

    bool startShrinking;

    private void Update()
    {
        Timer += Time.deltaTime;
        if(Timer > 5)
        {
            startShrinking = true;
        }
        if (startShrinking)
        {
            if(transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(.1f * Time.deltaTime, .1f * Time.deltaTime, .1f * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


}
