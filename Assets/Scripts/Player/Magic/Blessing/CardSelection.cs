using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelection : MonoBehaviour
{
    private BaseCardData m_cardData;

    public int Value => m_Value;
    
    [Header("UI")] 
    [SerializeField] private TMP_Text m_cardName;
    [SerializeField] private TMP_Text m_cardValue;
    [SerializeField] private Image m_cardImage;
    
    private int m_Value;

    public void SetData(BaseCardData cardData)
    {
        m_cardData = cardData;
        
        m_cardName.text = m_cardData.Name;
        
        m_Value = Random.Range(m_cardData.MinValue, m_cardData.MaxValue + 1);
        m_cardValue.text = m_Value.ToString();
        
        m_cardImage.sprite = m_cardData.Sprite;
    }
}
