using CardGame.Enums;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
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
    public bool IsPerformingAction => m_isPerformingAction;
    
    private ECardLane m_lane = ECardLane.Out;
    protected ERow m_currentRow;
    
    protected BaseCardData m_data;
    protected int m_value;
    protected bool m_isPerformingAction;
    
    protected CanvasGroup m_canvasGroup;
    
    private Transmute m_Transmute;
    private Vector3 m_imageStartingLocalPosition;

    protected virtual void Start()
    {
        GameManager.Instance.Player.TryGetComponent<Transmute>(out m_Transmute);
        m_canvasGroup = GetComponent<CanvasGroup>();

        m_imageStartingLocalPosition = image.transform.localPosition;
        
        EnableFloatingAnimation();
        GameManager.Instance.OnTurnCommited.AddListener(EnableFloatingAnimation);
    }

    #region Animations

    /// <summary>
    /// Animate image if the player can select this card
    /// </summary>
    private void EnableFloatingAnimation()
    {
        if (m_currentRow != ERow.Bottom)
        {
            return;
        }
        
        // If the player can't move to this card...
        if (!GameManager.Instance.Player.Movement.CanMoveTo(m_lane))
        {
            // ...reduce image alpha
            Color disabledColor = image.color;
            disabledColor.a = 0.5f;
            image.color = disabledColor;
            
            // ...slow down sine movement
            image.transform.DOLocalMoveX(image.transform.localPosition.x + 10f, 4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            image.transform.DOLocalMoveY(image.transform.localPosition.y + 8f, 3.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            // ...apply sine movement
            image.transform.DOLocalMoveX(image.transform.localPosition.x + 10f, 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            image.transform.DOLocalMoveY(image.transform.localPosition.y + 8f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }

    protected void EnableDisappearAnimation(TweenCallback callback)
    {
        Sequence disappearSequence = DOTween.Sequence();
        disappearSequence
            .Append(image.transform.DOScale(Vector3.one * 1.4f, 0.25f).SetEase(Ease.OutQuad))
            .Append(image.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InQuad))
            .OnComplete(callback);
        
        disappearSequence.Play();
    }

    #endregion

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
    
    public void DestroyCard()
    {
        transform.SetParent(GameManager.Instance.CardPool.transform);
        transform.localPosition = Vector3.zero;
        
        SetLaneAndRow(-1, ERow.Out);
        
        m_canvasGroup.alpha = 1;

        // Kill movement tween and go back to original position
        image.transform.DOKill();
        image.transform.localPosition = m_imageStartingLocalPosition;
        image.transform.localScale = Vector3.one;
        
        // Restore image alpha back to 1
        Color imageColor = image.color;
        imageColor.a = 1;
        image.color = imageColor;
        
        IsInPool = true;
    }

    #region EventSystemHandler

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_Transmute !=null && m_Transmute.IsTransmuting)
        {
            m_Transmute.PickCard(this);
            return;
        }

        // Abort if one of the bottom cards is already performing an action 
        foreach (Card card in GameManager.Instance.BottomRow.GetCards())
        {
            if (card.IsPerformingAction)
            {
                return;
            }
        }
        
        // Do nothing if the selected card is not at the bottom
        if (m_currentRow != ERow.Bottom || !GameManager.Instance.Player.Movement.CanMoveTo(m_lane))
        {
            AudioManager.Instance.Play("Denied");
            transform.DOShakePosition(0.4f, 10f);
            return;
        }
        
        GameManager.Instance.PlayTurn(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_currentRow != ERow.Bottom)
        {
            return;
        }

        transform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutSine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_currentRow != ERow.Bottom)
        {
            return;
        }
        
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InSine);
    }
    #endregion
}
