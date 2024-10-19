using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyParent : MonoBehaviour
{
    [Header("Enemy parameters")]
    [SerializeField] protected int health;

    [Header("Path following")]
    protected NavMeshAgent agent;
    [SerializeField] protected Vector3[] patrolPoints;
    protected int currentPatrolIndex;
    private bool goingForward = true;
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
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public virtual void PathWalking()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (goingForward)
            {
                // Перехід до наступної точки в напрямку вперед
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = patrolPoints.Length - 1;
                    goingForward = false;
                }
            }
            else
            {
                // Перехід до наступної точки в напрямку назад
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
    public virtual void TakeDamage(int damageValue)
    {
        Health -= damageValue;
    }
}
