using UnityEngine;

public class ShockCalculator : MonoBehaviour
{
    public static ShockCalculator instance;

    public bool ShockState;

    private Vector3 previousSpeed;

    [SerializeField] private float Shockstateduration;
    [SerializeField] private float SpeedDiffNeededForShockState;
    [SerializeField] private float StartGracePeriod;
    private float timewheenShockstateends;
    private float timewheenGraveperiodEnds;

    private Rigidbody RB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        timewheenGraveperiodEnds = Time.time + StartGracePeriod;
    }

    // Update is called once per frame
    void Update()
    {

        if (ShockState && Time.time > timewheenShockstateends)
        {
            ShockState = false;
        }

        Vector3 currentspeed = RB.linearVelocity;

        if (previousSpeed != null && Mathf.Abs((currentspeed.magnitude - previousSpeed.magnitude)) > SpeedDiffNeededForShockState && Time.time > timewheenGraveperiodEnds)
        {
            ShockState = true;
            timewheenShockstateends = Time.time + Shockstateduration;
        }

        previousSpeed = currentspeed;
    }


}
