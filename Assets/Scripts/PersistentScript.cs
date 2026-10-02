using UnityEngine;

public class PersistentScript : MonoBehaviour
{

    public static PersistentScript instance;

    [Header("0: Collection, 1: Survival, 2: Fast Run")]
    public int ChosenMode = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
