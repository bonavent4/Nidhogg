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

    [SerializeField] GameObject[] Players;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
       
        if(Players[0].GetComponent<PlayerMovement>().hasChosenColor && Players[1].GetComponent<PlayerMovement>().hasChosenColor && !Players[0].GetComponent<PlayerMovement>().HaveStarted)
        {
            anim.SetBool("StartGame", true);

            foreach (GameObject g in Players)
            {
                g.GetComponent<PlayerMovement>().HaveStarted = true;
                g.GetComponent<PlayerMovement>().ChooseText.gameObject.SetActive(false);
                g.GetComponent<PlayerMovement>().sword.GetComponent<Sword>().LightsaberEnd.GetComponent<SpriteRenderer>().color = g.GetComponent<PlayerMovement>().LightsaberFinalColor;
            }
            
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
                foreach (Image I in startText)
                {
                    I.enabled = false;
                }
                
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
        anim.enabled = false;
    }
}
