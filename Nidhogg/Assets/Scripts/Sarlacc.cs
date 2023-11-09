using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sarlacc : MonoBehaviour
{

    [SerializeField] GameObject PlayerWinText;

    float timer;
    [SerializeField] float maxTime;
    bool startCountDown;
    private void Update()
    {
        if (startCountDown)
        {
            timer += Time.deltaTime;
            if(timer >= maxTime)
            {
                SceneManager.LoadScene(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            PlayerWinText.SetActive(true);
            collision.gameObject.GetComponent<PlayerMovement>().Die();
            startCountDown = true;
        }
    }
}
