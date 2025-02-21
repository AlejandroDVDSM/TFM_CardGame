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
    public ECardLane Lane => m_lane;
    public int Value => m_value;
    
    private ECardLane m_lane = ECardLane.Out;
    private ERow m_currentRow;
    protected BaseCardData m_data;
    protected int m_value;

    public abstract void PerformAction();

    public void Set(BaseCardData cardData)
    {
        // Debug.Log($"Card: {cardData}");
        m_data = cardData;
        m_value = Random.Range(cardData.MinValue, cardData.MaxValue + 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameTxt.text = m_data.Name;
        valueTxt.text = m_value.ToString();
        image.sprite = m_data.Sprite;
    }
    
    public void UpdateValue(int cardValue)
    {
        m_value = cardValue;
        valueTxt.text = cardValue.ToString();
    }

    public void SetLaneAndRow(int laneIndex, ERow row)
    {
        m_lane = (ECardLane)laneIndex;
        m_currentRow = row;
    }

    public void SetRow(ERow row)
    {
        m_currentRow = row;
    }

    public void Disable()
    {
        m_lane = ECardLane.Out;
        m_currentRow = ERow.Out;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Do nothing if the selected card is not at the bottom
        // TODO: add tween when the selected card is not at the bottom
        if (m_currentRow != ERow.Bottom)
            return;
        
        GameManager.Instance.PlayTurn(this);
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
