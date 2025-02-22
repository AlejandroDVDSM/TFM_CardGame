using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Player m_player;
    
    Dictionary<EStatusType, int> m_appliedStatuses = new Dictionary<EStatusType, int>();

    private void Start()
    {
        m_player = GetComponent<Player>();
        GameManager.Instance.OnTurnCommited.AddListener(ApplyStatusesEffects);
    }

    /// <summary>
    /// Apply a new status to the player. If the status is already applied, it will overwrite the number of turns
    /// </summary>
    /// <param name="status">The new status to apply</param>
    /// <param name="turns">The number of turns the player will have this status</param>
    public void ApplyNewStatus(EStatusType status, int turns)
    {
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
    private void ApplyStatusesEffects()
    {
        if (m_appliedStatuses.ContainsKey(EStatusType.Poison))
            ApplyPoison();
        
        if (m_appliedStatuses.ContainsKey(EStatusType.Blind))
            ApplyBlind();
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
            m_appliedStatuses.Remove(EStatusType.Poison);
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
        }
    }

    public bool HasStatusApplied(EStatusType status)
    {
        return m_appliedStatuses.ContainsKey(status);
    }
}
