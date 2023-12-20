using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]

public class SC_NPCEnemy : MonoBehaviour, IEntity
{
    public float attackDistance = 3f;
    public float movementSpeed = 4f;
    public float npcHP = 100;
    public float npcStartingHP = 100;

    public float npcDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;
    public GameObject npcDeadPrefab;

    [SerializeField] Text healthText;
    [SerializeField] Slider sliderValue;

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public SC_EnemySpawner es;
    NavMeshAgent agent;
    //float nextAttackTime = 0;
    float lastAttackTime = 0;
    private SC_DamageReceiver damageReceiver;

    public AudioClip hurtAudio;
    AudioSource audioSource;


    void Start()
    {
        damageReceiver = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_DamageReceiver>();
        healthText = GameObject.FindGameObjectWithTag("EnemyHPText").GetComponent<Text>();
        sliderValue = GameObject.FindGameObjectWithTag("EnemySlider").GetComponent<Slider>();
        npcHP = npcStartingHP;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();

        if(GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        //if(agent.remainingDistance - attackDistance < 0.01f)
        //{
        //    Debug.Log("step 1");
        //    if(Time.time > nextAttackTime)
        //    {
        //        Debug.Log("step 2");
        //        nextAttackTime = Time.time + attackRate;

        //        RaycastHit hit;
        //        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
        //        {
        //            Debug.Log("step 3");
        //            Debug.Log(hit.collider.name);
        //            Debug.Log(hit.collider.tag);
        //            if (hit.transform.CompareTag("Player"))
        //            {
        //                Debug.Log("step 4");
        //                Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

        //                IEntity player = hit.transform.GetComponent<IEntity>();
        //                player.ApplyDamage(npcDamage);
        //            }
        //        }
        //    }
        //}

        agent.destination = playerTransform.position;

        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && (Time.time - lastAttackTime) > attackRate)
        {
            lastAttackTime = Time.time;            
            audioSource.Play();
            other.gameObject.GetComponent<SC_DamageReceiver>().ApplyDamage(npcDamage);            
        }
    }

    public void ApplyDamage(float points)
    {

        sliderValue.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -17, 0);
        npcHP -= points;
        healthText.text = npcHP.ToString() + " / " + npcStartingHP;
        sliderValue.maxValue = npcStartingHP;
        sliderValue.value = npcHP < 0 ? 0 : npcHP;
        if(npcHP <= 0)
        {
            damageReceiver.KilledAnEnemy();
            sliderValue.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 100, 0);
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);

            npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(npcDead, 10);
            es.EnemyEliminated(this); 
            Destroy(gameObject);
        }
    }    
}
