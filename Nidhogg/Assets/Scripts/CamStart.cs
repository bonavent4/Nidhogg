using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamStart : MonoBehaviour
{
    [SerializeField] bool startFadingOut;
    [SerializeField] Image[] startText;
    [SerializeField] float fadeOutSpeed;
    Animator anim;
    [SerializeField] GameObject SevenSquares;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            anim.SetBool("StartGame", true);   
        }

        if (startFadingOut)
        {
            foreach (Image I in startText)
            {
                var tempColor = I.color;
                tempColor.a -= Time.deltaTime * fadeOutSpeed;
                I.color = tempColor;
            }
            if(startText[0].color.a < 0 && startText[1].color.a < 0)
            {
                startFadingOut = false;
                
            }
        }
    }
    public void SlowlyFadeOutText()
    {
        startFadingOut = true;
    }

    public void StartPlaying()
    {
        SevenSquares.SetActive(true);
        Debug.Log("IsActive");
        anim.enabled = false;
    }
}
