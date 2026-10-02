using TMPro;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private float InvincibilityTime;
    [SerializeField] private GameOverScript _GameOverScript;
    private float InvincibilityCounter;
    public int RemainingLives;
    private Vector3 StartPosition;


    private void Start()
    {
        StartPosition = transform.position;
        HPText.text = ": " + RemainingLives;
    }

    private void Update()
    {
        if (transform.position.y < -50)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (Time.time > InvincibilityCounter)
        {

            RemainingLives--;

            if (RemainingLives <= 0)
            {
                _GameOverScript.OpenMenu();
            }
            else
            {
                EnemySpawner.instance.ResetEnemyPosition();
                GroundMovement.instance.StabilizePlane(true);
                ShockCalculator.instance.ShockState = true;
                transform.position = StartPosition;
                HPText.text = ": " + RemainingLives;
            }


        }
    }
}
