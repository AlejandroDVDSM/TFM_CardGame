using ScriptableObjects;
using UnityEngine;

namespace CardGame
{
    public class StatusCard : Card
    {
        public override void PerformAction()
        {
            m_isPerformingAction = true;
            
            StatusCardData itemCardData = m_data as StatusCardData;

            if (itemCardData)
            {
                GameManager.Instance.Player.Status.ApplyNewStatus(itemCardData.Status, Value);
            }
            else
            {
                Debug.LogError($"This card is not of type {typeof(StatusCard)}");
            }
            
            // GameManager.Instance.CommitTurn();
            // m_isPerformingAction = false;
            
            
            EnableDisappearAnimation(() =>
            {
                m_isPerformingAction = false;
                GameManager.Instance.CommitTurn();
            });
        }
    }
}