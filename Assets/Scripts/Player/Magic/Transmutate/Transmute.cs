using CardGame;
using CardGame.Enums;
using UnityEngine;

public class Transmute : MagicAttack
{
    public bool IsTransmuting => m_IsTransmuting;
    
    private bool m_IsTransmuting;
    private Card m_pickedCardToTransmute;
    private TransmuteView m_transmuteView;

    protected override void Start()
    {
        base.Start();
        m_transmuteView = m_magicUI as  TransmuteView;
    }

    // We don't use Cast here. We use ApplyItem
    public override void Cast()
    {
        // if (!CanCast())
        // {
        //     // TODO: add tween
        //     return;
        // }
        //
        // m_player.UpdateMana(m_magicData.ManaCost * -1);
        // hasUsedMagic = true;
    }

    public void EnableTransmutingMode()
    {
        // TODO: tween to give the feedback that we are in transmuting mode 
        m_IsTransmuting = true;
        m_transmuteView.EnableTransmuteModeUI();
    }

    public void PickCard(Card card)
    {
        m_pickedCardToTransmute = card;
        m_transmuteView.ShowItemCards();
    }

    public void ApplyItem(EItemType type, int value)
    {
        if (!CanCast())
        {
            // TODO: add tween
            return;
        }
        
        // Get an ItemCard from the pool
        ItemCard itemCard = GameManager.Instance.CardPool.ExtractItemCardOfType(type);
        itemCard.UpdateValue(value);

        // Get the row where the picked card to transmute is
        CardRow row = null;
        switch (m_pickedCardToTransmute.CurrentRow)
        {
            case ERow.Top:
                row = GameManager.Instance.TopRow;
                break;
            
            case ERow.Middle:
                row = GameManager.Instance.MiddleRow;
                break;
            
            case ERow.Bottom:
                row = GameManager.Instance.BottomRow;
                break;
        }
        
        // Place the ItemCard in the same place where the picked card is and destroy it
        row.PlaceSingleCard(itemCard, (int)m_pickedCardToTransmute.Lane, itemCard.transform.GetSiblingIndex());
        GameManager.Instance.CardPool.DestroyCard(m_pickedCardToTransmute);
        m_IsTransmuting = false;
        
        m_player.UpdateMana(m_magicData.ManaCost * -1);
        hasUsedMagic = true;
    }
}
