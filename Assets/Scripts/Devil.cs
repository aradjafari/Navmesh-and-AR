using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Devil : MonoBehaviour
{
    NavMeshAgent agent;

    private void Start()
    {
        agent = GameObject.FindObjectOfType<NavMeshAgent>().GetComponent<NavMeshAgent>();    
    }

    void Update()
    {
        //Vector3 targetDelta = agent.transform.position - transform.position;
        //float angleToTarget = Vector3.Angle(transform.forward, targetDelta);
        //Vector3 turnAxis = Vector3.Cross(transform.forward, targetDelta);

        //transform.RotateAround(transform.position, turnAxis, Time.deltaTime * 10 * angleToTarget);

        transform.LookAt(agent.transform);
    }
}
