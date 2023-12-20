using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public SC_DamageReceiver player;
    public Texture crosshairTexture;
    public float spawnInterval = 2;
    public int enemiesPerWave = 5;
    public Transform[] spawnPoints;

    float nextSpawnTime = 0;
    int waveNumber = 1;
    bool waitingForWave = true;
    float newWaveTimer = 0;
    int enemiesToEliminate;

    int enemiesEliminated = 0;
    int totalEnemiesSpawned = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        newWaveTimer = 10;
        waitingForWave = true;
    }

    private void Update()
    {
        if(waitingForWave)
        {
            if(newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;
            }
            else
            {
                enemiesToEliminate = waveNumber * enemiesPerWave;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave = false;
            }
        }
        else
        {
            if(Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;

                if(totalEnemiesSpawned < enemiesToEliminate)
                {
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                    SC_NPCEnemy npc = enemy.GetComponent<SC_NPCEnemy>();
                    npc.playerTransform = player.transform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }
        if(player.playerHP <= 0)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }
    /*
    private void OnGUI()
    {
        GUI.Box(new Rect(10, Screen.height - 35, 100, 25), ((int)player.playerHP).ToString() + " HP");
        GUI.Box(new Rect(Screen.width / 2 - 35, Screen.height - 35, 70, 25), player.weaponManager.selectedWeapon.bulletsPerMagazine.ToString());

        if(player.playerHP <= 0)
        {
            GUI.Box(new Rect(Screen.width / 2 - 85, Screen.height / 2 - 20, 170, 40), "Game Over\n(Press 'Space' to Restart)");
        }
        else
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);
        }
        GUI.Box(new Rect(Screen.width / 2 - 50, 10, 100, 25), (enemiesToEliminate - enemiesEliminated).ToString());

        if(waitingForWave)
        {
            GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 4 - 12, 250, 25), "Waiting for Wave " + waveNumber.ToString() + " (" + ((int)newWaveTimer).ToString() + " seconds left...)");
        }
    }
    */

    public void EnemyEliminated(SC_NPCEnemy enemy)
    {
        enemiesEliminated++;

        if(enemiesToEliminate - enemiesEliminated <= 0)
        {
            newWaveTimer = 10;            
            waitingForWave = true;
            waveNumber++;
        }
    }
}
