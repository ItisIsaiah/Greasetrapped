using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FrybroCore : NetworkBehaviour
{
    FState state;
    public NavMeshAgent agent;
    public GameObject[] playerList;
    public GameObject[] fries;
    public Transform[] RoomPoints;

    public ChaseState chaseState = new ChaseState();
    public PatrolState patrolState = new PatrolState();
    public LurkState lurkState = new LurkState();
    public Animator animator;
    public FOV sight;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();   
        //state = new LurkState();
        sight = GetComponent<FOV>();
        playerList = GameObject.FindGameObjectsWithTag("Player");
        ChangeState(patrolState);
        
    } 
        
          
        
    

    // Update is called once per frame
    void Update()
    {
        state.UpdateState(this);
    }


    public void ChangeState(FState newState)
    {
        if (state != null)
        {
            state.OnExit(this);
        }
        state = newState;
        state.OnEnter(this);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (state == chaseState && other.CompareTag("Player") ) { 
        
                // Kill animation

        }

    }
}
