using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;

namespace CardGame
{
    public class ItemCard : Card
    {
        public override void PerformAction()
        {
            ItemCardData itemCardData = m_data as ItemCardData;
            
            switch (itemCardData?.Type)
            {
                case EItemType.Health:
                    GameManager.Instance.Player.RestoreHealth(m_value);
                    break;
                
                case EItemType.Mana:
                    GameManager.Instance.Player.UpdateMana(m_value);
                    break;
                
                case EItemType.Coin:
                    GameManager.Instance.Player.AddCoins(m_value);
                    break;
                
                case EItemType.Armor:
                    GameManager.Instance.Player.RestoreArmor(m_value);
                    break;
            }
            
            GameManager.Instance.CommitTurn();
        }
    }
}