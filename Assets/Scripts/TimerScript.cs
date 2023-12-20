using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float startingTime = 180f;
    public float currentTime;
    public GameObject gameOverPanel;
    public GameObject player;
    public Text timerText;

    public void Start()
    {
        Time.timeScale = 1;
        currentTime = startingTime;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = true;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        timerText.text = currentTime.ToString("F2");

        if(currentTime <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            timerText.text = 0.ToString();
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            player.GetComponent<CharacterController>().enabled = false;
        }

    }
}
