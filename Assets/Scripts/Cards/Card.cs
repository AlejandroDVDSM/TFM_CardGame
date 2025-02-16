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
    public ECardLane CardLane => _cardLane;
    public ERow CurrentRow => _currentRow;
    public BaseCardData CardData => _cardData;
    
    private ECardLane _cardLane = ECardLane.OUT;
    private ERow _currentRow;
    private BaseCardData _cardData;
    protected int cardValue;

    protected abstract void PerformAction();

    public void SetCard(BaseCardData cardData)
    {
        // Debug.Log($"Card: {cardData}");
        _cardData = cardData;
        cardValue = Random.Range(cardData.MinValue, cardData.MaxValue + 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameTxt.text = _cardData.Name;
        valueTxt.text = cardValue.ToString();
        image.sprite = _cardData.Sprite;
    }

    public void SetLaneAndRow(int laneIndex, ERow row)
    {
        _cardLane = (ECardLane)laneIndex;
        _currentRow = row;
    }

    public void SetRow(ERow row)
    {
        _currentRow = row;
    }

    public void DisableCard()
    {
        _cardLane = ECardLane.OUT;
        _currentRow = ERow.OUT;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Do nothing if the selected card is not at the bottom
        // TODO: add tween when the selected card is not at the bottom
        if (_currentRow != ERow.BOTTOM)
            return;
        
        if (CardData.name.Equals("Cup"))
            GameManager.Instance.EndGame();
        else
        {
            PerformAction();
            GameManager.Instance.SelectCard(this);
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
