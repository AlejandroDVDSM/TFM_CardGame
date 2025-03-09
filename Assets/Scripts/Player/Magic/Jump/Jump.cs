public class Jump : MagicAttack
{
    public override void Cast()
    {
        if (!CanCast())
        {
            // TODO: add tween
            return;
        }
        
        GameManager.Instance.CommitTurn();
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
