using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_DamageReceiver : MonoBehaviour, IEntity
{
    public float playerHP = 100;
    public SC_CharacterController playerController;
    public SC_WeaponManager weaponManager;
    public GameObject blackKey;

    public Text playerHPText;
    public Slider playerHPSlider;

    public GameObject gameOverPanel;

    private int killCount;

    public void Start()
    {
        killCount = 0;
        blackKey.SetActive(false);
        playerHPSlider.maxValue = playerHP;
    }

    public void Update()
    {
        if(killCount >= 6)
        {
            blackKey.SetActive(true);
        }
    }

    public void KilledAnEnemy()
    {
        killCount++;
    }
        
    public void ApplyDamage(float points)
    {
        playerHP -= points;
        playerHPText.text = playerHP.ToString() + " / 100";
        playerHPSlider.value = playerHP;
        if(playerHP <= 0)
        {
            gameOverPanel.SetActive(true);
            playerController.canMove = false;
            playerHP = 0;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
