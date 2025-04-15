using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;

public class ArcaneProtection : MagicAttack
{
    [Header("Arcane Protection Settings")]
    [SerializeField] private int m_minNumberOfTurns;
    [SerializeField] private int m_maxNumberOfTurns;
    
    [SerializeField] private StatusCardData m_statusData;
    
    public override void Cast()
    {
        if (!CanCast())
        {
            // TODO: add tween
            return;
        }

        // Do not cast this magic if the player is already invincible
        if (m_player.Status.HasStatusApplied(EStatusType.ArcaneProtection))
        {
            Debug.Log($"The player can't use <{nameof(MagicAttack)}> as it is already invincible");
            return;
        }
        
        m_player.Status.ApplyNewStatus(m_statusData, Random.Range(m_minNumberOfTurns, m_maxNumberOfTurns + 1));
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
