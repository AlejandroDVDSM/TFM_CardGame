using System.Collections.Generic;
using CardGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fireball : MagicAttack
{
    [Header("Fireball")]
    
    [Min(1)]
    [SerializeField] private int m_damage;
    
    [SerializeField] private Image m_fireballFxPrefab;

    private Image m_fireballFxPlaced;
    
    public override void Cast()
    {
        if (!CanCast())
        {
            return;
        }

        // Check if there are enemies outside the pool to hit them. If there are not, we don't cast the magic
        List<EnemyCard> enemies = GameManager.Instance.CardPool.GetEnemiesOutsidePool();
        if (enemies.Count == 0)
        {
            Debug.Log($"There is no enemy outside the pool to cast <{nameof(Fireball)}>");
            return;
        }

        // Instantiate fireball fx
        if (!m_fireballFxPlaced)
        {
            m_fireballFxPlaced = Instantiate(m_fireballFxPrefab, m_player.transform);
            
            // Harcoded value
            m_fireballFxPlaced.transform.localPosition = new Vector3(0, -1200, 0);
        }

        // Move the fireball from the bottom of the screen to the top
        m_fireballFxPlaced.transform.DOLocalMoveY(1600, 1.2f)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                // Harcoded value
                m_fireballFxPlaced.transform.localPosition = new Vector3(0, -1200, 0);
                
                // Hit all enemies
                enemies.ForEach(e => e.Hit(m_damage));
            });
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
