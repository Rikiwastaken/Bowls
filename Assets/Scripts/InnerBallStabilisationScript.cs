using UnityEngine;

public class InnerBallStabilisationScript : MonoBehaviour
{
    [SerializeField] private Transform OuterBall;
    private Rigidbody OuterBallRB;



    private void Start()
    {
        OuterBallRB = OuterBall.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.position = OuterBall.position;

        Vector3 Outerballvelocity = OuterBallRB.linearVelocity;

        Vector3 Lookatpoint = transform.localPosition + SpeedCalculator.instance.GetImmediateLocalSpeed();


        transform.LookAt(Lookatpoint);


    }



}
