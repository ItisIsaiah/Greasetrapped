using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FrybroCore : NetworkBehaviour
{
    FState state;
    public NavMeshAgent agent;
    public GameObject[] playerList;
    public GameObject[] fries;


    public ChaseState chaseState = new ChaseState();
    public PatrolState patrolState = new PatrolState();
    public LurkState lurkState = new LurkState();
    // Start is called before the first frame update
    void Start()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        ChangeState(lurkState);
        
    } 
        
          
        
    

    // Update is called once per frame
    void Update()
    {
        state.UpdateState(this);
    }

    public void ChangeState(FState newState)
    {
        state.OnExit(this);
        state = newState;
        state.OnEnter(this);
    }
}
