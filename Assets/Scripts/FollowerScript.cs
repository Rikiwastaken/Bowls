using UnityEngine;
using UnityEngine.AI;

public class FollowerScript : MonoBehaviour
{


    private NavMeshAgent agent;

    private Transform PlayerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PlayerTransform = SpeedCalculator.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = PlayerTransform.position;
    }
}
