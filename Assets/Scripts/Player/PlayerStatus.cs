using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Player m_player;
    
    Dictionary<EStatusType, int> m_appliedStatuses = new();

    private void Start()
    {
        m_player = GetComponent<Player>();
        GameManager.Instance.OnTurnCommited.AddListener(ApplyCurrentStatuses);
    }

    /// <summary>
    /// Apply a new status to the player. If the status is already applied, it will overwrite the number of turns
    /// </summary>
    /// <param name="status">The new status to apply</param>
    /// <param name="turns">The number of turns the player will have this status</param>
    public void ApplyNewStatus(EStatusType status, int turns)
    {
        Debug.Log($"[PLAYER] Applying new status: <{status}>");
        
        if (m_appliedStatuses.ContainsKey(status))
        {
            m_appliedStatuses[status] = turns;
        }
        else
        {
            m_appliedStatuses.Add(status, turns);
        }
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
    }

    private void ApplyPoison()
    {
        if (m_appliedStatuses[EStatusType.Poison] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Poison}> status ({m_appliedStatuses[EStatusType.Poison]} turns left)");
            
            m_player.Hit(1);
            m_appliedStatuses[EStatusType.Poison]--;
        }
        else
        {
            m_appliedStatuses.Remove(EStatusType.Poison);
            Debug.Log($"[PLAYER] Status <{EStatusType.Poison}> removed>");
        }
    }

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

            m_appliedStatuses.Remove(EStatusType.Blind);
            Debug.Log($"[PLAYER] Status <{EStatusType.Blind}> removed>");
        }
    }
    
    private void ApplySilence() 
    {
        if (m_appliedStatuses[EStatusType.Silence] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Silence}> status ({m_appliedStatuses[EStatusType.Silence]} turns left)");
            m_appliedStatuses[EStatusType.Silence]--;
        }
        else
        {
            m_appliedStatuses.Remove(EStatusType.Silence);
            Debug.Log($"[PLAYER] Status <{EStatusType.Silence}> removed>");
        }
    }

    private void ApplyInvisibility()
    {
        if (m_appliedStatuses[EStatusType.Invisibility] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Invisibility}> status ({m_appliedStatuses[EStatusType.Invisibility]} turns left)");
            m_appliedStatuses[EStatusType.Invisibility]--;
        }
        else
        {
            m_appliedStatuses.Remove(EStatusType.Invisibility);
            Debug.Log($"[PLAYER] Status <{EStatusType.Invisibility}> removed>");
        }
    }

    private void ApplyProtection()
    {
        if (m_appliedStatuses[EStatusType.Protection] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Protection}> status ({m_appliedStatuses[EStatusType.Protection]} turns left)");
            m_appliedStatuses[EStatusType.Protection]--;
        }
        else
        {
            m_appliedStatuses.Remove(EStatusType.Protection);
            Debug.Log($"[PLAYER] Status <{EStatusType.Protection}> removed>");
        }
    }

    private void ApplyRegeneration()
    {
        if (m_appliedStatuses[EStatusType.Regeneration] > 0)
        {
            Debug.Log($"[PLAYER] Applying <{EStatusType.Regeneration}> status ({m_appliedStatuses[EStatusType.Regeneration]} turns left)");
            
            m_player.RestoreHealth(1);
            m_appliedStatuses[EStatusType.Regeneration]--;
        }
        else
        {
            m_appliedStatuses.Remove(EStatusType.Regeneration);
            Debug.Log($"[PLAYER] Status <{EStatusType.Regeneration}> removed>");
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
}
