using CardGame.Enums;
using UnityEngine;
using UnityEngine.UI;

public class BlessingView : MagicView
{
    [SerializeField] private GameObject m_blessingsSelectionPanel;
    
    private Blessing m_blessing;

    protected override void Start()
    {
        m_blessing = m_magicAttack as Blessing;
        m_CastBtn.onClick.AddListener(() => m_blessingsSelectionPanel.SetActive(true));
        m_blessingsSelectionPanel.SetActive(false);
        UpdateUI();
    }

    public void PickInvisibilityBlessing()
    {
        // TODO: tween
        m_blessing.ApplyBlessing(EStatusType.Invisibility);
        m_blessingsSelectionPanel.SetActive(false);
    }

    public void PickProtectionBlessing()
    {
        // TODO: tween
        m_blessing.ApplyBlessing(EStatusType.Protection);
        m_blessingsSelectionPanel.SetActive(false);
    }

    public void PickRegenerationBlessing()
    {
        // TODO: tween
        m_blessing.ApplyBlessing(EStatusType.Regeneration);
        m_blessingsSelectionPanel.SetActive(false);
    }
}
