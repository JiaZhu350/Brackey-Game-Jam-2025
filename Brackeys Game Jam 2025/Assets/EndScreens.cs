using TMPro;
using UnityEngine;

public class EndScreens : MonoBehaviour
{
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private TextMeshProUGUI _loseBiscuitCounter;
    [SerializeField] private TextMeshProUGUI _winBiscuitCounter;


    private void Start()
    {
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
    }

    public void PlayerLose(int _biscuitNum)
    {
        Time.timeScale = 0f;
        _loseScreen.SetActive(true);
        _loseBiscuitCounter.text = $"FINAL SCORE: {_biscuitNum} BISCUITS";
    }

    public void PlayerWin(int _biscuitNum)
    {
        Time.timeScale = 0f;
        _winScreen.SetActive(true);
        _winBiscuitCounter.text = $"FINAL SCORE: {_biscuitNum} BISCUITS";
    }
}
