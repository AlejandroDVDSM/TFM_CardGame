namespace CardGame
{
    public class EnemyCard : Card
    {
        protected override void PerformAction()
        {
            // TODO: tween (?)
            GameManager.Instance.Player.Hit(cardValue);
        }
    }
}