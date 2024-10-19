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

    protected Slider DetectionSlider;
    protected bool isDetected = false;
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
        if(DetectionSlider.value == int.MaxValue)
        {
            isDetected = true;
        }
    }
    public virtual void PathWalking()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
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
        }
    }
    public virtual void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        while (other.gameObject.CompareTag("Player") && !isDetected)
        {
            DetectionSlider.value += 1;
        }
    }

    public virtual void TakeDamage(int damageValue)
    {
        Health -= damageValue;
    }
}
