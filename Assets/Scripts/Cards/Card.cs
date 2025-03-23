using System;
using CardGame.Enums;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Card: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text nameTxt;    
    [SerializeField] private TMP_Text valueTxt;
    [SerializeField] private Image background;
    [SerializeField] private Image shadow;
    [SerializeField] protected Image image;
    
    public bool IsInPool { get; set; }
    public ECardLane Lane => m_lane;
    public ERow CurrentRow => m_currentRow;
    public int Value => m_value;
    
    private ECardLane m_lane = ECardLane.Out;
    protected ERow m_currentRow;
    
    protected BaseCardData m_data;
    protected int m_value;
    
    protected CanvasGroup m_canvasGroup;
    
    private Transmute m_Transmute;

    protected virtual void Start()
    {
        GameManager.Instance.Player.TryGetComponent<Transmute>(out m_Transmute);
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    public abstract void PerformAction();

    public void SetData(BaseCardData cardData)
    {
        m_data = cardData;
        m_value = Random.Range(cardData.MinValue, cardData.MaxValue + 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameTxt.text = m_data.Name;
        valueTxt.text = m_value.ToString();
        image.sprite = m_data.Sprite;
        background.color = m_data.BackgroundColor;
        shadow.color = m_data.BackgroundColor;
    }

    public void HideValue(bool hide)
    {
        valueTxt.text = hide ? "?" : Value.ToString();
    }
    
    public void UpdateValue(int cardValue)
    {
        m_value = Mathf.Clamp(cardValue, 0, m_value);
        valueTxt.text = m_value.ToString();
    }

    public void SetLaneAndRow(int laneIndex, ERow row)
    {
        m_lane = (ECardLane)laneIndex;
        m_currentRow = row;
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_Transmute !=null && m_Transmute.IsTransmuting)
        {
            m_Transmute.PickCard(this);
            return;
        }
        
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
