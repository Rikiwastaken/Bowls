using UnityEngine;
using UnityEngine.InputSystem;

public class GroundMovement : MonoBehaviour
{

    [SerializeField] private float maxangle;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float InputDeadZone;
    [SerializeField] private Transform PlayerCameraTransform;

    private InputAction _Moveaction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Moveaction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlane(transform);

    }

    private void MovePlane(Transform TransformToRotate)
    {

        // First we acitvate the inputaction
        if (!_Moveaction.enabled)
        {
            _Moveaction.Enable();
        }

        // We Read the input in a Vector3
        Vector2 MoveValue = _Moveaction.ReadValue<Vector2>();

        Vector3 NewRotation = TransformToRotate.rotation.eulerAngles;



        // If the input is higher than the dead zone, we calculate the new rotation depending on the input
        if (MoveValue.sqrMagnitude > InputDeadZone)
        {

            Vector3 RotationChange = new Vector3(MoveValue.y * MoveSpeed * Time.deltaTime, 0.0f, -MoveValue.x * MoveSpeed * Time.deltaTime);


            NewRotation = TransformToRotate.rotation.eulerAngles + RotationChange;
        }



        // We then rectify the values if it's higer than the max angle we dicided

        NewRotation = new Vector3(ClampAngle(NewRotation.x, -maxangle, maxangle), NewRotation.y, ClampAngle(NewRotation.z, -maxangle, maxangle));




        // Then we apply the rotation

        TransformToRotate.rotation = Quaternion.Euler(NewRotation);

    }


    public static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }
}
