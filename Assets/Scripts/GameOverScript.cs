using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI survivalTimeTMP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;


        Timer _Timer = Timer.instance;
        _Timer.StopTimer = true;
        int SecondsSurvived = (int)_Timer.TimeSinceStarted;

        int Minutes = SecondsSurvived / 60;
        int seconds = SecondsSurvived % 60;

        survivalTimeTMP.text = "You survived ";
        if (Minutes > 0)
        {
            survivalTimeTMP.text += Minutes + " minutes and ";
        }
        survivalTimeTMP.text += seconds + " seconds.";
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
