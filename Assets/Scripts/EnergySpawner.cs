using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{

    public static EnergySpawner Instance;

    [SerializeField] private Transform EnergyPositionHolder;
    [SerializeField] private TextMeshProUGUI PickedUpEnergyText;
    [SerializeField] private float TimeBetweenEnergySpawns;
    private float TimeBetweenEnergySpawnsCounter;
    [SerializeField] private GameObject EnergyPrefab;
    private List<GameObject> EnergyList = new List<GameObject>();
    private Transform PlayerTransform;


    public int EnergyPickedUp;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnergy(Vector3.zero);
        PlayerTransform = ShockCalculator.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnergyList.Count < EnergyPositionHolder.childCount)
        {
            if (Time.time > TimeBetweenEnergySpawnsCounter)
            {
                TimeBetweenEnergySpawnsCounter = Time.time + TimeBetweenEnergySpawns;
                SpawnEnergy(PlayerTransform.transform.position);
            }
        }
    }

    private void SpawnEnergy(Vector3 CurrentPos)
    {
        Vector3 Spawnpos = GetRandomSpawnPoint(CurrentPos);
        GameObject NewEnergy = Instantiate(EnergyPrefab);
        EnergyList.Add(NewEnergy);
        NewEnergy.transform.SetParent(transform);
        NewEnergy.transform.position = Spawnpos;
        NewEnergy.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
    }

    private Vector3 GetRandomSpawnPoint(Vector3 CurrentPosition)
    {
        List<Vector3> PotentialPositions = new List<Vector3>();
        foreach (Transform child in EnergyPositionHolder)
        {
            PotentialPositions.Add(child.position);
        }

        if (CurrentPosition != Vector3.zero)
        {
            Vector3 closestPosition = Vector3.zero;
            float mindist = Mathf.Infinity;
            foreach (Vector3 position in PotentialPositions)
            {
                float distance = Vector3.Distance(position, CurrentPosition);
                if (distance < mindist)
                {
                    mindist = distance;
                    closestPosition = position;
                }
            }

            PotentialPositions.Remove(closestPosition);
        }

        return PotentialPositions[Random.Range(0, PotentialPositions.Count)];
    }

    public void EnergyTouched(GameObject Energy)
    {
        EnergyPickedUp++;
        PickedUpEnergyText.text = ": " + EnergyPickedUp + "/10";
        EnergyList.Remove(Energy);
        Destroy(Energy);
    }
}
