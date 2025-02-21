namespace CardGame
{
    public class EnemyCard : Card
    {
        public override void PerformAction()
        {
            // TODO: tween (?)
            GameManager.Instance.Player.Hit(m_value);
        }
    }
}