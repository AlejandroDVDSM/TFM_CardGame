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

        public void DestroyCard()
        {
            
        }

        public Card ExtractCardFromPool()
        {
            List <Card> inactiveCards = _cardsPool.FindAll(card => card.IsInPool);
            Card card = inactiveCards[Random.Range(0, _cardsPool.Count)];
            card.IsInPool = false;
            return card;
        }

        public List<Card> ExtractRangeFromPool()
        {
            List <Card> cards = _cardsPool.FindAll(card => card.IsInPool).GetRange(0, 3);
            
            foreach (Card card in cards)
                card.IsInPool = false;
            
            return cards;
        }
    }
}