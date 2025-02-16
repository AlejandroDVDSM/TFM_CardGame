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
        [Header("Cards Prefab")]
        // [SerializeField] private Card cardPrefab;
        [SerializeField] private EnemyCard enemyCardPrefab;
        [SerializeField] private ItemCard itemCardPrefab;
        [SerializeField] private StatusCard statusCardPrefab;
        [SerializeField] private CupCard cupCardPrefab;
        
        [Header("Card Data")]
        [SerializeField] private CardCollection enemiesCollection;
        [SerializeField] private CardCollection itemsCollection;
        [SerializeField] private CardCollection statusesCollection;
        [SerializeField] private BaseCardData cupCardData;
        
        [Header("Pool Config")]
        [Min(3)]
        [SerializeField] private int poolSize; 

        private List<EnemyCard> _enemiesPool;
        private List<ItemCard> _itemsPool;
        private List<StatusCard> _statusesPool;
        private List<CupCard> _cupsPool;

        private void Awake()
        {
            InitPools();
        }

        #region Pool

        /// <summary>
        /// Initialize all pools
        /// </summary>
        private void InitPools()
        {
            InitEnemyPool();
            InitItemPool();
            InitStatusPool();
            InitCupPool();
        }

        private void InitEnemyPool()
        {
            _enemiesPool = new List<EnemyCard>();

            for (int i = 0; i < poolSize; i++)
            {
                EnemyCard enemyCard = Instantiate(enemyCardPrefab, transform);
                enemyCard.IsInPool = true;
                _enemiesPool.Add(enemyCard);
            }
        }
        
        private void InitItemPool()
        {
            _itemsPool = new List<ItemCard>();

            for (int i = 0; i < poolSize; i++)
            {
                ItemCard itemCard = Instantiate(itemCardPrefab, transform);
                itemCard.IsInPool = true;
                _itemsPool.Add(itemCard);
            }
        }
        
        private void InitStatusPool()
        {
            _statusesPool = new List<StatusCard>();

            for (int i = 0; i < poolSize; i++)
            {
                StatusCard statusCard = Instantiate(statusCardPrefab, transform);
                statusCard.IsInPool = true;
                _statusesPool.Add(statusCard);
            }
        }
        
        private void InitCupPool()
        {
            _cupsPool = new List<CupCard>();

            for (int i = 0; i < poolSize; i++)
            {
                CupCard cupCard = Instantiate(cupCardPrefab, transform);
                cupCard.IsInPool = true;
                _cupsPool.Add(cupCard);
            }
        }
        
        #endregion

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
        /// Bring a card back to the pool
        /// </summary>
        /// <param name="card">Card that will go back to the pool</param>
        private void DestroyCard(Card card)
        {
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.DisableCard();
            card.IsInPool = true;
            // Debug.Log($"Destroy card: {card} - {card.GetInstanceID()}");
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
                Card card = null;

                switch (Random.Range(0, 3))
                {
                    // Enemy
                    case 0:
                        card = ExtractCardFromPool(ECardType.ENEMY);
                        break;
                    
                    // Items
                    case 1:
                        card = ExtractCardFromPool(ECardType.ITEM);
                        break;
                    
                    // Status
                    case 2:
                        card = ExtractCardFromPool(ECardType.STATUS);
                        break;
                }

                if (card != null)
                {
                    cards.Add(card);
                    card.IsInPool = false;
                }
                else
                    Debug.LogError("Card is null");
            
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
                Card card = ExtractCardFromPool(ECardType.CUP);
                cards.Add(card);
                card.IsInPool = false;
            }
            
            return cards;
        }

        /// <summary>
        /// Picks a random card from the pool
        /// </summary>
        /// <returns>A card</returns>
        private Card ExtractCardFromPool(ECardType cardType)
        {
            Card card = null;
            switch (cardType)
            {
                case ECardType.ENEMY:
                    card = _enemiesPool.First(c => c.IsInPool);
                    card.SetCard(enemiesCollection.GetRandomCard());
                    break;
                
                case ECardType.ITEM:
                    card = _itemsPool.First(c => c.IsInPool);
                    card.SetCard(itemsCollection.GetRandomCard());
                    break;
                
                case ECardType.STATUS:
                    card = _statusesPool.First(c => c.IsInPool);
                    card.SetCard(statusesCollection.GetRandomCard());
                    break;
                
                case ECardType.CUP:
                    card = _cupsPool.First(c => c.IsInPool);
                    card.SetCard(cupCardData);

                    break;
            }
            
            return card;
        }
    }
}