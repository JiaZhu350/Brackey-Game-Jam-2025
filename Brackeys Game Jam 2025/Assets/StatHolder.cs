using TMPro;
using UnityEngine;

public class StatHolder : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _resistanceText;

    void Update()
    {
        BuildingSpeed();
        BuildingAttack();
        BuildingResistance();
    }

    private void BuildingSpeed()
    {
        _speedText.text = $"Player Speed: {Mathf.RoundToInt(_player.GetPlayerMoveSpeed())}";
    }

    private void BuildingAttack()
    {
        _attackText.text = $"Player Damage: {Mathf.RoundToInt(_player.GetPlayerDamage())}";
    }

    private void BuildingResistance()
    {
        _resistanceText.text = $"Player Resistance: {Mathf.RoundToInt(_player.GetResistance())}";
    }
}
