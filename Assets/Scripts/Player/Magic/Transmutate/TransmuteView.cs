using CardGame.Enums;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransmuteView : MagicView
{
    [Header("Transmute")]
    [SerializeField] private GameObject m_transmuteSelectionPanel;
    [SerializeField] private TMP_Text m_transmuteModeText;
    [SerializeField] private CardCollection m_itemsCollection;
    
    [SerializeField] private CardSelection m_cardSelectionPrefab;
    
    private Transmute m_transmute;
    
    protected override void Start()
    {
        m_transmute = m_magicAttack as Transmute;

        if (!m_transmute)
        {
            Debug.LogError("[TransmuteView] MagicAttack is not Transmute");
            return;
        }
        
        m_CastBtn.onClick.AddListener(m_transmute.EnableTransmutingMode);
        m_transmuteSelectionPanel.SetActive(false);
        m_transmuteModeText.gameObject.SetActive(false);
        UpdateUI();
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();

        CardSelection cardSelection = null;
        foreach (var item in m_itemsCollection.Cards)
        {
            cardSelection = Instantiate(m_cardSelectionPrefab, m_transmuteSelectionPanel.transform);
            cardSelection.SetData(item);
            cardSelection.GetComponentInChildren<Button>().onClick.AddListener(() => PickItem(((ItemCardData)item).Type, cardSelection.Value));
        }
    }

    public void ShowItemCards()
    {
        m_transmuteSelectionPanel.SetActive(true);
        m_transmuteModeText.gameObject.SetActive(false);
    }
    
    private void PickItem(EItemType itemType, int value)
    {
        m_transmute.ApplyItem(itemType, value);
        m_transmuteSelectionPanel.SetActive(false);
    }

    public void EnableTransmuteModeUI()
    {
        m_transmuteModeText.gameObject.SetActive(true);
    }
}
