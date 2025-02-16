namespace CardGame
{
    public class EnemyCard : Card
    {
        protected override void PerformAction()
        {
            // TODO: tween (?)
            
            // TODO: fix card applying damage when the player can't move to the selected card
            GameManager.Instance.Player.Hit(cardValue);
        }
    }
}