using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Shop items")]
    [SerializeField] private CardCollection m_shopCollection;
    [SerializeField] private CardSelection m_cardSelectionPrefab;

    [Header("References")]
    [SerializeField] private GameObject m_shopPopup;
    [SerializeField] private GameObject m_shopContent;

    private void Start()
    {
        // The shop must be closed at the start of the game
        CloseShop();

        CreateShopItems();
    }
        
    /// <summary>
    /// Open the shop
    /// </summary>
    public void OpenShop()
    {
        // TODO: add fade in tween
        m_shopPopup.SetActive(true);
    }

    /// <summary>
    /// Close the shop
    /// </summary>
    public void CloseShop()
    {
        // TODO: add fade out tween
        m_shopPopup.SetActive(false);
    }
    
    /// <summary>
    /// Instantiate all items present in the shop collection
    /// </summary>
    private void CreateShopItems()
    {
        CardSelection shopItem = null;
        foreach (BaseCardData shopItemData in m_shopCollection.Cards)
        {
            shopItem = Instantiate(m_cardSelectionPrefab, m_shopContent.transform);
            shopItem.SetData(shopItemData);
            shopItem.GetComponentInChildren<Button>().onClick.AddListener(() => BuyItem((ItemCardData)shopItemData, shopItem.Value));
        }
    }

    /// <summary>
    /// Apply an item effect if the player has enough coin to buy it
    /// </summary>
    /// <param name="shopItemData">Data of the picked item</param>
    /// <param name="value">Value of the item</param>
    private void BuyItem(ItemCardData shopItemData, int value)
    {
        // Check if the player has enough coins to buy the item...
        if (GameManager.Instance.Player.Coins < shopItemData.Price)
        {
            Debug.Log($"[SHOP] Player doesn't have enough money to buy '{shopItemData.Name}'. " +
                      $"Price: {shopItemData.Price} <-> Player's coins: {GameManager.Instance.Player.Coins}");
            return;
        }
        
        switch (shopItemData.Type)
        {
            case EItemType.Health:
                GameManager.Instance.Player.RestoreHealth(value);
                break;
                
            case EItemType.Mana:
                GameManager.Instance.Player.UpdateMana(value);
                break;
                
            case EItemType.Armor:
                GameManager.Instance.Player.RestoreArmor(value);
                break;
        }
        
        GameManager.Instance.Player.UpdateCoins(shopItemData.Price * -1);
        
        // TODO: add tween when picking an item
        CloseShop();
    }
}
