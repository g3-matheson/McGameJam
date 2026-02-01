using UnityEngine;
using UnityEngine.AI;

public class NavMeshTester : MonoBehaviour
{

    private NavMeshAgent agent;
    void Awake()
    {
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        agent.speed = 6.0f;
	}

    void Update()
    {
        agent.SetDestination(new Vector3(-22.8f, 3.8f, 0f));
        // Force the agent to stay at z=0
        //Vector3 pos = transform.position;
        //pos.z = 0f;
        //transform.position = pos;
    }

}
