using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;           // Assign the player Transform in the Inspector
    public float maxDistance = 20f;   // Max ray distance
    public LayerMask raycastMask;      // Set this to include only layers you want to hit (e.g., Player)
    public NavMeshAgent Agent;
    public int hp;

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned!");
            return;
        }
        if (hp < 1)
        {
            Destroy(gameObject);
        }
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Optional: Draw the ray in the editor
        Debug.DrawRay(transform.position, directionToPlayer * maxDistance, Color.green);

        // Perform the raycast
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxDistance, raycastMask))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Agent.destination = player.transform.position;
                Debug.Log("Player hit by raycast!");
                // Add your logic here (e.g., shoot, chase, alert, etc.)
            }
            else
            {
                Debug.Log("Raycast hit something else: " + hit.transform.name);
            }
        }
    }
}
