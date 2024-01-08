using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform enemyTarget;

    public static EnemyAI instance;

    public Transform[] patrolWaypoints;
    public Vector3 patrolAreaCenter;
    public float patrolAreaRadius = 10f;

    private int currentWaypointIndex;

    public GameObject bullet;

    public Animator anim;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyTarget = GameObject.FindGameObjectWithTag("Player").transform;

        // Set initial waypoint for patrolling
        SetNextWaypoint();
    }

    void Update()
    {
        if (enemyTarget != null)
        {
            // Check if the player is within the line of sight
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (enemyTarget.position - transform.position).normalized, out hit, patrolAreaRadius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (IsPlayerWithinPatrolArea())
                    {
                        // Player is within the patrol area, follow the player
                        agent.SetDestination(enemyTarget.position);
                        // agent.isStopped = true;
                        // anim.SetBool("Aim", true);
                        transform.LookAt(enemyTarget.position);
                        spawnFire();
                        return; // Exit the Update method to prevent further movement logic
                    }
                }
            }

            // If the player is not in line of sight or not within the patrol area, resume patrolling
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Patrol();
            }
        }
    }


    bool IsPlayerWithinPatrolArea()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, enemyTarget.position);
        return distanceToPlayer < patrolAreaRadius;
    }

    void Patrol()
    {
        if (patrolWaypoints.Length > 0)
        {
            if (Vector3.Distance(transform.position, patrolWaypoints[currentWaypointIndex].position) < 0.5f)
            {
                // Reached the current waypoint, set the next waypoint
                SetNextWaypoint();
            }
            else
            {
                SetPrevousWaypoint();
            }

            // Move towards the current waypoint
            agent.SetDestination(patrolWaypoints[currentWaypointIndex].position);
        }
    }

    public void SetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
    }

    public void SetPrevousWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex - 1 + patrolWaypoints.Length) % patrolWaypoints.Length;
    }

    void spawnFire()
    {
        GameObject _enemyFire = PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
        Destroy(_enemyFire, 2f);
    }

    public void SetEnemyTarget(Transform target)
    {
        enemyTarget = target;
    }
}
