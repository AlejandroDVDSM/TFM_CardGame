using UnityEngine;

public class SwapStat : MagicAttack
{
    public override void Cast()
    {
        if (!CanCast())
        {
            // TODO: add tween
            return;
        }

        // If the player doesn't have armor it doesn't make sense to swap the stats because
        // it will end up getting 0 hp in the exchange
        if (m_player.CurrentArmor == 0)
        {
            Debug.Log($"The player can't use <{nameof(MagicAttack)}> as its armor is 0");
            return;
        }
        
        m_player.SwapStats();
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
