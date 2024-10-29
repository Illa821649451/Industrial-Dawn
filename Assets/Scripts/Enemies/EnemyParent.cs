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
    [SerializeField] protected bool isElite;

    protected NavMeshAgent agent;
    [SerializeField] protected List<Vector3> patrolPoints;
    protected int currentPatrolIndex;
    private bool goingForward = false;
    private bool changingPoint = false;
    private bool goingToLastKnown;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    public Vector3 lastKnownPosition;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

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
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        DetectionSlider = slider.GetComponent<Slider>();
    }
    public virtual IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    public virtual void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 2.34f, transform.position.z), radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    lastKnownPosition = playerRef.transform.position;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }

        }
        else if (canSeePlayer)
        {
            if (isElite)
            {
                changingPoint = true;
                agent.SetDestination(lastKnownPosition);
                goingToLastKnown = true;
            }
            canSeePlayer = false;
        }

    }
    public void Update()
    {
        PathWalking();
        PatrolingArea();
        if (goingToLastKnown)
        {
            if (agent.remainingDistance == 0)
            {
                StartCoroutine(InspectingDelay());
            }
        }
    }
    public virtual void PatrolingArea()
    {
        if (canSeePlayer && !isDetected)
        {
            DetectionSlider.value += 1 * Time.deltaTime;
            agent.isStopped = true;
            if (agent.isStopped == true)
            {
                Vector3 lookPosition = new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z);
                transform.LookAt(lookPosition);
            }
            if (DetectionSlider.value >= DetectionSlider.maxValue)
            {
                isDetected = true;
            }
        }
        else if (!canSeePlayer)
        {
            DetectionSlider.value -= 1 * Time.deltaTime;
            agent.isStopped = false;
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
        if (!goingToLastKnown)
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
            changingPoint = false;
        }
        else { yield return null; }
    }

    IEnumerator InspectingDelay()
    {
        goingToLastKnown = false;
        yield return new WaitForSeconds(5);
        agent.SetDestination(patrolPoints[currentPatrolIndex]);
        changingPoint = false;
    }
    public virtual void TakeDamage(int damageValue)
    {
        Health -= damageValue;
    }
}
