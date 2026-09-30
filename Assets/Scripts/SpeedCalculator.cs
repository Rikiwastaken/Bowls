using System.Collections.Generic;
using UnityEngine;

public class SpeedCalculator : MonoBehaviour
{
    public static SpeedCalculator instance;
    private List<Vector3> previouscoordinates = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    // Update is called once per frame
    void Update()
    {



        previouscoordinates.Add(transform.localPosition);
        if (previouscoordinates.Count > 30)
        {
            previouscoordinates.RemoveAt(0);
        }
    }

    public Vector3 GetImmediateLocalSpeed()
    {
        Vector3 Currentposition = transform.localPosition;

        Vector3 currentSpeed = Vector3.zero;

        for (int i = 0; i < previouscoordinates.Count - 1; i++)
        {
            currentSpeed += (previouscoordinates[i + 1] - previouscoordinates[i]) / Time.deltaTime;
        }


        currentSpeed = currentSpeed / (previouscoordinates.Count - 1);



        return currentSpeed;

    }
}
