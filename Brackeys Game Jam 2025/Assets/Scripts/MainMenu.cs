using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        //Switch scene to main game
        SceneManager.LoadScene("MainGame");
    }

    public void Tutorial()
    {
        //Switch scene to tutorial
        SceneManager.LoadScene("");
    }
}
