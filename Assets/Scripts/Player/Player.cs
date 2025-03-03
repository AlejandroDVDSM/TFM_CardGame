using System;
using CardGame.Enums;
using CardGame.Player;
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
    
    public int CurrentMana => m_stats.m_currentMana;
    
    [SerializeField] private Stats m_stats;
    
    [Space(10)]
    [Header("UI")]
    [SerializeField] private TMP_Text m_healthText;
    [SerializeField] private TMP_Text m_armorText;
    [SerializeField] private TMP_Text m_manaText;
    [SerializeField] private TMP_Text m_coinsText;

    public PlayerMovement Movement => m_movement;
    private PlayerMovement m_movement;
    
    public PlayerStatus Status => m_status;
    private PlayerStatus m_status;

    private void Awake()
    {
        m_movement = GetComponent<PlayerMovement>();
        m_status = GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        m_stats.m_currentHealth = m_stats.MaxHealth;
        m_healthText.text = m_stats.m_currentHealth.ToString();
        m_armorText.text = m_stats.m_currentArmor.ToString();
        m_manaText.text = m_stats.m_currentMana.ToString();
        m_coinsText.text = m_stats.m_coins.ToString();
    }

    /// <summary>
    /// Reduce player's health. If the player has armor, it will reduce the armor first
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    public void Hit(int damage)
    {
        // Reduce all damage in half if it has Protection enabled
        if (Status.HasStatusApplied(EStatusType.Protection))
        {
            damage /= 2;
        }
        
        // If the player has armor and is not poisoned...
        if (m_stats.m_currentArmor > 0 && !m_status.HasStatusApplied(EStatusType.Poison))
        {
            // ... get the damage that couldn't be absorbed by the armor and apply it to the health
            int damageNotAbsorbedByArmor = Mathf.Abs(m_stats.m_currentArmor - damage);
            m_stats.m_currentArmor = Mathf.Clamp(m_stats.m_currentArmor - damage, 0, m_stats.m_currentArmor);
            m_stats.m_currentHealth = Mathf.Clamp(m_stats.m_currentHealth - damageNotAbsorbedByArmor, 0, m_stats.m_currentHealth);
        }
        else
        { // ... apply damage directly to player's health
            m_stats.m_currentHealth = Mathf.Clamp(m_stats.m_currentHealth - damage, 0, m_stats.m_currentHealth);   
        }
        
        // Check if the damage was enough to kill the player
        if (m_stats.m_currentHealth == 0)
        {
            // TODO: end game
            // TODO: add tween
            // TODO: sound
            Debug.Log("DEAD");
        }
        
        // Update texts
        m_healthText.text = m_stats.m_currentHealth.ToString();
        m_armorText.text = m_stats.m_currentArmor.ToString();
    }

    /// <summary>
    /// Heal player
    /// </summary>
    /// <param name="health">Heal value</param>
    public void RestoreHealth(int health)
    {
        m_stats.m_currentHealth = Mathf.Clamp(m_stats.m_currentHealth + health, m_stats.m_currentHealth, m_stats.MaxHealth);
        m_healthText.text = m_stats.m_currentHealth.ToString();
    }

    /// <summary>
    /// Add armor to the player
    /// </summary>
    /// <param name="armor">Armor value</param>
    public void RestoreArmor(int armor)
    {
        m_stats.m_currentArmor = Mathf.Clamp(m_stats.m_currentArmor + armor, m_stats.m_currentArmor, m_stats.MaxArmor);
        m_armorText.text = m_stats.m_currentArmor.ToString();
    }

    /// <summary>
    /// Increase current mana
    /// </summary>
    /// <param name="mana">Mana value</param>
    public void UpdateMana(int mana)
    {
        m_stats.m_currentMana = Mathf.Clamp(m_stats.m_currentMana + mana, 0, m_stats.MaxMana);
        m_manaText.text = m_stats.m_currentMana.ToString();
    }

    /// <summary>
    /// Add coins
    /// </summary>
    /// <param name="coins">The amount to add</param>
    public void AddCoins(int coins)
    {
        m_stats.m_coins += coins;
        m_coinsText.text = m_stats.m_coins.ToString();
    }
}
