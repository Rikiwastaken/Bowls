using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySpawner : MonoBehaviour
{

    public static EnergySpawner Instance;

    [SerializeField] private Transform EnergyPositionHolder;
    [SerializeField] private TextMeshProUGUI PickedUpEnergyText;
    [SerializeField] private Image PickedUpEnergyImage;
    [SerializeField] private float TimeBetweenEnergySpawns;
    private float TimeBetweenEnergySpawnsCounter;
    [SerializeField] private GameObject EnergyPrefab;
    private List<GameObject> EnergyList = new List<GameObject>();
    private Transform PlayerTransform;


    public int EnergyPickedUp;

    private PersistentScript _PersistentScript;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerTransform = ShockCalculator.instance.transform;
        _PersistentScript = PersistentScript.instance;
        if (_PersistentScript.ChosenMode == 2)
        {
            foreach (Transform Position in EnergyPositionHolder)
            {
                SpawnEnergy();
            }
        }
        UpdateEnergyHUD();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_PersistentScript.ChosenMode)
        {
            case 0:

                if (EnergyList.Count < EnergyPositionHolder.childCount)
                {
                    if (Time.time > TimeBetweenEnergySpawnsCounter)
                    {
                        TimeBetweenEnergySpawnsCounter = Time.time + TimeBetweenEnergySpawns;
                        SpawnEnergy();
                    }
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }


    }

    private void UpdateEnergyHUD()
    {
        switch (_PersistentScript.ChosenMode)
        {
            case 0:

                if (!PickedUpEnergyImage.gameObject.activeSelf)
                {
                    PickedUpEnergyImage.gameObject.SetActive(true);
                }
                PickedUpEnergyText.text = ": " + EnergyPickedUp + "/10";
                break;
            case 1:
                if (PickedUpEnergyImage.gameObject.activeSelf)
                {
                    PickedUpEnergyImage.gameObject.SetActive(false);
                }
                PickedUpEnergyText.text = "";
                break;
            case 2:
                if (!PickedUpEnergyImage.gameObject.activeSelf)
                {
                    PickedUpEnergyImage.gameObject.SetActive(true);
                }
                PickedUpEnergyText.text = ": " + EnergyPickedUp + "/5";
                break;
        }
    }

    private void SpawnEnergy()
    {
        Vector3 Spawnpos = GetRandomSpawnPoint(PlayerTransform.position);
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

        // First We Remove Positions Already Taken
        List<Vector3> Listpositionstoremove = new List<Vector3>();
        foreach (Vector3 position in PotentialPositions)
        {
            foreach (GameObject Energy in EnergyList)
            {
                if (Vector3.Distance(position, Energy.transform.position) < 1)
                {
                    Listpositionstoremove.Add(position);
                }
            }
        }

        foreach (Vector3 positionstoremove in Listpositionstoremove)
        {
            PotentialPositions.Remove(positionstoremove);
        }

        // Then we remove the closest unless it's the last one.

        if (CurrentPosition != Vector3.zero && PotentialPositions.Count > 1)
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
        UpdateEnergyHUD();
        EnergyList.Remove(Energy);
        Destroy(Energy);
    }
}
