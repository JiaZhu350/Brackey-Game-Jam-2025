using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _biscuitUI;
    [SerializeField] private TextMeshProUGUI _biscuitText;

    public void UpdateBiscuitUI(int _amount)
    {
        _biscuitText.text = _amount.ToString();
    }

    // enable and disable biscuit UI
    public void ToggleBiscuit()
    {
        if (_biscuitUI.activeSelf)
        {
            _biscuitUI.SetActive(false);
        }
        else
        {
            _biscuitUI.SetActive(true);
        }
    }
}
