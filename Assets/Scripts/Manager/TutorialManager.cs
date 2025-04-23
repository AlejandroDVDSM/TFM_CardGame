using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct TutorialDialogue
    {
        [SerializeField] 
        internal string id;
        
        [TextArea] [SerializeField]
        internal string DialogueLine;

        [Space(10)] [SerializeField]
        internal bool AllowArrowToContinue;
        
        [Space(10)]
        public UnityEvent OnStep;
    }
    
    [Header("Tutorial Config")]
    [SerializeField] private int m_tutorialTurns;
    [SerializeField] private TutorialDialogue[] m_dialogue;
    
    [Header("Highlight")]
    [SerializeField] private RectTransform m_maskImage;
    [SerializeField] private RectTransform m_referencePosition;
    
    [Header("Dialogue")]
    [SerializeField] private TMP_Text m_dialogueTxt;
    [SerializeField] private Button m_continueButton;
    
    [Space(10)]
    [SerializeField] private GameObject m_tutorialPanel;
    [SerializeField] private Button m_shopButton;
    
    public static TutorialManager Instance;
    public int TutorialTurns => m_tutorialTurns;
    public bool IsRunning { get; private set; }
    
    public Card CurrentHighlightedCard => m_currentHighlightedCard;

    private int m_currentDialogueLine;

    private int m_currentStep;

    private Card m_currentHighlightedCard;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTutorial()
    {
        GameManager.Instance.OnTurnCommited.AddListener(OnTutorialTurnCommited);
        
        AudioManager.Instance.Play("GameMusic");
        m_tutorialPanel.gameObject.SetActive(true);
        IsRunning = true;
        NextStep();
    }

    public void NextStep()
    {
        m_currentHighlightedCard = null;
        m_dialogue[m_currentDialogueLine].OnStep.Invoke();
        
        m_continueButton.gameObject.SetActive(m_dialogue[m_currentDialogueLine].AllowArrowToContinue);
        
        if (m_currentDialogueLine < m_dialogue.Length)
        {
            ShowNextDialog();
        }
    }
    
    public void DoStep1()
    {
        StartCoroutine(DoStep1Coroutine());
    }
    
    private IEnumerator DoStep1Coroutine()
    {
        InitTutorialCards();
        
        // We must wait 0.2 s. due to the time it takes to complete the card movement animation when populating rows
        yield return new WaitForSeconds(0.2f);

        List<Card> allCards = new List<Card>();
        allCards.AddRange(GameManager.Instance.BottomRow.GetCards());
        allCards.AddRange(GameManager.Instance.MiddleRow.GetCards());
        allCards.AddRange(GameManager.Instance.TopRow.GetCards());
        
        RectTransform[] cardsTarget = new RectTransform[allCards.Count];

        for (int i = 0; i < allCards.Count; i++)
        {
            cardsTarget[i] = allCards[i].GetComponent<RectTransform>();
        }
        
        UpdateHighlightTarget(cardsTarget, 25);

    }

    public void DoStep2()
    {
        Card targetCard = GameManager.Instance.BottomRow.GetCards()[1];
        UpdateHighlightTarget(targetCard.ValueImage.rectTransform, 10);
    }

    public void DoStep3()
    {
        Card targetCard = GameManager.Instance.BottomRow.GetCards()[1];
        UpdateHighlightTarget(targetCard.GetComponent<RectTransform>(), 25);
        
        m_currentHighlightedCard = targetCard;
    }

    public void DoStep4()
    {
        UpdateHighlightTarget(GameManager.Instance.Player.GetComponent<RectTransform>(), 50);
    }

    public void DoStep5()
    {
        Card targetCard = GameManager.Instance.BottomRow.GetCards()[2];
        UpdateHighlightTarget(targetCard.GetComponent<RectTransform>(), 25);

        m_currentHighlightedCard = targetCard;
    }

    public void DoStep6()
    {
        StartCoroutine(DoStep6Coroutine());
    }

    private IEnumerator DoStep6Coroutine()
    {
        yield return new WaitForSeconds(0.2f);

        RectTransform bottomCoinCard = GameManager.Instance.BottomRow.GetCards()[2].GetComponent<RectTransform>();
        RectTransform middleEnemyCard = GameManager.Instance.MiddleRow.GetCards()[2].GetComponent<RectTransform>();
        UpdateHighlightTarget(new []{ bottomCoinCard, middleEnemyCard }, 12);
    }

    public void DoStep7()
    {
        Card targetCard = GameManager.Instance.BottomRow.GetCards()[2];
        UpdateHighlightTarget(targetCard.GetComponent<RectTransform>(), 15);
        
        m_currentHighlightedCard = targetCard;
    }

    public void DoStep8()
    {
        UpdateHighlightTarget(m_shopButton.GetComponent<RectTransform>(), 10);
    }

    public void DoStep9()
    {
        UpdateHighlightTarget(FindAnyObjectByType<MagicView>().CastBtn.GetComponent<RectTransform>(), 10);
    }
    
    public void EndTutorial()
    {
        IsRunning = false;
        GameManager.Instance.OnTurnCommited.RemoveListener(OnTutorialTurnCommited);
        // m_highlight.gameObject.SetActive(false);
        // m_dialogueContainer.gameObject.SetActive(false);
        m_tutorialPanel.gameObject.SetActive(false);

        PlayerPrefs.SetInt("TutorialCompleted", 1);

        Debug.Log("[TUTORIAL] End Tutorial");
        
    }
    
    /// <summary>
    /// Update size and position of the highlight taking into account a single target
    /// </summary>
    /// <param name="target">The RectTransform to highlight</param>
    public void UpdateHighlightTarget(RectTransform target, int padding = 0)
    {
        Rect boundingRect = GetEnclosingRect(new []{ target }, m_referencePosition, padding);
        m_maskImage.DOAnchorPos(boundingRect.center, 0.25f);
        m_maskImage.DOSizeDelta(boundingRect.size, 0.25f);
    }

    /// <summary>
    /// Update size and position of the highlight taking into account multiple targets
    /// </summary>
    /// <param name="targets">All RectTransforms to highlight</param>
    public void UpdateHighlightTarget(RectTransform[] targets, int padding = 0)
    {
        Rect boundingRect = GetEnclosingRect(targets, m_referencePosition, padding);
        m_maskImage.DOAnchorPos(boundingRect.center, 0.25f);
        m_maskImage.DOSizeDelta(boundingRect.size, 0.25f);
    }
    
    private void OnTutorialTurnCommited()
    {
        NextStep();
    }

    private void ShowNextDialog()
    {
        Debug.Log($"[TUTORIAL] Current dialogue '{m_dialogue[m_currentDialogueLine].id}'");
        m_dialogueTxt.text = m_dialogue[m_currentDialogueLine].DialogueLine;
        m_currentDialogueLine++;
    }

    private void InitTutorialCards()
    {
        List<Card> bottomRowCards = new()
        {
            GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Health),
            GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Armor),
            GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Health)
        };
        
        List<Card> middleRowCards = new()
        {
            GameManager.Instance.CardPool.ExtractStatusCardOfType(EStatusType.Blind),
            GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Mana),
            GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Armor)
        };
        
        List<Card> topRowCards = new()
        {
            GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Armor),
            GameManager.Instance.CardPool.ExtractStatusCardOfType(EStatusType.Poison),
            GameManager.Instance.CardPool.ExtractEnemyCard()
        };

        GameManager.Instance.BottomRow.PopulateRow(bottomRowCards);
        GameManager.Instance.MiddleRow.PopulateRow(middleRowCards);
        GameManager.Instance.TopRow.PopulateRow(topRowCards);
    }
    
    /// <summary>
    /// Computes a Rect that encloses all the given RectTransforms, with an optional padding.
    /// The computed bounds are relative to the provided reference RectTransform.
    /// If referenceRect is null, the returned Rect is in world space.
    /// </summary>
    /// <param name="rectTransforms">Array of RectTransforms to enclose.</param>
    /// <param name="referenceRect">
    /// (Optional) The RectTransform that you want the result to be relative to.
    /// If null, the returned Rect is in world space.
    /// </param>
    /// <param name="padding">
    /// Optional padding to add to each side (in the coordinate space of the returned rect).
    /// A positive value expands the rect; a negative value shrinks it.
    /// </param>
    /// <returns>A Rect that bounds all the given RectTransforms with the specified padding.</returns>
    private static Rect GetEnclosingRect(RectTransform[] rectTransforms, RectTransform referenceRect = null, int padding = 0) {
        if (rectTransforms == null || rectTransforms.Length == 0)
            return new Rect();

        // If a reference rect is provided, update its layout to ensure accurate positions.
        if (referenceRect != null) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(referenceRect);
        }

        // Use this array to hold the 4 corners for each RectTransform.
        Vector3[] corners = new Vector3[4];
        bool useLocalSpace = (referenceRect != null);

        // Get corners for the first RectTransform.
        rectTransforms[0].GetWorldCorners(corners);
        // Initialize min and max in the proper coordinate space.
        Vector2 min, max;
        if (useLocalSpace) {
            Vector3 localCorner = referenceRect.InverseTransformPoint(corners[0]);
            min = new Vector2(localCorner.x, localCorner.y);
            max = min;
            for (int j = 0; j < 4; j++) {
                localCorner = referenceRect.InverseTransformPoint(corners[j]);
                Vector2 point = new Vector2(localCorner.x, localCorner.y);
                min = Vector2.Min(min, point);
                max = Vector2.Max(max, point);
            }
        }
        else {
            min = new Vector2(corners[0].x, corners[0].y);
            max = min;
            for (int j = 0; j < 4; j++) {
                Vector2 point = new Vector2(corners[j].x, corners[j].y);
                min = Vector2.Min(min, point);
                max = Vector2.Max(max, point);
            }
        }

        // Process the rest of the RectTransforms.
        for (int i = 1; i < rectTransforms.Length; i++) {
            rectTransforms[i].GetWorldCorners(corners);
            if (useLocalSpace) {
                for (int j = 0; j < 4; j++) {
                    Vector3 localCorner = referenceRect.InverseTransformPoint(corners[j]);
                    Vector2 point = new Vector2(localCorner.x, localCorner.y);
                    min = Vector2.Min(min, point);
                    max = Vector2.Max(max, point);
                }
            }
            else {
                for (int j = 0; j < 4; j++) {
                    Vector2 point = new Vector2(corners[j].x, corners[j].y);
                    min = Vector2.Min(min, point);
                    max = Vector2.Max(max, point);
                }
            }
        }

        // Apply padding (increases size by padding on every side).
        min -= new Vector2(padding, padding);
        max += new Vector2(padding, padding);

        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }
}
