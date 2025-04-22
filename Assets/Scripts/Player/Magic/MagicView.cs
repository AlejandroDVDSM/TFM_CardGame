using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class MagicView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected Button m_CastBtn;
    [SerializeField] protected TMP_Text m_magicNameTxt;
    [SerializeField] protected TMP_Text m_manaCostTxt;
    
    public Button CastBtn => m_CastBtn;

    protected MagicAttack m_magicAttack;

    private void Awake()
    {
        m_magicAttack = FindAnyObjectByType<MagicAttack>();
    }

    protected virtual void Start()
    {
        m_CastBtn.onClick.AddListener(m_magicAttack.Cast);
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        m_magicNameTxt.text = m_magicAttack.MagicData.Name;
        m_manaCostTxt.text = m_magicAttack.MagicData.ManaCost.ToString();   
    }
}