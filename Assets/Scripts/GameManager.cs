using System;
using System.Collections.Generic;
using CardGame;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CardPool cardPool;
    
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
        StartGame();
    }

    private void StartGame()
    {
        cardPool.InitPool();
        
        // Populate top row
        List<Card> cards = cardPool.ExtractRangeFromPool();
        topRow.PopulateRow(cards);
        
        // Populate middle row
        cards = cardPool.ExtractRangeFromPool();
        middleRow.PopulateRow(cards);
        
        // Populate bottom row
        cards = cardPool.ExtractRangeFromPool();
        bottomRow.PopulateRow(cards);
        
        
        
    }
    
    
    
}
