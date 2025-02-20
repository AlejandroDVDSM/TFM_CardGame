using System;
using CardGame.Enums;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    private struct Stats
    {
        [SerializeField]
        [Min(1)]
        internal int MaxHealth;
        [SerializeField]
        [Min(1)]
        internal int MaxArmor;
        [SerializeField]
        [Min(1)]
        internal int MaxMana;

        internal int m_currentHealth;
        internal int m_currentArmor;
        internal int m_currentMana;
        internal int m_coins;
    }
    
    [SerializeField] private Stats m_stats;
    
    [Space(10)]
    [Header("UI")]
    [SerializeField] private TMP_Text m_healthText;
    [SerializeField] private TMP_Text m_armorText;
    [SerializeField] private TMP_Text m_manaText;
    [SerializeField] private TMP_Text m_coinsText;
    
    [Space(10)]
    [Header("Position")]
    [SerializeField] private float m_lateralMovement;
    [SerializeField] private float m_posY;
    
    private ECardLane m_currentLane = ECardLane.Middle;
    
    private void Start()
    {
        m_stats.m_currentHealth = m_stats.MaxHealth;
        m_healthText.text = m_stats.m_currentHealth.ToString();
        m_armorText.text = m_stats.m_currentArmor.ToString();
        m_manaText.text = m_stats.m_currentMana.ToString();
        m_coinsText.text = m_stats.m_coins.ToString();
    }

    /// <summary>
    /// Reduce player's health
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    public void Hit(int damage)
    {
        m_stats.m_currentHealth = Mathf.Clamp(m_stats.m_currentHealth - damage, 0, m_stats.m_currentHealth);
        
        if (m_stats.m_currentHealth == 0)
        {
            // TODO: end game
            // TODO: add tween
            // TODO: sound
            Debug.Log("DEAD");
        }
        
        m_healthText.text = m_stats.m_currentHealth.ToString();
    }

    public void RestoreHealth(int health)
    {
        m_stats.m_currentHealth = Mathf.Clamp(m_stats.m_currentHealth + health, m_stats.m_currentHealth, m_stats.MaxHealth);
        m_healthText.text = m_stats.m_currentHealth.ToString();
    }

    public void RestoreArmor(int armor)
    {
        m_stats.m_currentArmor = Mathf.Clamp(m_stats.m_currentArmor + armor, m_stats.m_currentArmor, m_stats.MaxArmor);
        m_armorText.text = m_stats.m_currentArmor.ToString();
    }

    public void RestoreMana(int mana)
    {
        m_stats.m_currentMana = Mathf.Clamp(m_stats.m_currentMana + mana, m_stats.m_currentMana, m_stats.MaxMana);
        m_manaText.text = m_stats.m_currentMana.ToString();
    }

    public void AddCoins(int coins)
    {
        m_stats.m_coins += coins;
        m_coinsText.text = m_stats.m_coins.ToString();
    }
    
    /// <summary>
    /// Place the player in the desired lane
    /// </summary>
    /// <param name="cardLane">The desired lane</param>
    public void PlaceInPosition(ECardLane cardLane)
    {
        // Checks if the player is currently in one side and is trying to select a card from the other side
        // If it is, he can't move there
        // TODO: add tween to give player the feedback that it can't move to the other side
        if (!CanMoveTo(cardLane))
            return;
        
        Vector3 newPos = Vector3.zero;
        
        switch (cardLane)
        {
            case ECardLane.Left:
                newPos = new Vector3(-m_lateralMovement, m_posY, 0);
                break;
            
            case ECardLane.Middle:
                newPos = new Vector3(0, m_posY, 0);
                break;
            
            case ECardLane.Right:
                newPos = new Vector3(m_lateralMovement, m_posY, 0);
                break;
        }

        // Update position and current lane
        transform.localPosition = newPos;
        m_currentLane = cardLane;
    }

    /// <summary>
    /// Check if the player can move to the desired lane
    /// </summary>
    /// <param name="cardLane">The desired lane to check</param>
    /// <returns>Return true if the player can move to the desired lane. Return false if not</returns>
    public bool CanMoveTo(ECardLane cardLane)
    {
        if (Mathf.Abs(m_currentLane - cardLane) > 1)
        {
            Debug.LogWarning($"[Player] Player can not move from {m_currentLane} to {cardLane}");
            return false;
        }

        return true;
    }
}
