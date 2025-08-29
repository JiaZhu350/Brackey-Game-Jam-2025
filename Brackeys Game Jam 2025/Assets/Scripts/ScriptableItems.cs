
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItems", menuName = "Scriptable Objects/ScriptableItems")]
public class ScriptableItems : ScriptableObject
{
    private enum PlayerStatMod { Speed, Health, Attack, Resistance, None }
    private enum RiskType { Normal, AllIn}


    [Header("Item Functionality")]
    [SerializeField] private PlayerStatMod _statModIncrease;
    [SerializeField] private PlayerStatMod _statModDecrease;
    [SerializeField] private RiskType _riskLevel;
    [SerializeField] private float _statIncreaseAmount;
    public float StatIncreaseAmount => _statIncreaseAmount;
    [SerializeField] private float _statDecreaseAmount;
    public float StatDecreaseAmount => _statDecreaseAmount;
    [SerializeField] private int _normalRiskCostReduction; //Does not affect all ins
    [SerializeField] private float _riskUpperLimit;
    [SerializeField] private float _riskLowerLimit;
    private Player _player;
    private PlayerStatMod _statRisked;


    [Header("Item Components")]
    [SerializeField] private Sprite _itemSprite;
    public Sprite ItemSprite => _itemSprite;
    [SerializeField] private string _itemName;
    public string ItemName => _itemName;
    [SerializeField] private int _initialBiscuitCost;
    public int CostOfItem => _initialBiscuitCost;
    [TextArea][SerializeField] private string _itemDescription;
    public string ItemDescrition => _itemDescription;


    public void ApplyStats()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        IncreaseStats(_statIncreaseAmount);
        DecreaseStats(-1f * _statDecreaseAmount);
    }

    private void IncreaseStats(float _amountAltered)
    {
        switch (_statModIncrease)
        {
            case PlayerStatMod.Speed:
                //Alter player speed
                _player.AddSpeed(_amountAltered);
                break;
            case PlayerStatMod.Health:
                //Alter player health
                _player.AddHealth(_amountAltered);
                break;
            case PlayerStatMod.Attack:
                //Alter Player Damage
                _player.AddDamage(_amountAltered);
                break;
            case PlayerStatMod.Resistance:
                //Alter Player Reistance
                _player.AddResistance(_amountAltered);
                break;
            case PlayerStatMod.None:
                break;
        }
    }

    private void DecreaseStats(float _amountAltered)
    {
        switch (_statModDecrease)
        {
            case PlayerStatMod.Speed:
                //Alter player speed
                _player.AddSpeed(_amountAltered);
                break;
            case PlayerStatMod.Health:
                //Alter player health
                _player.AddHealth(_amountAltered);
                break;
            case PlayerStatMod.Attack:
                //Alter Player Damage
                _player.AddDamage(_amountAltered);
                break;
            case PlayerStatMod.Resistance:
                //Alter Player Reistance
                _player.AddResistance(_amountAltered);
                break;
            case PlayerStatMod.None:
                break;
        }
    }

    public void ApplyRiskStats()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        ApplyStats();
        switch (_riskLevel)
        {
            case RiskType.Normal:
                NormalRisk();
                break;
            case RiskType.AllIn:
                RiskAll();
                break;
        }
    }

    public int NormalBiscuitCost()
    {
        return _initialBiscuitCost;
    }

    public int RiskBiscuitCost()
    {
        switch (_riskLevel)
        {
            case RiskType.Normal:
                return _initialBiscuitCost - _normalRiskCostReduction;
            case RiskType.AllIn:
                return 1;
        }
        return NormalBiscuitCost();
    }


    private void NormalRisk()
    {
        _statRisked = (PlayerStatMod)Random.Range(0, System.Enum.GetValues(typeof(PlayerStatMod)).Length - 1);
        RiskedStatMod();
    }

    private void RiskAll() //Risk all stats for massive discount
    {
        for (int _loopAmount = 0; _loopAmount < 5; _loopAmount++)
        {
            _statRisked = (PlayerStatMod)Random.Range(0, System.Enum.GetValues(typeof(PlayerStatMod)).Length - 1);
            RiskedStatMod();
        }
    }

    private void RiskedStatMod()
    {
        float _modifyAmount = Random.Range(_riskLowerLimit, _riskUpperLimit);
        //Debug.Log(_modifyAmount);
        switch (_statRisked)
        {
            case PlayerStatMod.Speed:
                //Alter player speed
                _player.AddSpeed(_modifyAmount);
                break;
            case PlayerStatMod.Health:
                //Alter player health
                _player.AddHealth(_modifyAmount);
                break;
            case PlayerStatMod.Attack:
                //Alter Player Damage
                _player.AddDamage(_modifyAmount);
                break;
            case PlayerStatMod.Resistance:
                //Alter Player Reistance
                _player.AddResistance(_modifyAmount);
                break;
            case PlayerStatMod.None:
                break;
        }
    }
}
