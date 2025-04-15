using CardGame.Enums;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TurnsLeftTxt;
    
    private Image m_statusIcon;
    private EStatusType m_statusType;
    
    public EStatusType StatusType => m_statusType;

    private void Awake()
    {
        m_statusIcon = GetComponent<Image>();
    }

    public void SetStatusIcon(StatusCardData statusData, int turns)
    {
        m_statusIcon.sprite = statusData.Icon;
        m_TurnsLeftTxt.text = turns.ToString();
        m_statusType = statusData.Status;
    }

    public void UpdateTurns(int turns)
    {
        m_TurnsLeftTxt.text = turns.ToString();
    }
}
