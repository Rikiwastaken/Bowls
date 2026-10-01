using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{


    private NavMeshAgent agent;

    private Transform PlayerTransform;


    [SerializeField] private Animator EnemyAnimator;

    [Header("Map Variables")]
    [Header("Types: 1: Follower, 2: RandomPatroller, 3: TargettedPatroller, 4: DelayedFollower")]

    [SerializeField] private int EnemyType = -1;
    // Variables for type 2 and 3
    private float TimeBeforeAIResetsCounter;
    [SerializeField] private float TimeBeforeAIResets;
    [SerializeField] private float MinDistanceBeforeTargetChange;
    [SerializeField] private float DetectionDistance;

    //Variables for type 3
    private float TimeBeforeChaseIsAbandonnedCounter;
    [SerializeField] private float TimeBeforeChaseIsAbandonned;
    private bool Chasing;


    //Variables for type 4
    private float TimeBeforeTakingPositionIntoAccountCounter;
    [SerializeField] private float TimeBeforeTakingPositionIntoAccount;
    private List<Vector3> PreviousPositions = new List<Vector3>();



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
            case 4:
                DelayedFollowerAI();
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

    private void DelayedFollowerAI()
    {
        PreviousPositions.Add(PlayerTransform.localPosition);
        if (TimeBeforeTakingPositionIntoAccountCounter == 0)
        {
            TimeBeforeTakingPositionIntoAccountCounter = Time.time + TimeBeforeTakingPositionIntoAccount;
        }
        else if (Time.time > TimeBeforeTakingPositionIntoAccountCounter)
        {
            agent.destination = PreviousPositions[0];
            PreviousPositions.RemoveAt(0);
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
