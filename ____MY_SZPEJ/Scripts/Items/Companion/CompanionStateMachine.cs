using UnityEngine;
using UnityEngine.AI;

public class CompanionStateMachine : MonoBehaviour
{
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float agroRange = 6f;
    private Transform player;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        GameObject target = FindNearestEnemy();

        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        else if (Vector3.Distance(transform.position, player.position) > followDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float closest = agroRange;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closest)
            {
                closest = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
