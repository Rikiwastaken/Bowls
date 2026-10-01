using UnityEngine;

public class EnergyScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShockCalculator>() != null)
        {
            Spawner.EnergyTouched(gameObject);
        }
    }

    private EnergySpawner Spawner;

    private void Start()
    {
        Spawner = EnergySpawner.Instance;
    }

}
