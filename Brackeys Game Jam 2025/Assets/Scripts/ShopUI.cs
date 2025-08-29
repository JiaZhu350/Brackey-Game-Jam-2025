using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    [Header("Item Box One")]
    [SerializeField] private Image _itemIconOne;
    [SerializeField] private TextMeshProUGUI _nameDisplayerOne;
    [SerializeField] private TextMeshProUGUI _biscuitCounterOne;
    private ScriptableItems _itemInBoxOne;

    [Header("Item Box Two")]
    [SerializeField] private Image _itemIconTwo;
    [SerializeField] private TextMeshProUGUI _nameDisplayerTwo;
    [SerializeField] private TextMeshProUGUI _biscuitCounterTwo;
    private ScriptableItems _itemInBoxTwo;


    [Header("Item Box Three")]
    [SerializeField] private Image _itemIconThree;
    [SerializeField] private TextMeshProUGUI _nameDisplayerThree;
    [SerializeField] private TextMeshProUGUI _biscuitCounterThree;
    private ScriptableItems _itemInBoxThree;


    [Header("Information Display Box")]
    [SerializeField] private Image _displayBox;
    [SerializeField] private TextMeshProUGUI _discriptionBox;


    [Header("Player Biscuit Counter")]
    [SerializeField] private TextMeshProUGUI _displayerPlayerBiscuits;
    private int _numOfPlayerBisuits;

    private ScriptableItems _currentlySelectedItem;
    private Player _player;

    private void Awake()
    {
        _container.SetActive(false);
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void BuildingShopUI(ScriptableItems[] _items)
    {
        if (_items.Length != 3)
        {
            Debug.LogWarning("ItemHolder must only have 3 elements; Currently accessed ItemHolder contains " + _items.Length + "elements");
        }
        _container.SetActive(true);
        ResettingDisplay();
        Time.timeScale = 0f;
        _itemInBoxOne = _items[0];
        _itemInBoxTwo = _items[1];
        _itemInBoxThree = _items[2];
        BuildingShopContainer();
        UpdatePlayerBiscuitCounter();
    }

    private void ResettingDisplay()
    {
        _displayBox.sprite = null;
        _discriptionBox.text = "";
    }

    private void BuildingShopContainer()
    {
        BuildItemBoxOne();
        BuildItemBoxTwo();
        BuildItemBoxThree();
    }

    private void BuildItemBoxOne()
    {
        _itemIconOne.sprite = _itemInBoxOne.ItemSprite;
        _nameDisplayerOne.text = _itemInBoxOne.ItemName;
        _biscuitCounterOne.text = _itemInBoxOne.CostOfItem.ToString();
    }

    private void BuildItemBoxTwo()
    {
        _itemIconTwo.sprite = _itemInBoxTwo.ItemSprite;
        _nameDisplayerTwo.text = _itemInBoxTwo.ItemName;
        _biscuitCounterTwo.text = _itemInBoxTwo.CostOfItem.ToString();
    }

    private void BuildItemBoxThree()
    {
        _itemIconThree.sprite = _itemInBoxThree.ItemSprite;
        _nameDisplayerThree.text = _itemInBoxThree.ItemName;
        _biscuitCounterThree.text = _itemInBoxThree.CostOfItem.ToString();
    }

    private void UpdatePlayerBiscuitCounter()
    {
        _numOfPlayerBisuits = _player.GetBiscuit();
        _displayerPlayerBiscuits.text = _numOfPlayerBisuits.ToString();
    }


    public void SelectingItemBoxOne()
    {
        _currentlySelectedItem = _itemInBoxOne;
        SettingItemDiscription();
    }

    public void SelectingItemBoxTwo()
    {
        _currentlySelectedItem = _itemInBoxTwo;
        SettingItemDiscription();
    }

    public void SelectingItemBoxThree()
    {
        _currentlySelectedItem = _itemInBoxThree;
        SettingItemDiscription();
    }

    private void SettingItemDiscription()
    {
        _displayBox.sprite = _currentlySelectedItem.ItemSprite;
        _discriptionBox.text = _currentlySelectedItem.ItemDescrition;
    }

    public void BuyItemWithBiscuit()
    {
        if (_currentlySelectedItem == null)
        {
            UnableToBuy();
            return;
        }

        int _itemPrice = _currentlySelectedItem.NormalBiscuitCost();
        if (_itemPrice > _numOfPlayerBisuits)
        {
            UnableToBuy();
        }
        else
        {
            _player.ModifyBiscuit(-1 * _itemPrice);
            _currentlySelectedItem.ApplyStats();
            UpdatePlayerBiscuitCounter();
        }
    }

    public void RiskItForTheBiscuit()
    {
        if (_currentlySelectedItem == null)
        {
            UnableToBuy();
            return;
        }
        
        int _itemPrice = _currentlySelectedItem.RiskBiscuitCost();
        if (_itemPrice > _numOfPlayerBisuits)
        {
            UnableToBuy();
        }
        else
        {
            _player.ModifyBiscuit(-1 * _itemPrice);
            _currentlySelectedItem.ApplyRiskStats();
            UpdatePlayerBiscuitCounter();
        }
    }

    private void UnableToBuy()
    {
        //Player a sound or something
    }

    public void ExitShop()
    {
        _container.SetActive(false);
        Time.timeScale = 1f;
    }
}
