using UnityEngine;
using UnityEngine.InputSystem;

public class GroundMovement : MonoBehaviour
{

    public static GroundMovement instance;

    [SerializeField] private float maxangle;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float InputDeadZone;
    [SerializeField] private Transform PlayerCameraTransform;

    private InputAction _Moveaction;
    private InputAction _Stabilize;

    private ShockCalculator _ShockCalculator;

    private bool Stabilizing;
    private float RemainingStabilizingTime;
    [SerializeField] private float StabilizingDuration;
    private Quaternion initialRotation;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Moveaction = InputSystem.actions.FindAction("Move");
        _Stabilize = InputSystem.actions.FindAction("Stabilize");
        _ShockCalculator = ShockCalculator.instance;
    }

    // Update is called once per frame
    void Update()
    {

        if (!_ShockCalculator.ShockState)
        {
            if (!Stabilizing)
            {
                MovePlane();
            }



        }

        StabilizePlane();

    }

    public void StabilizePlane(bool ForceStabilization = false)
    {
        // First we acitvate the inputaction
        if (!_Stabilize.enabled)
        {
            _Stabilize.Enable();
        }

        if (Stabilizing)
        {
            if (RemainingStabilizingTime > 0)
            {
                RemainingStabilizingTime -= Time.deltaTime;
                float ratio = 1f - (RemainingStabilizingTime / StabilizingDuration);

                transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.identity, ratio);
            }
            else
            {
                Stabilizing = false;
            }

        }
        else if (ForceStabilization || (_Stabilize.WasPerformedThisFrame() && !_ShockCalculator.ShockState))
        {
            Stabilizing = true;
            RemainingStabilizingTime = StabilizingDuration;
            initialRotation = transform.rotation;
        }

    }


    private void MovePlane()
    {

        // First we acitvate the inputaction
        if (!_Moveaction.enabled)
        {
            _Moveaction.Enable();
        }

        // We Read the input in a Vector3
        Vector2 MoveValue = _Moveaction.ReadValue<Vector2>();

        Vector3 NewRotation = transform.rotation.eulerAngles;



        // If the input is higher than the dead zone, we calculate the new rotation depending on the input
        if (MoveValue.sqrMagnitude > InputDeadZone)
        {

            Vector3 RotationChange = new Vector3(MoveValue.y * MoveSpeed * Time.deltaTime, 0.0f, -MoveValue.x * MoveSpeed * Time.deltaTime);


            NewRotation = transform.rotation.eulerAngles + RotationChange;
        }



        // We then rectify the values if it's higer than the max angle we dicided

        NewRotation = new Vector3(ClampAngle(NewRotation.x, -maxangle, maxangle), NewRotation.y, ClampAngle(NewRotation.z, -maxangle, maxangle));




        // Then we apply the rotation

        transform.rotation = Quaternion.Euler(NewRotation);

    }


    public static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }
}
