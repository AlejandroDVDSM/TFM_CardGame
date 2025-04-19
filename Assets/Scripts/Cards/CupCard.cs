namespace CardGame
{
    public class CupCard : Card
    {
        public override void PerformAction()
        {
            // TODO
            GameManager.Instance.EndGame(true);
        }
    }
}