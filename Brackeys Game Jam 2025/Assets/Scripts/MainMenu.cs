using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayCredits()
    {
        //Switch scene to credits scene
    }

    public void StartGame()
    {
        //Switch scene to main game
        SceneManager.LoadScene("MainGame");
    }

    public void Tutorial()
    {
        //Switch scene to tutorial
    }
}
