using CardGame.Enums;
using ScriptableObjects;

namespace CardGame
{
    public class StatusCard : Card
    {
        public override void PerformAction()
        {
            if (!m_data is StatusCardData)
                return;
            
            StatusCardData itemCardData = m_data as StatusCardData;
            
            switch (itemCardData.Status)
            {
                case EStatusType.Poison:
                    GameManager.Instance.Player.Status.ApplyPoison(Value);
                    break;
                
                case EStatusType.Blind:
                    // TODO
                    break;
                
                case EStatusType.Silence:
                    // TODO
                    break;
                
                case EStatusType.Invisibility:
                    // TODO
                    break;
                
                case EStatusType.Regeneration:
                    // TODO
                    break;
                
                case EStatusType.Protection:
                    // TODO
                    break;
            }

        }
    }
}