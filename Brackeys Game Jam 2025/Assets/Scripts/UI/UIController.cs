using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _biscuitUI;
    [SerializeField] private TextMeshProUGUI _biscuitText;

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
}
