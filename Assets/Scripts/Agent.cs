using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    public AnimationClip walk;
    public AnimationClip jump;

    private bool rePosedAtStart = false;

    public void SetRePosedAtStart(bool value)
    {
        rePosedAtStart = value;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        target = GameObject.Find("Target");
    }

    void Update()
    {
        Vector3 tempPos = agent.transform.position;
        tempPos.y = target.transform.position.y;
        agent.transform.position = tempPos;

         if (target && rePosedAtStart)
        {
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
        }

        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100 * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) <= 0.5f)
        {
            GetComponent<Animation>().Play("Jump");
            Vector3 pos = new Vector3(0, 0.135f, 0);
            transform.position = target.transform.position + pos;
        }
        else
        {
            GetComponent<Animation>().Play("Running");
        }
    }
}
