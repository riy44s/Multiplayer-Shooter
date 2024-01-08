using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    [Header("Enemy Health and Damage")]
    public float enemySpeed;

    [Header("Enemy Things")]
    public NavMeshAgent botAgent;
    public Transform playerBody;
    public LayerMask playerLayer;

    [Header("Enemy States")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool isPlayer = false;

    public static Bot instance;

    private void Awake()
    {
        instance = this;
        botAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInshootingRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position,shootingRadius, playerLayer);

        if (playerInvisionRadius && !playerInshootingRadius) Pursueplayer();
    }

    private void Pursueplayer()
    {
        if (botAgent.SetDestination(playerBody.position))
        {

        }
    }
}
