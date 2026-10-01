using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{


    private NavMeshAgent agent;

    private Transform PlayerTransform;


    [SerializeField] private Animator EnemyAnimator;

    [Header("Map Variables")]
    [Header("Types: 1: Follower, 2: RandomPatroller, 3: TargettedPatroller")]

    [SerializeField] private int EnemyType = -1;

    private float TimeBeforeAIResetsCounter;
    [SerializeField] private float TimeBeforeAIResets;
    [SerializeField] private float MinDistanceBeforeTargetChange;
    [SerializeField] private float DetectionDistance;
    private float TimeBeforeChaseIsAbandonnedCounter;
    [SerializeField] private float TimeBeforeChaseIsAbandonned;
    private bool Chasing;

    [Header("Map Variables")]
    [SerializeField] private float MapRadius;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PlayerTransform = SpeedCalculator.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {

        EnemyAnimator.SetFloat("Speed", agent.velocity.magnitude);

        switch (EnemyType)
        {
            case 1:
                agent.destination = PlayerTransform.position;
                break;
            case 2:
                RandomPatrollerAI();
                break;
            case 3:
                TargettedPatrollerAI();
                break;

        }

    }

    private void RandomPatrollerAI()
    {



        if (Time.time > TimeBeforeAIResetsCounter || Vector3.Distance(transform.position, agent.destination) <= MinDistanceBeforeTargetChange)
        {
            agent.destination = RandomNavmeshLocation(MapRadius / 2);
            TimeBeforeAIResetsCounter = Time.time + TimeBeforeAIResets;
        }
    }

    private void TargettedPatrollerAI()
    {
        if (Chasing)
        {
            agent.destination = PlayerTransform.position;
            if (Time.time > TimeBeforeChaseIsAbandonnedCounter)
            {
                Chasing = false;
            }
        }
        else
        {
            if (Time.time > TimeBeforeAIResetsCounter || Vector3.Distance(transform.position, agent.destination) <= MinDistanceBeforeTargetChange)
            {
                agent.destination = RandomNavmeshLocation(MapRadius / 2);
                TimeBeforeAIResetsCounter = Time.time + TimeBeforeAIResets;
            }
        }

        if (Vector3.Distance(transform.position, PlayerTransform.position) < DetectionDistance)
        {
            Vector3 Direction = PlayerTransform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Direction, out hit, DetectionDistance))
            {
                if (hit.collider.transform == PlayerTransform)
                {
                    Chasing = true;
                    TimeBeforeChaseIsAbandonnedCounter = Time.time + TimeBeforeChaseIsAbandonned;
                }
            }
        }

    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

}
