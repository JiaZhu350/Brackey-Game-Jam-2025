using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _biscuitUI;
    [SerializeField] private TextMeshProUGUI _biscuitText;
    [SerializeField] private GameObject _pauseMenu;


    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    public void UpdateBiscuitUI()
    {
        _biscuitText.text = _player.GetBiscuit().ToString();
    }

    // enable and disable biscuit UI
    public void ToggleBiscuitUI()
    {
        if (_biscuitUI.activeSelf)
        {
            _biscuitUI.SetActive(false);
        }
        else
        {
            _biscuitUI.SetActive(true);
            UpdateBiscuitUI();
        }
    }

    public void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
