using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void About()
    {
        SceneManager.LoadScene("AboutScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
