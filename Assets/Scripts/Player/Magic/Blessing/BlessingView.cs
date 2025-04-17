using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class BlessingView : MagicView
{
    [Header("Blessing")]
    [SerializeField] private Image m_blessingsPanel;
    [SerializeField] private RectTransform m_blessingsContent;
    [SerializeField] private CardCollection m_blessingsCollection;
    [SerializeField] private CardSelection m_cardSelectionPrefab;
    
    private Blessing m_blessing;
    private CardSelection[] m_CardSelections;

    protected override void Start()
    {
        m_blessing = m_magicAttack as Blessing;
        m_CastBtn.onClick.AddListener(OpenBlessingPanel);
        CloseBlessingPanel(false);
        UpdateUI();
    }

    public void OpenBlessingPanel()
    {
        m_blessingsPanel.gameObject.SetActive(true);
        m_blessingsPanel.DOFade(0.95f, 0.1f)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                foreach (CardSelection cardSelection in m_CardSelections)
                {
                    cardSelection.transform.localScale = Vector3.zero;
                    cardSelection.transform.DOScale(Vector3.one, 0.2f)
                        .SetEase(Ease.OutSine)/*.SetDelay(0.1f)*/;
                }
            });
    }

    public void CloseBlessingPanel(bool playAnimation = true)
    {
        if (!playAnimation)
        {
            m_blessingsPanel.gameObject.SetActive(false);
            return;
        }
        
        m_blessingsPanel.DOFade(0f, 0.3f)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                foreach (CardSelection cardSelection in m_CardSelections)
                {
                    cardSelection.transform.localScale = Vector3.zero;
                }
                
                m_blessingsPanel.gameObject.SetActive(false);
            });
    }
    
    protected override void UpdateUI()
    {
        base.UpdateUI();

        m_CardSelections = new CardSelection[m_blessingsCollection.Cards.Count];
        for (int i = 0; i < m_blessingsCollection.Cards.Count; i++)
        {
            CardSelection cardSelection = Instantiate(m_cardSelectionPrefab, m_blessingsContent);
            cardSelection.SetData(m_blessingsCollection.Cards[i]);
            cardSelection.GetComponentInChildren<Button>().onClick.AddListener(() => PickBlessing(cardSelection));
            cardSelection.transform.localScale = Vector3.zero;
            
            m_CardSelections[i] = cardSelection;
        }
    }

    private void PickBlessing(CardSelection selectedBlessing)
    {
        if (!m_blessing.CanCast())
        {
            selectedBlessing.transform.DOShakePosition(0.5f, 7.5f);
            return;
        }
        
        m_blessing.ApplyBlessing((StatusCardData)selectedBlessing.CardData);
    }
}
