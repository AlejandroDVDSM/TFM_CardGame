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
                GameManager.Instance.Player.Status.ApplyNewStatus(itemCardData, Value);
            }
            else
            {
                Debug.LogError($"This card is not of type {typeof(StatusCard)}");
            }
            
            EnableDisappearAnimation(() =>
            {
                m_isPerformingAction = false;
                GameManager.Instance.CommitTurn();
            });
        }
    }
}