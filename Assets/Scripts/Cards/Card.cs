using CardGame.Enums;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text nameTxt;    
    [SerializeField] private TMP_Text valueTxt;
    [SerializeField] private Image image;
    
    public bool IsInPool { get; set; }
    public ECardLane CardLane => m_cardLane;
    
    private ECardLane m_cardLane = ECardLane.Out;
    private ERow m_currentRow;
    private BaseCardData m_cardData;
    protected int m_cardValue;

    public abstract void PerformAction();

    public void Set(BaseCardData cardData)
    {
        // Debug.Log($"Card: {cardData}");
        m_cardData = cardData;
        m_cardValue = Random.Range(cardData.MinValue, cardData.MaxValue + 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameTxt.text = m_cardData.Name;
        valueTxt.text = m_cardValue.ToString();
        image.sprite = m_cardData.Sprite;
    }

    public void SetLaneAndRow(int laneIndex, ERow row)
    {
        m_cardLane = (ECardLane)laneIndex;
        m_currentRow = row;
    }

    public void SetRow(ERow row)
    {
        m_currentRow = row;
    }

    public void Disable()
    {
        m_cardLane = ECardLane.Out;
        m_currentRow = ERow.Out;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Do nothing if the selected card is not at the bottom
        // TODO: add tween when the selected card is not at the bottom
        if (m_currentRow != ERow.Bottom)
            return;
        
        if (m_cardData.name.Equals("Cup"))
            GameManager.Instance.EndGame();
        else
        {
            GameManager.Instance.PlayTurn(this);
            //PerformAction();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: add juicy effect on hover
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: add juicy effect when it stops being hovered
    }
}
