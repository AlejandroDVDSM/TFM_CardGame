using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame
{
    public class CardPool : MonoBehaviour
    {
        [Header("Card Data")]
        [SerializeField] private Card cardPrefab;
        [SerializeField] private CardCollection enemiesCollection;
        [SerializeField] private CardCollection itemsCollection;
        [SerializeField] private CardCollection statusesCollection;
        [SerializeField] private BaseCardData cupCardData;
        
        [Header("Pool Config")]
        [Min(0)]
        [SerializeField] private int poolSize = 12; // 3 card x 4 rows

        private List<Card> _cardsPool;

        private void Awake()
        {
            InitPool();
        }

        /// <summary>
        /// Initialize a pool
        /// </summary>
        private void InitPool()
        {
            _cardsPool = new List<Card>();

            for (int i = 0; i < poolSize; i++)
            {
                Card card = Instantiate(cardPrefab, transform);
                card.IsInPool = true;
                _cardsPool.Add(card);
            }
        }

        /// <summary>
        /// Sends cards back to the pool
        /// </summary>
        /// <param name="cards">A range of cards to send back to the pool</param>
        public void DestroyCards(List<Card> cards)
        {
            foreach (Card card in cards)
                DestroyCard(card);
        }

        /// <summary>
        /// Picks the first three cards from the pool
        /// </summary>
        /// <returns>A list containing three cards</returns>
        public List<Card> ExtractRangeFromPool()
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < 3; i++)
            {
                Card card = ExtractCardFromPool();

                switch (Random.Range(0, 3))
                {
                    // Enemy
                    case 0:
                        card.SetCard(enemiesCollection.GetRandomCard());
                        break;
                    
                    // Items
                    case 1:
                        card.SetCard(itemsCollection.GetRandomCard());
                        break;
                    
                    // Status
                    case 2:
                        card.SetCard(statusesCollection.GetRandomCard());
                        break;
                }
                
                cards.Add(card);
            }
            
            return cards;
        }

        /// <summary>
        /// Extract three cups from the pool
        /// </summary>
        /// <returns>A list of cards containing three cups</returns>
        public List<Card> ExtractCupsFromPool()
        {
            List<Card> cards = new List<Card>();
            
            for (int i = 0; i < 3; i++)
            {
                Card card = ExtractCardFromPool();
                card.SetCard(cupCardData/* itemsCollection.GetItemCardByType(EItemType.CUP) */);

                cards.Add(card);
            }
            
            return cards;
        }

        /// <summary>
        /// Picks a random card from the pool
        /// </summary>
        /// <returns>A card</returns>
        private Card ExtractCardFromPool()
        {
            Card card = _cardsPool.First(c => c.IsInPool);
            card.IsInPool = false;
            return card;
        }
        
        
        /// <summary>
        /// Bring a card back to the pool
        /// </summary>
        /// <param name="card">Card that will go back to the pool</param>
        private void DestroyCard(Card card)
        {
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.DisableCard();
            card.IsInPool = true;
        }
    }
}