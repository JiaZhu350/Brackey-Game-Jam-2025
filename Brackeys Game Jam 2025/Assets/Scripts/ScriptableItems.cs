
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItems", menuName = "Scriptable Objects/ScriptableItems")]
public class ScriptableItems : ScriptableObject
{
    private enum PlayerStatMod { Speed, Health, Damage, Resistance, None }
    private enum RiskType { Normal, AllIn}


    [Header("Item Functionality")]
    [SerializeField] private PlayerStatMod _baseStat1;
    [SerializeField] private PlayerStatMod _baseStat2;
    [SerializeField] private RiskType _riskLevel;
    [SerializeField] private float _baseStat1IncreaseAmount;
    public float Stat1IncreaseAmount => _baseStat1IncreaseAmount;
    [SerializeField] private float _baseStat2IncreaseAmount;
    public float Stat2IncreaseAmount => _baseStat2IncreaseAmount;
    [SerializeField] private int _normalRiskCostReduction; //Does not affect all ins
    [SerializeField] private float _stat1UpperRange;
    [SerializeField] private float _stat1LowerRange;
    [SerializeField] private float _stat2UpperRange;
    [SerializeField] private float _stat2LowerRange;
    private Player _player;


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

        ChangeStats(_baseStat1, Stat1IncreaseAmount);
        ChangeStats(_baseStat2, Stat2IncreaseAmount);
    }

    private void ChangeStats(PlayerStatMod statMod, float _amountAltered)
    {
        Debug.Log(statMod);
        Debug.Log(_amountAltered);
        switch (statMod)
        {
            case PlayerStatMod.Speed:
                //Alter player speed
                _player.AddSpeed(_amountAltered);
                break;
            case PlayerStatMod.Health:
                //Alter player health
                _player.AddHealth(_amountAltered);
                break;
            case PlayerStatMod.Damage:
                //Alter Player Damage
                _player.AddDamage(_amountAltered);
                break;
            case PlayerStatMod.Resistance:
                //Alter Player Resistance
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
        RiskedStatMod(_baseStat1, _stat1LowerRange, _stat1UpperRange);
        RiskedStatMod(_baseStat2, _stat2LowerRange, _stat2UpperRange);
    }

    private void RiskAll() //Risk all stats for massive discount
    {
        for (int _loopAmount = 0; _loopAmount < 3; _loopAmount++)
        {
            RiskedStatMod(_baseStat1, _stat1LowerRange, _stat1UpperRange);
            RiskedStatMod(_baseStat2, _stat2LowerRange, _stat2UpperRange);
        }
    }

    private void RiskedStatMod(PlayerStatMod statRisked, float lowerRange, float upperRange)
    {
        float _modifyAmount = Mathf.Round(Random.Range(lowerRange, upperRange));
        switch (statRisked)
        {
            case PlayerStatMod.Speed:
                //Alter player speed
                Debug.Log(_modifyAmount);
                Debug.Log(lowerRange);
                Debug.Log(upperRange);
                _player.AddSpeed(_modifyAmount);
                break;
            case PlayerStatMod.Health:
                //Alter player health
                _player.AddHealth(_modifyAmount);
                break;
            case PlayerStatMod.Damage:
                //Alter Player Damage
                _player.AddDamage(_modifyAmount);
                break;
            case PlayerStatMod.Resistance:
                //Alter Player Resistance
                _player.AddResistance(_modifyAmount);
                break;
            case PlayerStatMod.None:
                break;
        }
    }
}
