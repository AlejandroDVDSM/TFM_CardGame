using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Transform m_statusIconsContainer;
    [SerializeField] private StatusIcon m_statusIconPrefab;
    
    private Player m_player;
    
    private Dictionary<EStatusType, int> m_appliedStatuses = new();
    private List<StatusIcon> m_statusIconsPlaced = new();

    private void Start()
    {
        m_player = GetComponent<Player>();
        GameManager.Instance.OnTurnCommited.AddListener(ApplyCurrentStatuses);
    }

    /// <summary>
    /// Apply a new status to the player. If the status is already applied, it will overwrite the number of turns
    /// </summary>
    /// <param name="statusData">The new status to apply</param>
    /// <param name="turns">The number of turns the player will have this status</param>
    public void ApplyNewStatus(StatusCardData statusData, int turns)
    {
        Debug.Log($"[PLAYER] Applying new status: <{statusData.Status}>");
        
        // Check if the status is already applied...
        if (m_appliedStatuses.ContainsKey(statusData.Status))
        { 
            // ...overwrite it
            m_appliedStatuses[statusData.Status] = turns;
            UpdateStatusInfo(statusData.Status, turns);
        }
        else
        {
            // ... add it
            m_appliedStatuses.Add(statusData.Status, turns);
            AddStatusInfo(statusData, turns);
        }
    }
    
    /// <summary>
    /// Check if the given status is active for the player
    /// </summary>
    /// <param name="status">The status to check</param>
    /// <returns>True if the player has the given status applied. False if not</returns>
    public bool HasStatusApplied(EStatusType status)
    {
        return m_appliedStatuses.ContainsKey(status);
    }
    
    /// <summary>
    /// Apply all current player's statuses
    /// </summary>
    private void ApplyCurrentStatuses()
    {
        if (m_appliedStatuses.ContainsKey(EStatusType.Poison))
            ApplyPoison();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.Blind))
            ApplyBlind();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.Silence))
            ApplySilence();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.Invisibility))
            ApplyInvisibility();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.Protection))
            ApplyProtection();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.Regeneration))
            ApplyRegeneration();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.ArcaneProtection))
            ApplyArcaneProtection();

        UpdateStatusesInfo();
    }

    /// <summary>
    /// Reduce player's health by 1
    /// </summary>
    private void ApplyPoison()
    {
        if (m_appliedStatuses[EStatusType.Poison] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Poison}> status ({m_appliedStatuses[EStatusType.Poison]} turns left)");
            
            m_player.Hit(1);
            m_appliedStatuses[EStatusType.Poison]--;
            AudioManager.Instance.Play("Poison");
        }
        // else
        // {
        //     m_appliedStatuses.Remove(EStatusType.Poison);
        //     Debug.Log($"[PLAYER] Status <{EStatusType.Poison}> removed>");
        // }
    }

    /// <summary>
    /// Hide all cards' values
    /// </summary>
    private void ApplyBlind()
    {
        List<Card> cards = FindObjectsByType<Card>(FindObjectsSortMode.None).Where(c => !c.IsInPool).ToList();
        
        if (m_appliedStatuses[EStatusType.Blind] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Blind}> status ({m_appliedStatuses[EStatusType.Blind]} turns left)");
            
            // Hide the values of all cards outside the pool 
            foreach (var card in cards)
            {
                card.HideValue(true);
            }
            
            m_appliedStatuses[EStatusType.Blind]--;
        }
        else
        {
            // Hide the values of all cards outside the pool 
            foreach (var card in cards)
            {
                card.HideValue(false);
            }

            // m_appliedStatuses.Remove(EStatusType.Blind);
            // Debug.Log($"[PLAYER] Status <{EStatusType.Blind}> removed>");
        }
    }
    
    /// <summary>
    /// Prevent player from casting any magic
    /// </summary>
    private void ApplySilence() 
    {
        if (m_appliedStatuses[EStatusType.Silence] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Silence}> status ({m_appliedStatuses[EStatusType.Silence]} turns left)");
            m_appliedStatuses[EStatusType.Silence]--;
        }
    }

    /// <summary>
    /// Make enemies ignore the player. Enemies won't drop coins while this status is enabled
    /// </summary>
    private void ApplyInvisibility()
    {
        if (m_appliedStatuses[EStatusType.Invisibility] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Invisibility}> status ({m_appliedStatuses[EStatusType.Invisibility]} turns left)");
            m_appliedStatuses[EStatusType.Invisibility]--;
        }
    }

    /// <summary>
    /// Reduce damage by half
    /// </summary>
    private void ApplyProtection()
    {
        if (m_appliedStatuses[EStatusType.Protection] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Protection}> status ({m_appliedStatuses[EStatusType.Protection]} turns left)");
            m_appliedStatuses[EStatusType.Protection]--;
        }
    }

    /// <summary>
    /// Heal player each turn while this status is enabled
    /// </summary>
    private void ApplyRegeneration()
    {
        if (m_appliedStatuses[EStatusType.Regeneration] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Regeneration}> status ({m_appliedStatuses[EStatusType.Regeneration]} turns left)");
            
            m_player.RestoreHealth(1);
            m_appliedStatuses[EStatusType.Regeneration]--;
            
            AudioManager.Instance.Play("Regeneration");
        }
    }

    /// <summary>
    /// Restore health each turn and block all damage
    /// </summary>
    private void ApplyArcaneProtection()
    {
        if (m_appliedStatuses[EStatusType.ArcaneProtection] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.ArcaneProtection}> status ({m_appliedStatuses[EStatusType.ArcaneProtection]} turns left)");
            
            m_player.RestoreHealth(1);
            m_appliedStatuses[EStatusType.ArcaneProtection]--;
        }
    }

    private void AddStatusInfo(StatusCardData statusData, int turns)
    {
        StatusIcon newStatusIcon = Instantiate(m_statusIconPrefab, m_statusIconsContainer);
        newStatusIcon.SetStatusIcon(statusData, turns);
        
        m_statusIconsPlaced.Add(newStatusIcon);
    }
    
    private void UpdateStatusInfo(EStatusType statusType, int turns)
    {
        StatusIcon statusIcon = m_statusIconsPlaced.FirstOrDefault(s => s.StatusType == statusType);

        if (!statusIcon)
        {
            return;
        }
        
        statusIcon.UpdateTurns(turns);
    }

    private void UpdateStatusesInfo()
    {
        foreach (var appliedStatus in m_appliedStatuses)
        {
            if (appliedStatus.Value > 0)
            {
                UpdateStatusInfo(appliedStatus.Key, appliedStatus.Value);
            } else 
            {
                RemoveStatusInfo(appliedStatus.Key);
                
                Debug.Log($"[PLAYER] Status <{appliedStatus.Key}> removed>");
                m_appliedStatuses.Remove(appliedStatus.Key);
            }
        }
    }

    private void RemoveStatusInfo(EStatusType statusType)
    {
        StatusIcon statusIcon = m_statusIconsPlaced.FirstOrDefault(s => s.StatusType == statusType);
        m_statusIconsPlaced.Remove(statusIcon);

        if (!statusIcon)
        {
            return;
        }
        
        Destroy(statusIcon.gameObject);
    }
}
