using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] AudioSource Song;
    AudioSource theMusic;
    AudioSource startTheMusic;
    float goal;
    float startGoal;
    [SerializeField]bool isSlwingMusic;
    [SerializeField]bool isSpeedingMusic;
    [SerializeField] float SlowSpeed;
    [SerializeField] float SuperSlowSpeed;
    float previousVolume;

    Manager manager;

    [SerializeField] int[] States;

    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
        previousVolume = Song.volume;
    }

    private void Update()
    {
       

        SlowlyStopMusic(theMusic, goal, previousVolume);
        SlowlyStartMusic(startTheMusic, startGoal);
    }
   
    
    public void music()
    {
        Song.volume = previousVolume;
        if (manager.gameState == States[0] || manager.gameState == States[1])
        {
            isSlwingMusic = false;
            Debug.Log("Start Music");
            Song.Play();
            startGoal = previousVolume;
            Song.volume = 0;
            isSpeedingMusic = true;
            startTheMusic = Song;
            return;
        }
        else if(manager.previousGamestate == States[0] || manager.previousGamestate == States[1])
        {
            isSpeedingMusic = false;
            Song.volume = previousVolume;
            isSlwingMusic = true;
            theMusic = Song;
            goal = 0;
            
            Debug.Log("Did It anyway");
        }
    }
    void SlowlyStopMusic(AudioSource audio, float goal, float previousV)
    {
        if (isSlwingMusic)
        {
            if (audio.volume > goal)
            {
                audio.volume -= SuperSlowSpeed * Time.deltaTime;
            }
            else
            {
                audio.volume = previousV;
                audio.Stop();
                isSlwingMusic = false;
            }
        }

    }
    void SlowlyStartMusic(AudioSource audio, float goal)
    {
        if (isSpeedingMusic)
        {
            if (audio.volume < goal)
            {

                audio.volume += SlowSpeed * Time.deltaTime;
            }
            else
            {
                audio.volume = goal;
                isSpeedingMusic = false;
            }
        }


    }
}
