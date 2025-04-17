using CardGame.Enums;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;

public class Blessing : MagicAttack
{
    private BlessingView m_blessingView;
    private StatusCardData m_pickedBlessing;
    
    protected override void Start()
    {
        base.Start();
        
        m_blessingView = m_magicUI as BlessingView;
    }

    public override void Cast()
    {
        if (!CanCast())
        {
            return;
        }

        if (!m_blessingView)
        {
            Debug.LogError($"Couldn't cast magic view as {nameof(BlessingView)}");
            return;
        }
        
        m_blessingView.CloseBlessingPanel();
        GameManager.Instance.Player.Status.ApplyNewStatus(m_pickedBlessing, 2);
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }

    public void ApplyBlessing(StatusCardData blessing)
    {
        m_pickedBlessing = blessing;
        Cast();
    }
}
