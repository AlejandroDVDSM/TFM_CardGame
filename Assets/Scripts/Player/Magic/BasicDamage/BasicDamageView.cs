using CardGame.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicDamageView : MonoBehaviour, IMagicView
{
    [Header("Button")]
    [SerializeField] private Button m_CastBtn;
    
    [Header("Toggles")] 
    [SerializeField] private Toggle m_nextToggle;
    [SerializeField] private Toggle m_laneToggle;
    [SerializeField] private Toggle m_allToggle;
    
    [Header("Texts")]
    [SerializeField] private TMP_Text m_nextToggleTxt;
    [SerializeField] private TMP_Text m_laneToggleTxt;
    [SerializeField] private TMP_Text m_allToggleTxt;

    
    private BasicDamage m_basicDamage;

    private void Start()
    {
        m_basicDamage = FindAnyObjectByType<BasicDamage>();
        
        m_CastBtn.onClick.AddListener(m_basicDamage.Cast);
        m_nextToggle.onValueChanged.AddListener(OnNextModeSelected);
        m_laneToggle.onValueChanged.AddListener(OnLaneModeSelected);
        m_allToggle.onValueChanged.AddListener(OnAllModeSelected);
        
        UpdateUI();
    }

    private void OnNextModeSelected(bool isOn)
    {
        if (!isOn)
        {
            return;
        }
        
        m_basicDamage.SetMode(EBasicDamageMode.Next);
    }

    private void OnLaneModeSelected(bool isOn)
    {
        if (!isOn)
        {
            return;
        }
        
        m_basicDamage.SetMode(EBasicDamageMode.Lane);
    }

    private void OnAllModeSelected(bool isOn)
    {
        if (!isOn)
        {
            return;
        }
        
        m_basicDamage.SetMode(EBasicDamageMode.All);
    }
    
    public void UpdateUI()
    {
        m_nextToggleTxt.text = $"Next [{m_basicDamage.GetManaCost(EBasicDamageMode.Next)}]";
        m_laneToggleTxt.text = $"Lane [{m_basicDamage.GetManaCost(EBasicDamageMode.Lane)}]";
        m_allToggleTxt.text = $"All [{m_basicDamage.GetManaCost(EBasicDamageMode.All)}]";
    }
}
