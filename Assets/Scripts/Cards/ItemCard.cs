using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;

namespace CardGame
{
    public class ItemCard : Card
    {
        public override void PerformAction()
        {
            if (!m_cardData is ItemCardData)
                return;
            
            ItemCardData itemCardData = m_cardData as ItemCardData;
            
            switch (itemCardData.Type)
            {
                case EItemType.Health:
                    GameManager.Instance.Player.RestoreHealth(m_cardValue);
                    break;
                
                case EItemType.Mana:
                    GameManager.Instance.Player.RestoreMana(m_cardValue);
                    break;
                
                case EItemType.Coin:
                    GameManager.Instance.Player.AddCoins(m_cardValue);
                    break;
                
                case EItemType.Armor:
                    GameManager.Instance.Player.RestoreArmor(m_cardValue);
                    break;
            }

        }
    }
}