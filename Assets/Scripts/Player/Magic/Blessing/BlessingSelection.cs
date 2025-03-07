using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlessingSelection : MonoBehaviour
{
    [SerializeField] private StatusCardData m_blessingData;

    [Header("UI")] 
    [SerializeField] private TMP_Text m_blessingName;
    [SerializeField] private TMP_Text m_blessingValue;
    [SerializeField] private Image m_blessingImage;
    
    void Start()
    {
        m_blessingName.text = m_blessingData.Name;
        m_blessingValue.text = Random.Range(m_blessingData.MinValue, m_blessingData.MaxValue + 1).ToString();
        m_blessingImage.sprite = m_blessingData.Sprite;
    }
}
