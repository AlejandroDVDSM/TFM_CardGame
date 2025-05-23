﻿using System.Collections.Generic;
using System.Linq;
using CardGame;
using CardGame.Enums;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardPool : MonoBehaviour
{
    [Header("Cards Prefab")]
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
    [SerializeField] private int enemiesPoolSize;
    [SerializeField] private int itemsPoolSize;
    [SerializeField] private int statusesPoolSize;
    
    private List<Card> m_cardsPool;
    private const int k_CupsPoolSize = 3;

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
        m_cardsPool = new List<Card>();

        for (int i = 0; i < enemiesPoolSize; i++)
        {
            EnemyCard enemyCard = Instantiate(enemyCardPrefab, transform);
            enemyCard.IsInPool = true;
            m_cardsPool.Add(enemyCard);
        }
        for (int i = 0; i < itemsPoolSize; i++)
        {
            ItemCard itemCard = Instantiate(itemCardPrefab, transform);
            itemCard.IsInPool = true;
            m_cardsPool.Add(itemCard);
        }
        for (int i = 0; i < statusesPoolSize; i++)
        {
            StatusCard statusCard = Instantiate(statusCardPrefab, transform);
            statusCard.IsInPool = true;
            m_cardsPool.Add(statusCard);
        }
        for (int i = 0; i < k_CupsPoolSize; i++)
        {
            CupCard cupCard = Instantiate(cupCardPrefab, transform);
            cupCard.IsInPool = true;
            m_cardsPool.Add(cupCard);
        }
    }
    
    #endregion
    
    public List<Card> GetCardsInRow(ERow row)
    {
        return GetCardsOutsidePool().Where(c => c.CurrentRow == row).ToList();
    }
    
    /// <summary>
    /// Get all the EnemyCard that are outside the pool 
    /// </summary>
    /// <returns></returns>
    public List<EnemyCard> GetEnemiesOutsidePool()
    {
        return m_cardsPool.OfType<EnemyCard>().Where(card => !card.IsInPool).ToList();
    }

    /// <summary>
    /// Get all the cards that are outside the pool without filtering by any type
    /// </summary>
    /// <returns></returns>
    public List<Card> GetCardsOutsidePool()
    {
        return m_cardsPool.Where(c => !c.IsInPool).ToList();
    }
    
    /// <summary>
    /// Sends cards back to the pool
    /// </summary>
    /// <param name="cards">A range of cards to send back to the pool</param>
    public void DestroyCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            DestroyCard(card);
        }
    }
    
    /// <summary>
    /// Bring a card back to the pool
    /// </summary>
    /// <param name="card">Card that will go back to the pool</param>
    public void DestroyCard(Card card)
    {
        card.DestroyCard(); 
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
                    card = ExtractCardFromPool(ECardType.Enemy);
                    break;
                
                // Items
                case 1:
                    card = ExtractCardFromPool(ECardType.Item);
                    break;
                
                // Status
                case 2:
                    card = ExtractCardFromPool(ECardType.Status);
                    break;
            }

            if (card != null)
                cards.Add(card);
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
            Card card = ExtractCardFromPool(ECardType.Cup);
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
            case ECardType.Enemy:
                card = m_cardsPool.OfType<EnemyCard>().FirstOrDefault(c => c.IsInPool);
                
                // If we didn't find an EnemyCard, try with another type of card
                if (!card)
                    card = ExtractCardFromPool(ECardType.Item);
                    
                break;
            
            case ECardType.Item:
                card = m_cardsPool.OfType<ItemCard>().FirstOrDefault(c => c.IsInPool);
                              
                // If we didn't find an ItemCard, try with another type of card
                if (!card)
                    card = ExtractCardFromPool(ECardType.Status);
                
                break;
            
            case ECardType.Status:
                card = m_cardsPool.OfType<StatusCard>().FirstOrDefault(c => c.IsInPool);
                                    
                // If we didn't find an EnemyCard, try with another type of card
                if (!card)
                    card = ExtractCardFromPool(ECardType.Enemy);
                
                break;
            
            case ECardType.Cup:
                card = m_cardsPool.OfType<CupCard>().FirstOrDefault(c => c.IsInPool);
                
                // This scenario shouldn't happen as we only spawn three cups during a game
                if (!card)
                {
                    Debug.LogError($"[POOL] Could not find a <{typeof(CupCard)}> in the pool");
                    return null;
                }

                break;
        }

        if (card?.GetType() == typeof(EnemyCard))
            card.SetData(enemiesCollection.GetRandomData());
        else if (card?.GetType() == typeof(ItemCard))
            card.SetData(itemsCollection.GetRandomData());
        else if (card?.GetType() == typeof(StatusCard))
            card.SetData(statusesCollection.GetRandomData());
        else if (card?.GetType() == typeof(CupCard))
            card.SetData(cupCardData);
        
        if (card)
            card.IsInPool = false;
        
        return card;
    }

    /// <summary>
    /// Get a item card
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public ItemCard ExtractItemCardOfType(EItemType itemType)
    {
        ItemCard itemCard = m_cardsPool.OfType<ItemCard>().FirstOrDefault(c => c.IsInPool);

        if (!itemCard)
        { 
            // If there is no ItemCard in the pool, create one and add it
            itemCard = Instantiate(itemCardPrefab, transform);
            itemCard.IsInPool = true;
            m_cardsPool.Add(itemCard);
        }
        
        itemCard.SetData(itemsCollection.GetItemDataByType(itemType));
        itemCard.IsInPool = false;
        return itemCard;
    }
    
    /// <summary>
    /// Get a status card
    /// </summary>
    /// <param name="statusType">Status to get</param>
    /// <returns>A status card matching the desired type</returns>
    public StatusCard ExtractStatusCardOfType(EStatusType statusType)
    {
        StatusCard statusCard = m_cardsPool.OfType<StatusCard>().FirstOrDefault(c => c.IsInPool);

        if (!statusCard)
        {
            // If there is no StatusCard in the pool, create one and add it
            statusCard = Instantiate(statusCardPrefab, transform);
            statusCard.IsInPool = true;
            m_cardsPool.Add(statusCard);
        }
        
        statusCard.SetData(statusesCollection.GetStatusDataByType(statusType));
        statusCard.IsInPool = false;
        return statusCard;
    }

    /// <summary>
    /// Get an enemy card
    /// </summary>
    /// <returns>An enemy card</returns>
    public EnemyCard ExtractEnemyCard()
    {
        EnemyCard enemyCard = m_cardsPool.OfType<EnemyCard>().FirstOrDefault(c => c.IsInPool);

        if (!enemyCard)
        {
            enemyCard = Instantiate(enemyCardPrefab, transform);
            enemyCard.IsInPool = true;
            m_cardsPool.Add(enemyCard);
        }
        
        enemyCard.SetData(enemiesCollection.GetRandomData());
        enemyCard.IsInPool = false;
        return enemyCard;
    }
}