using System;
using System.Collections.Generic;
using CardGame;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private CardPool _cardPool;

    public UnityEvent<Card> OnCardSelected;
    
    [Header("Rows")]
    [SerializeField] private CardRow topRow;
    [SerializeField] private CardRow middleRow;
    [SerializeField] private CardRow bottomRow;
    
    
    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void Start()
    {
        InitGame();
    }

    /// <summary>
    /// Initialize game
    /// </summary>
    private void InitGame()
    {
        // Initialize cards pool
        _cardPool.InitPool();
        
        // Populate top row
        topRow.PopulateRow(_cardPool.ExtractRangeFromPool());
        
        // Populate middle row
        middleRow.PopulateRow(_cardPool.ExtractRangeFromPool());
        
        // Populate bottom row
        bottomRow.PopulateRow(_cardPool.ExtractRangeFromPool());
    }

    
    public void SelectCard(Card card)
    {
        OnCardSelected?.Invoke(card);

        List<Card> topRowCards = topRow.GetCards();
        List<Card> middleRowCards = middleRow.GetCards();
        List<Card> bottomRowCards = bottomRow.GetCards();
        
        // New cards from the pool will go the top row
        topRow.PopulateRow(_cardPool.ExtractRangeFromPool());
        
        // The cards that were in the top row will go to the middle row now
        middleRow.PopulateRow(topRowCards);
        
        // Send the cards that were in the bottom row back to the pool
        _cardPool.DestroyCards(bottomRowCards);
        
        // The cards that were in the middle row will go to the bottom row now
        bottomRow.PopulateRow(middleRowCards);
        
        
        
    }
}
