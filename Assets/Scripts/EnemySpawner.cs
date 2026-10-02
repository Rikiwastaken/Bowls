
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;
    [SerializeField] private TextMeshProUGUI TimerText;

    [SerializeField] private List<GameObject> EnemyPrefabs;

    [SerializeField] private float MinDistToSpawnEnemy;

    [SerializeField] private float TimeBetweenEnemySpawns;
    private float TimeBetweenEnemySpawnsCounter;

    private List<int> remainingEnemyID;

    [Header("Global Enemy Variable")]
    public List<GameObject> SpawnedEnemies;
    public float DistanceToClosestEnemy;
    private Transform playertransform;

    private PersistentScript _PersistentScript;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playertransform = ShockCalculator.instance.transform;
        _PersistentScript = PersistentScript.instance;

        TimeBetweenEnemySpawnsCounter = TimeBetweenEnemySpawns;
        remainingEnemyID = new List<int>();
        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            remainingEnemyID.Add(i);
        }
        if (_PersistentScript.ChosenMode == 2)
        {
            foreach (GameObject enemy in EnemyPrefabs)
            {
                SpawnEnemy();
            }
        }



    }

    private void Update()
    {

        TickDownEnemySpawn();

        GameObject ClosestEnemy = null;
        DistanceToClosestEnemy = 9999;

        foreach (GameObject Enemy in SpawnedEnemies)
        {
            float distance = Vector3.Distance(Enemy.transform.position, playertransform.position);
            if (ClosestEnemy == null || distance < DistanceToClosestEnemy)
            {
                ClosestEnemy = Enemy;
                DistanceToClosestEnemy = distance;
            }
        }


        if (DistanceToClosestEnemy == 9999)
        {
            DistanceToClosestEnemy = -1;
        }
    }

    private void TickDownEnemySpawn()
    {
        if (TimeBetweenEnemySpawnsCounter <= 0)
        {
            SpawnEnemy();
            TimeBetweenEnemySpawnsCounter = TimeBetweenEnemySpawns * (SpawnedEnemies.Count / 4f + 1f);
        }
        else
        {
            TimeBetweenEnemySpawnsCounter -= Time.deltaTime;
        }

        TimerText.text = "Next Sentry arrives in: " + (int)TimeBetweenEnemySpawnsCounter;

    }

    private void SpawnEnemy()
    {
        int EnemyID;
        if (remainingEnemyID.Count > 0)
        {
            EnemyID = remainingEnemyID[Random.Range(0, remainingEnemyID.Count)];
        }
        else
        {
            EnemyID = Random.Range(0, EnemyPrefabs.Count);
        }


        Vector3 SpawnPosition = Vector3.zero;

        int safeguard = 0;
        while (SpawnPosition == Vector3.zero && safeguard < 300)
        {
            SpawnPosition = RandomNavmeshLocation(50);
            if (Vector3.Distance(playertransform.position, SpawnPosition) < MinDistToSpawnEnemy)
            {
                SpawnPosition = Vector3.zero;
            }

            foreach (GameObject Enemy in SpawnedEnemies)
            {
                if (Vector3.Distance(playertransform.position, SpawnPosition) < MinDistToSpawnEnemy)
                {
                    SpawnPosition = Vector3.zero;
                }
            }
            safeguard++;
        }

        if (SpawnPosition == Vector3.zero)
        {
            Debug.LogError("could not find suitable spawn pos");
        }
        else
        {
            GameObject newenemy = Instantiate(EnemyPrefabs[EnemyID]);
            newenemy.transform.position = SpawnPosition;
            SpawnedEnemies.Add(newenemy);
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

#if UNITY_EDITOR
    [ContextMenu("SpawnRandomEnemy")]
    void SpawnEnemyHelper()
    {
        SpawnEnemy();
    }
#endif

}
