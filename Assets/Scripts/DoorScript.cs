using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public Text displayText;
    public GameObject winPanel;

    public AudioClip winTheme;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();//GetComponent<AudioSource>();
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            displayText.text = "Press E to escape level 1!";

            if(Input.GetKeyDown(KeyCode.E))
            {
                audioSource.Stop();
                audioSource.clip = winTheme;
                audioSource.Play();

                Time.timeScale = 0;
                winPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
