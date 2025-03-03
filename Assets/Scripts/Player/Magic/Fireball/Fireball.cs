using System.Collections.Generic;
using CardGame;
using UnityEngine;

public class Fireball : MagicAttack
{
    [Min(1)]
    [SerializeField] private int m_damage;

    public override void Cast()
    {
        if (!CanCast())
        {
            // TODO: add tween
            return;
        }

        List<EnemyCard> enemies = GameManager.Instance.CardPool.GetEnemiesOutsidePool();
        
        if (enemies.Count == 0)
        {
            Debug.Log($"There is no enemy outside the pool to cast <{nameof(Fireball)}>");
            return;
        }
        
        // TODO: add tween to reflects that cards are taking damage
        enemies.ForEach(e => e.Hit(m_damage));
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
