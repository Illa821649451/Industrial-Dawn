using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyParent : MonoBehaviour
{
    [SerializeField] protected int health;
    private NavMeshAgent agent;
    [SerializeField] protected List<Vector3> patrolPoints;
    protected int currentPatrolIndex;
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
    
    public virtual void TakeDamage(int damageValue)
    {
        Health -= damageValue;
    }
}
