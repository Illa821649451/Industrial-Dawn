using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyParent : MonoBehaviour
{
    [Header("Enemy parameters")]
    [SerializeField] protected int health;


    protected NavMeshAgent agent;
    [SerializeField] protected List<Vector3> patrolPoints;
    protected int currentPatrolIndex;
    private bool goingForward = false;
    private bool changingPoint = false;

    protected Slider DetectionSlider;
    protected bool isDetected = false;
    protected bool playerInTrigger;
    public int Health
    {
        get { return health; }
        set
        {
            if (value <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                health = value;
            }
        }
    }
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject slider = transform.Find("Canvas/DetectionSlider").gameObject;
        DetectionSlider = slider.GetComponent<Slider>();
    }
    public void Update()
    {
        PathWalking();
        if(!playerInTrigger && !isDetected)
        {
            DetectionSlider.value -= 1;
        } 
    }
    public virtual void PathWalking()
    {
        if (agent.remainingDistance == agent.stoppingDistance && !agent.pathPending && changingPoint == false)
        {
            changingPoint = true;
            StartCoroutine(WalkingDelay());
            
        }
    }

    IEnumerator WalkingDelay()
    {
        yield return new WaitForSeconds(3f);
        if (goingForward)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Count)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
                goingForward = false;
            }
        }
        else
        {
            currentPatrolIndex--;
            if (currentPatrolIndex < 0)
            {
                currentPatrolIndex = 0;
                goingForward = true;
            }
        }
        agent.SetDestination(patrolPoints[currentPatrolIndex]);
        changingPoint = false;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    public virtual void OnTriggerStay(Collider other)
    {
        if(!isDetected && playerInTrigger)
        {
            DetectionSlider.value += 1;
            agent.isStopped = true;
            if(agent.isStopped == true)
            {
                transform.LookAt(other.transform.position);
            }
            if (DetectionSlider.value >= DetectionSlider.maxValue)
            {
                isDetected = true;
            }
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.isStopped = false;
        }
    }

    public virtual void TakeDamage(int damageValue)
    {
        Health -= damageValue;
    }
}
