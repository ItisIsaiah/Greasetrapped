using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FOV : MonoBehaviour
{


    [Range(0, 180)]
    public float angle;
    public float radius;
    public bool canSee;
    public GameObject objectseen;
    public GameObject[] targets;

    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstructionMask;

    // Start is called before the first frame update
    void Start()
    {
        if (targets == null)
        {
            targets = GameObject.FindGameObjectsWithTag("Player");
        }

        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FOVRoutine()
    {

        WaitForSeconds wait = new WaitForSeconds(.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distance2target = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distance2target, obstructionMask))
                {
                    canSee = true;
                    objectseen=rangeChecks[0].gameObject;
                }
                else
                {
                    canSee = false;
                    objectseen = null;
                }
            }
            else
            {
                canSee = false;
                objectseen=null;
}
        }
        else if (canSee)
        {
            canSee = false;
            objectseen = null;  
        }

    }
}
