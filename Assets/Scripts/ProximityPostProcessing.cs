using UnityEngine;
using UnityEngine.Rendering;

public class ProximityPostProcessing : MonoBehaviour
{

    [SerializeField] private Volume ProximityPostProcessingComponent;
    [SerializeField] private float MinRangeForPP;
    [SerializeField] private float MaxRangeForPP;

    private EnemySpawner spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = EnemySpawner.instance;
    }

    // Update is called once per frame
    void Update()
    {
        ProximityPostProcessingComponent.weight = CalculateVolumeRatio();
    }


    private float CalculateVolumeRatio()
    {


        float FillRatio = 0f;

        if (spawner.DistanceToClosestEnemy >= 0)
        {
            // calculate the Ratio depending on the distance

            if (spawner.DistanceToClosestEnemy < MinRangeForPP)
            {
                FillRatio = 1f;
            }
            else if (spawner.DistanceToClosestEnemy > MaxRangeForPP)
            {
                FillRatio = 0f;
            }
            else
            {
                FillRatio = (spawner.DistanceToClosestEnemy - MinRangeForPP) / (MaxRangeForPP - MinRangeForPP);
            }
        }



        return FillRatio;

    }
}
