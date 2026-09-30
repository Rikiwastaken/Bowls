using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{

    [SerializeField] private Transform PlayerGO;

    [SerializeField] private Transform CamTransform;

    [SerializeField] private float CamElevation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CamTransform.localPosition = PlayerGO.localPosition + new Vector3(0f, CamElevation, 0f);
    }
}
