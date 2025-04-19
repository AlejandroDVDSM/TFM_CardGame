using System.Collections.Generic;
using System.Linq;
using CardGame;
using CardGame.Enums;
using UnityEngine;

public class BasicDamage : MagicAttack
{
    [Min(1)]
    [SerializeField] private int m_damage;
    
    private int m_nextManaCost;
    private int m_laneManaCost;
    private int m_allManaCost;
    
    private EBasicDamageMode m_selectedMode = EBasicDamageMode.Next;

    protected override void Start()
    {
        base.Start();
        
        // TODO: improve mana cost formula
        m_nextManaCost = m_magicData.ManaCost;
        m_laneManaCost = m_magicData.ManaCost + 3;
        m_allManaCost = m_magicData.ManaCost + 6;
    }

    public override void Cast()
    {
        if (!CanCast())
        {
            AudioManager.Instance.Play("Denied");
            return;
        }

        switch (m_selectedMode)
        {
            case EBasicDamageMode.Next:
                CastToNextEnemy();
                break;
            
            case EBasicDamageMode.Lane:
                CastToLane();
                break;
            
            case EBasicDamageMode.All:
                CastToAll();
                break;
        }
        
        AudioManager.Instance.Play("BasicDamageCast");
    }

    /// <summary>
    /// Cast magic to hit enemies next to the player
    /// </summary>
    private void CastToNextEnemy()
    {
        EnemyCard enemy = GameManager.Instance.CardPool.GetEnemiesOutsidePool().FirstOrDefault(e =>
            e.Lane == m_player.Movement.CurrentLane && e.CurrentRow == ERow.Middle);

        if (enemy != null)
        {
            enemy.Hit(m_damage);
            hasUsedMagic = true;
            m_player.UpdateMana(m_nextManaCost * -1);
        }
        else
        {
            Debug.LogWarning("[MagicAttack - BasicDamage] Could not find an enemy next to the player");
        }
    }

    /// <summary>
    /// Cast magic to hit enemies in the same lane as the player
    /// </summary>
    private void CastToLane()
    {
        if (m_player.CurrentMana < m_laneManaCost)
        {
            return;
        }
        
        List<EnemyCard> enemies = GameManager.Instance.CardPool.GetEnemiesOutsidePool().Where(e =>
            e.Lane == m_player.Movement.CurrentLane).ToList();

        if (enemies.Count == 0)
        {
            return;
        }

        foreach (var enemy in enemies)
        {
            enemy.Hit(m_damage);
        }
        
        hasUsedMagic = true;
        m_player.UpdateMana(m_laneManaCost * -1);
    }

    /// <summary>
    /// Cast magic to hit all enemies
    /// </summary>
    private void CastToAll()
    {
        if (m_player.CurrentMana < m_allManaCost)
        {
            return;
        }

        List<EnemyCard> enemies = GameManager.Instance.CardPool.GetEnemiesOutsidePool();

        if (enemies.Count == 0)
        {
            return;
        }

        foreach (var enemy in enemies)
        {
            enemy.Hit(m_damage);
        }
        
        hasUsedMagic = true;
        m_player.UpdateMana(m_allManaCost * -1);
        
    }
    
    public void SetMode(EBasicDamageMode mode)
    {
        m_selectedMode = mode;
        // Debug.Log(mode);
    }
    
    public int GetManaCost(EBasicDamageMode mode)
    {
        switch (mode)
        {
            case EBasicDamageMode.Next:
                return m_nextManaCost;
            case EBasicDamageMode.Lane:
                return m_laneManaCost;
            case EBasicDamageMode.All:
                return m_allManaCost;
        }

        return -1;
    }

}
