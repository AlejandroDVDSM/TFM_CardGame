using UnityEngine;

public class Shuffle : MagicAttack
{
    public override void Cast()
    {
        if (!CanCast())
        {
            // TODO: add tween
            return;
        }
        
        foreach (var card in GameManager.Instance.CardPool.GetCardsOutsidePool())
        {
            GameManager.Instance.CardPool.DestroyCard(card);
        }
        
        GameManager.Instance.TopRow.PopulateRow();
        GameManager.Instance.MiddleRow.PopulateRow();
        GameManager.Instance.BottomRow.PopulateRow();
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
