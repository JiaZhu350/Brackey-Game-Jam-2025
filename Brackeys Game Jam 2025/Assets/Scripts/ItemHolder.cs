using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private ScriptableItems[] ItemsInShop;

    //Just for testing purposes
    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            SendItemsToUI();
        }
    }

    private void SendItemsToUI()
    {
        GameObject.FindGameObjectWithTag("Shop UI").GetComponent<ShopUI>().BuildingShopUI(ItemsInShop);
    }
}
