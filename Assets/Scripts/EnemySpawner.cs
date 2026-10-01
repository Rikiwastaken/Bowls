
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;

    [SerializeField] private List<GameObject> EnemyPrefabs;

    [SerializeField] private float MinDistToSpawnEnemy;

    [Header("Global Enemy Variable")]
    public List<GameObject> SpawnedEnemies;
    public float DistanceToClosestEnemy;

    private Transform playertransform;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playertransform = ShockCalculator.instance.transform;
    }

    private void Update()
    {
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

    private void SpawnEnemy()
    {
        int EnemyID = Random.Range(0, EnemyPrefabs.Count);

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
