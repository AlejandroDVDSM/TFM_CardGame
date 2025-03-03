using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireballView : MonoBehaviour, IMagicView
{

    [Header("UI")]
    [SerializeField] private Button m_CastBtn;
    [SerializeField] private TMP_Text m_magicNameTxt;
    [SerializeField] private TMP_Text m_manaCostTxt;
    
    private Fireball m_magicAttack;

    private void Start()
    {
        m_magicAttack = FindAnyObjectByType<Fireball>();
        m_CastBtn.onClick.AddListener(m_magicAttack.Cast);
        UpdateUI();
    }

    public void UpdateUI()
    {
        m_magicNameTxt.text = m_magicAttack.MagicData.Name;
        m_manaCostTxt.text = m_magicAttack.MagicData.ManaCost.ToString();
    }
}