using CardGame.Enums;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Shop items")]
    [SerializeField] private CardCollection m_shopCollection;
    [SerializeField] private CardSelection m_cardSelectionPrefab;

    [Header("References")]
    [SerializeField] private Image m_shopPopup;
    [SerializeField] private GameObject m_shopContent;

    private void Start()
    {
        // The shop must be closed at the start of the game
        CloseShop(false);

        CreateShopItems();
    }
        
    /// <summary>
    /// Open the shop
    /// </summary>
    public void OpenShop()
    {
        m_shopPopup.gameObject.SetActive(true);
        m_shopPopup.DOFade(0.95f, 0.1f)
            .SetEase(Ease.OutSine);
    }

    /// <summary>
    /// Close the shop
    /// </summary>
    public void CloseShop(bool playAnimation = true)
    {
        if (!playAnimation)
        {
            m_shopPopup.gameObject.SetActive(false);
            return;
        }
        
        m_shopPopup.DOFade(0f, 0.2f)
            .SetEase(Ease.InSine)
            .OnComplete(() => m_shopPopup.gameObject.SetActive(false));
    }
    
    /// <summary>
    /// Instantiate all items present in the shop collection
    /// </summary>
    private void CreateShopItems()
    {
        foreach (BaseCardData shopItemData in m_shopCollection.Cards)
        {
            CardSelection shopItem = Instantiate(m_cardSelectionPrefab, m_shopContent.transform);
            shopItem.SetData(shopItemData);
            shopItem.GetComponentInChildren<Button>().onClick.AddListener(() => BuyItem(shopItem, shopItem.Value));
        }
    }

    /// <summary>
    /// Apply an item effect if the player has enough coins to buy it
    /// </summary>
    /// <param name="shopItem">Picked item to buy</param>
    /// <param name="value">Value of the item</param>
    private void BuyItem(CardSelection shopItem, int value)
    {
        ItemCardData shopItemData = shopItem.CardData as ItemCardData;

        if (!shopItemData)
        {
            Debug.LogError("[SHOP] The selected card is not an ItemCard.");
            return;
        }
        
        // Check if the player has enough coins to buy the item...
        if (GameManager.Instance.Player.Coins < shopItemData.Price)
        {
            Debug.Log($"[SHOP] Player doesn't have enough money to buy '{shopItemData.Name}'. " +
                      $"Price: {shopItemData.Price} <-> Player's coins: {GameManager.Instance.Player.Coins}");
            shopItem.transform.DOShakePosition(0.5f, 7.5f);
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
        
        CloseShop();
    }
}
