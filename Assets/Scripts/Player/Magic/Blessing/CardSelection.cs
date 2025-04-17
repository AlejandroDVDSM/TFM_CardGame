using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelection : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private TMP_Text m_cardName;
    [SerializeField] private TMP_Text m_cardValue;
    [SerializeField] private Image m_cardImage;
    
    [Header("Shop Card Selection")]
    [SerializeField] private TMP_Text m_itemPrice;
    
    public int Value => m_Value;
    public BaseCardData CardData => m_cardData;
    
    private int m_Value;
    private BaseCardData m_cardData;
    

    public void SetData(BaseCardData cardData)
    {
        m_cardData = cardData;
        
        m_cardName.text = m_cardData.Name;
        
        m_Value = Random.Range(m_cardData.MinValue, m_cardData.MaxValue + 1);
        m_cardValue.text = m_Value.ToString();
        
        m_cardImage.sprite = m_cardData.Sprite;

        // Set the price text if the card selection is used in the shop
        if (m_itemPrice)
        {
            m_itemPrice.text = ((ItemCardData)cardData).Price.ToString();
        }
    }
}
