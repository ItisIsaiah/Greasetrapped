using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Fries : NetworkBehaviour
{
    public float radius = 2f;
    public NavMeshAgent agent;
    [SerializeField]
    Transform testPlayer;
    bool foundSomething;
    FOV sight;
    Collider c;
    Transform posFound;
    Rigidbody rb;
    FrybroCore f;
    
    // Start is called before the first frame update
    void Start()
    {
        sight = GetComponent<FOV>();
        foundSomething =false;
        agent = GetComponent<NavMeshAgent>();
        c=GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        c.enabled = false;
        rb.useGravity = false;
        //FindCertainPlayer(testPlayer);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!IsServer) return;
        if (sight.canSee)
        {

            ReturntoFrybro(f, true);

        }


    }

    public void FindCertainPlayer(Transform p,Transform returnPos,FrybroCore f)
    {
        if (!IsServer) return;
        foundSomething = false;

        if (p != null)
        {
            Vector3 randomPosition = GetRandomPositionWithinRadius(p.position, radius);
            randomPosition.y = 0;
            
            Debug.Log($"Random Position: {randomPosition}");
            if (CanAgentReachTarget(agent, randomPosition))
            {
                agent.SetDestination(randomPosition);
                
                //If you find the target
                    // ReturntoFrybro(l.f.position,true)

            }
            else
            {
                radius = radius / 2;
                FindCertainPlayer(p,returnPos,f);
            }
        }
        
    }

    public void ReturntoFrybro(FrybroCore f,bool foundSomething)
    {
        if (!IsServer) return;
        agent.SetDestination(f.transform.position);
        if (Vector3.Distance(f.transform.position, transform.position) < .3f) { 
            if( foundSomething){
                f.Hint(posFound);
                posFound = null;
            }
            c.enabled = false;
            rb.useGravity = false;
            transform.position = Vector3.zero;

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
