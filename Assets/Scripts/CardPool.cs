using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardPool : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;
        
        [Min(0)]
        [SerializeField] private int poolSize = 12; // 3 card x 4 rows

        private List<Card> _cardsPool;

        /// <summary>
        /// Initialize a pool
        /// </summary>
        public void InitPool()
        {
            _cardsPool = new List<Card>();

            for (int i = 0; i < poolSize; i++)
            {
                Card card = Instantiate(cardPrefab, transform);
                _cardsPool.Add(card);
                card.IsInPool = true;
            }
        }

        
        public void CreateCard()
        {
            
        }

        /// <summary>
        /// Bring a card back to the pool
        /// </summary>
        /// <param name="card">Card that will go back to the pool</param>
        public void DestroyCard(Card card)
        {
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.DisableCard();
        }

        /// <summary>
        /// Sends cards back to the pool
        /// </summary>
        /// <param name="card">A range of cards to send back to the pool</param>
        public void DestroyCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                card.transform.SetParent(transform);
                card.transform.localPosition = Vector3.zero;
                card.DisableCard();
            }
        }
        

        /// <summary>
        /// Picks a random card from the pool
        /// </summary>
        /// <returns>A card</returns>
        public Card ExtractCardFromPool()
        {
            List <Card> inactiveCards = _cardsPool.FindAll(card => card.IsInPool);
            Card card = inactiveCards[Random.Range(0, _cardsPool.Count)];
            card.IsInPool = false;
            return card;
        }

        /// <summary>
        /// Picks the first three cards from the pool
        /// </summary>
        /// <returns>A list containing three cards</returns>
        public List<Card> ExtractRangeFromPool()
        {
            List <Card> cards = _cardsPool.FindAll(card => card.IsInPool).GetRange(0, 3);
            
            foreach (Card card in cards)
                card.IsInPool = false;
            
            return cards;
        }
    }
}