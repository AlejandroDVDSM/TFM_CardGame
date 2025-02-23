using CardGame.Enums;

namespace CardGame
{
    public class EnemyCard : Card
    {
        private void Start()
        {
            GameManager.Instance.OnTurnCommited.AddListener(CheckPlayerPosition);
        }

        public override void PerformAction()
        {            
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
        
        private void CheckPlayerPosition()
        {
            if (GameManager.Instance.Player.Movement.CurrentLane == Lane && m_currentRow == ERow.Bottom)
            {
                PerformAction();
            }
        }
    }
}