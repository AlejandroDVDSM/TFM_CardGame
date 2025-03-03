using CardGame.Enums;
using UnityEngine;

namespace CardGame
{
    public class EnemyCard : Card
    {
        private void Start()
        {
            GameManager.Instance.OnTurnCommited.AddListener(AutoAttack);
        }

        public override void PerformAction()
        {
            // The enemy will avoid the player if it is invisible
            if (GameManager.Instance.Player.Status.HasStatusApplied(EStatusType.Invisibility))
            {
                GameManager.Instance.CommitTurn();
                return;
            }
            
            // Hit player
            GameManager.Instance.Player.Hit(m_value);
            
            // Replace this enemy card for a coin card
            ItemCard coinCard = GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Coin);
            coinCard.UpdateValue(Value);
            
            // TODO: add tween before replacing to card. The effect has to be like an attack
            
            GetComponentInParent<CardRow>().PlaceSingleCard(coinCard, Lane);
            GameManager.Instance.CardPool.DestroyCard(this);
            // StartCoroutine(Test());
        }

        public void Hit(int damage)
        {
            UpdateValue(Mathf.Clamp(m_value - damage, 0, m_value));
        }

        /// <summary>
        /// Automatically attacks a player after commiting a turn if the player is in the enemy line of sight
        /// </summary>
        private void AutoAttack()
        {
            if (GameManager.Instance.Player.Status.HasStatusApplied(EStatusType.Invisibility))
            {
                return;
            }
            
            if (GameManager.Instance.Player.Movement.CurrentLane == Lane && m_currentRow == ERow.Bottom)
            {
                PerformAction();
            }
        }
        
        // private IEnumerator Test()
        // {
        //     // Replace this enemy card for a coin card
        //     ItemCard coinCard = GameManager.Instance.CardPool.ExtractItemCardOfType(EItemType.Coin);
        //     coinCard.UpdateValue(Value);
        //     
        //     // TODO: add tween before replacing to card. The effect has to be like an attack
        //     yield return new WaitForSeconds(1);
        //     
        //     GetComponentInParent<CardRow>().PlaceSingleCard(coinCard, Lane);
        //     GameManager.Instance.CardPool.DestroyCard(this);
        // }
    }
}