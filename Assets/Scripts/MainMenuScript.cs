using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    private PersistentScript _PersistentScript;

    private void Start()
    {
        _PersistentScript = PersistentScript.instance;

    }

    public void CollectionChosen()
    {
        _PersistentScript.ChosenMode = 0;
        SceneManager.LoadScene("MainScene");
    }

    public void SurvivalChosen()
    {
        _PersistentScript.ChosenMode = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void FastRunChosen()
    {
        _PersistentScript.ChosenMode = 2;
        SceneManager.LoadScene("MainScene");
    }
}
