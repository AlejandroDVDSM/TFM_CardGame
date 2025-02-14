using CardGame.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsInPool { get; set; }
    public ECardLane CardLane => _cardLane;
    public ERow CurrentRow => _currentRow;
    
    private ECardLane _cardLane = ECardLane.OUT;
    private ERow _currentRow;

    protected abstract void ApplyEffect();

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
        IsInPool = true;
        _cardLane = ECardLane.OUT;
        _currentRow = ERow.OUT;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Do nothing if the selected card is not at the bottom
        // TODO: add tween when the selected card is not at the bottom
        if (_currentRow != ERow.BOTTOM)
            return;
        
        GameManager.Instance.SelectCard(this);
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
