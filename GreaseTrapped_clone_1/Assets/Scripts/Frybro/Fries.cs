using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AI;

public class Fries : NetworkBehaviour
{
    public float radius = 10f;
    NavMeshAgent agent;
    [SerializeField]
    Transform testPlayer;
    bool foundSomething;
    FOV sight;
    
    // Start is called before the first frame update
    void Start()
    {
        sight = GetComponent<FOV>();
        foundSomething =false;
        agent = GetComponent<NavMeshAgent>();
        //FindCertainPlayer(testPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsServer) return;


        
    }

    public void FindCertainPlayer(Transform p,Transform returnPos,LurkState l)
    {
        foundSomething= false;
        if (p != null)
        {
            Vector3 randomPosition = GetRandomPositionWithinRadius(p.position, radius);
            
            Debug.Log($"Random Position: {randomPosition}");
            if (CanAgentReachTarget(agent, randomPosition))
            {
                agent.SetDestination(randomPosition);
                //If you find the target
                    // ReturntoFrybro(l.f.position,true)

            }
            else
            {
               // FindCertainPlayer(p);
            }
        }
        
    }

    public void ReturntoFrybro(Transform f,bool foundSomething)
    {
        if (!foundSomething)
        {
            agent.SetDestination(f.position);
        }
    }

    Vector3 GetRandomPositionWithinRadius(Vector3 center, float radius)
    {
        // Generate a random point within a circle
        Vector2 randomPoint = Random.insideUnitCircle * radius;

        // Convert to 3D and offset it to the target position
        Vector3 randomPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + center;

        return randomPosition;
    }


    bool CanAgentReachTarget(NavMeshAgent agent, Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        // Check if the path status is complete
        return path.status == NavMeshPathStatus.PathComplete;
    }
}
