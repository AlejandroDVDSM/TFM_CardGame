using CardGame.Enums;
using ScriptableObjects;

namespace CardGame
{
    public class ItemCard : Card
    {
        public override void PerformAction()
        {
            m_isPerformingAction = true;
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
                    GameManager.Instance.Player.UpdateCoins(m_value);
                    break;
                
                case EItemType.Armor:
                    GameManager.Instance.Player.RestoreArmor(m_value);
                    break;
                
                case EItemType.Chest:
                    FindAnyObjectByType<Shop>().OpenShop(true);
                    AudioManager.Instance.Play("PickupChest");
                    break;
                    
            }
            
            EnableDisappearAnimation(() =>
            {
                m_isPerformingAction = false;
                GameManager.Instance.CommitTurn();
            });
        }
    }
}