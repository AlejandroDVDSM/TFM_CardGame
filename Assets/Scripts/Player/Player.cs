using System;
using CardGame.Enums;
using CardGame.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    
    public int MaxHealth => m_stats.MaxHealth;
    public int MaxArmor => m_stats.MaxArmor;
    public int MaxMana => m_stats.MaxMana;
    
    public int CurrentHealth => m_stats.m_currentHealth;
    public int CurrentArmor => m_stats.m_currentArmor;
    public int CurrentMana => m_stats.m_currentMana;
    public int Coins => m_stats.m_coins;
    
    public string CharacterName => m_characterData.Name;
    
    [SerializeField] private CharacterData m_characterData;
    
    [Space(10)]
    [Header("UI")]
    [SerializeField] private TMP_Text m_healthText;
    [SerializeField] private TMP_Text m_armorText;
    [SerializeField] private TMP_Text m_manaText;
    [SerializeField] private TMP_Text m_coinsText;
    [SerializeField] private Image m_characterImage;

    [SerializeField] private TMP_Text m_characterNameText;

    [Header("Debug")]
    [SerializeField] private bool m_fullArmorAtStart;
    [SerializeField] private bool m_fullManaAtStart;

    public PlayerMovement Movement => m_movement;
    private PlayerMovement m_movement;
    
    public PlayerStatus Status => m_status;

    private PlayerStatus m_status;
    
    private Stats m_stats;

    private void Awake()
    {
        m_movement = GetComponent<PlayerMovement>();
        m_status = GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // Set stats
        m_stats.MaxHealth = m_characterData.HealthPoints;
        m_stats.MaxArmor = m_characterData.Armor;
        m_stats.MaxMana = m_characterData.Mana;
        m_stats.m_currentHealth = m_stats.MaxHealth;
        
        // Debug: start game with full armor
        if (m_fullArmorAtStart)
        {
            m_stats.m_currentArmor = MaxArmor;
        }
        
        // Debug: start game with full mana
        if (m_fullManaAtStart)
        {
            m_stats.m_currentMana = MaxMana;
        }

        // Update UI
        m_healthText.text = CurrentHealth.ToString();
        m_armorText.text = CurrentArmor.ToString();
        m_manaText.text = CurrentMana.ToString();
        m_coinsText.text = m_stats.m_coins.ToString();
        
        m_characterNameText.text = m_characterData.Name;
        m_characterImage.sprite = m_characterData.Sprite;
    }

    /// <summary>
    /// Reduce player's health. If the player has armor, it will reduce the armor first
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    public void Hit(int damage)
    {
        // Make player invincible while having Arcane Protection status enabled
        if (m_status.HasStatusApplied(EStatusType.ArcaneProtection))
        {
            return;
        }
        
        // Reduce all damage in half if it has Protection enabled
        if (Status.HasStatusApplied(EStatusType.Protection))
        {
            damage /= 2;
        }
        
        // If the player has armor and is not poisoned...
        if (CurrentArmor > 0 && !m_status.HasStatusApplied(EStatusType.Poison))
        {
            // ... get the damage that couldn't be absorbed by the armor and apply it to the health
            int damageNotAbsorbedByArmor = 0;
            if (CurrentArmor - damage < 0)
            {
                damageNotAbsorbedByArmor = Mathf.Abs(CurrentArmor - damage);
            }
            
            m_stats.m_currentArmor = Mathf.Clamp(CurrentArmor - damage, 0, CurrentArmor);
            m_stats.m_currentHealth = Mathf.Clamp(CurrentHealth - damageNotAbsorbedByArmor, 0, CurrentHealth);
        }
        else
        { // ... apply damage directly to player's health
            m_stats.m_currentHealth = Mathf.Clamp(CurrentHealth - damage, 0, CurrentHealth);   
        }

        // Feedback that player got hit
        transform.DOShakePosition(1f, 10f);
        
        // Check if the damage was enough to kill the player
        if (CurrentHealth == 0)
        {
            // TODO: end game
            // TODO: add tween
            // TODO: sound
            Debug.Log("DEAD");
        } else if (CurrentHealth < MaxHealth / 2)
        {
            m_characterImage.DOColor(Color.red, 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        
        // Update texts
        m_healthText.text = CurrentHealth.ToString();
        m_armorText.text = CurrentArmor.ToString();
    }

    /// <summary>
    /// Heal player
    /// </summary>
    /// <param name="health">Heal value</param>
    public void RestoreHealth(int health)
    {
        m_stats.m_currentHealth = Mathf.Clamp(CurrentHealth + health, CurrentHealth, MaxHealth);
        m_healthText.text = CurrentHealth.ToString();

        if (CurrentHealth >= MaxHealth / 2)
        {
            m_characterImage.DOKill();
            m_characterImage.DOColor(Color.white, 0.2f).SetEase(Ease.InSine);
        }
    }

    /// <summary>
    /// Add armor to the player
    /// </summary>
    /// <param name="armor">Armor value</param>
    public void RestoreArmor(int armor)
    {
        m_stats.m_currentArmor = Mathf.Clamp(CurrentArmor + armor, CurrentArmor, MaxArmor);
        m_armorText.text = CurrentArmor.ToString();
    }

    /// <summary>
    /// Increase current mana
    /// </summary>
    /// <param name="mana">Mana value</param>
    public void UpdateMana(int mana)
    {
        m_stats.m_currentMana = Mathf.Clamp(CurrentMana + mana, 0, MaxMana);
        m_manaText.text = CurrentMana.ToString();
    }

    /// <summary>
    /// Swap health stat for armor and viceversa
    /// </summary>
    public void SwapStats()
    {
        int health = CurrentHealth;
        int armor = CurrentArmor;
        
        m_stats.m_currentHealth = Mathf.Clamp(armor, armor, MaxHealth);
        m_stats.m_currentArmor = Mathf.Clamp(health, health, MaxArmor);
        
        m_healthText.text = CurrentHealth.ToString();
        m_armorText.text = CurrentArmor.ToString();
    }
    
    /// <summary>
    /// Add coins
    /// </summary>
    /// <param name="coins">The amount to add</param>
    public void UpdateCoins(int coins)
    {
        m_stats.m_coins += coins;
        m_coinsText.text = m_stats.m_coins.ToString();
    }
}
