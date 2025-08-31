using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLogic : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private TextMeshProUGUI _healthDisplayer;
    [SerializeField] private GameObject _aliveIcon;
    [SerializeField] private GameObject _deathIcon;

    private void Start()
    {
        _aliveIcon.SetActive(true);
        _deathIcon.SetActive(false);
    }

    private void Update()
    {
        float _playerCurrentHealth = _player.GetCurrentHealth;
        float _playerMaxHealth = _player.GetMaxHealth;
        SliderImplementation(_playerCurrentHealth, _playerMaxHealth);
        HealthLabelImplementation(_playerCurrentHealth, _playerMaxHealth);
        SwitchHeartIcon(_playerCurrentHealth);
    }

    private void SliderImplementation(float currentHealth, float maxHealth)
    {
        _healthBarSlider.value = currentHealth;
        _healthBarSlider.maxValue = maxHealth;
    }

    private void HealthLabelImplementation(float currentHealth, float maxHealth)
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        _healthDisplayer.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(maxHealth)}";
    }

    private void SwitchHeartIcon(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            _aliveIcon.SetActive(false);
            _deathIcon.SetActive(true);
        }
    }
}
