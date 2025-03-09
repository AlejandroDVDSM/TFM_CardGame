using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class BlessingView : MagicView
{
    [Header("Blessing")]
    [SerializeField] private GameObject m_blessingsSelectionPanel;
    [SerializeField] private CardCollection m_blessingsCollection;
    [SerializeField] private CardSelection m_cardSelectionPrefab;
    
    private Blessing m_blessing;

    protected override void Start()
    {
        m_blessing = m_magicAttack as Blessing;
        m_CastBtn.onClick.AddListener(() => m_blessingsSelectionPanel.SetActive(true));
        m_blessingsSelectionPanel.SetActive(false);
        UpdateUI();
    }
    
    protected override void UpdateUI()
    {
        base.UpdateUI();

        CardSelection cardSelection = null;
        foreach (var blessing in m_blessingsCollection.Cards)
        {
            cardSelection = Instantiate(m_cardSelectionPrefab, m_blessingsSelectionPanel.transform);
            cardSelection.SetData(blessing);
            cardSelection.GetComponentInChildren<Button>().onClick.AddListener(() => PickBlessing(((StatusCardData)blessing).Status /*, cardSelection.Value*/));
        }
    }

    private void PickBlessing(EStatusType status)
    {
        // TODO: tween when picking blessing
        m_blessing.ApplyBlessing(status);
        m_blessingsSelectionPanel.SetActive(false);
    }
}
