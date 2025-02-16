using System.Collections.Generic;
using CardGame;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public PlayerCard Player => _player;
    
    [Header("Game settings")]
    [SerializeField] private CardPool cardPool;
    
    [Min(1)]
    [SerializeField] private int turns;
    
    [Header("Rows")]
    [SerializeField] private CardRow topRow;
    [SerializeField] private CardRow middleRow;
    [SerializeField] private CardRow bottomRow;

    [Header("Events")]
    public UnityEvent<Card> OnCardSelected;

    [SerializeField] private PlayerCard _player;

    private void Awake()
    {
        // Singleton
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
        // Populate top row
        topRow.PopulateRow(cardPool.ExtractRangeFromPool());
        
        // Populate middle row
        middleRow.PopulateRow(cardPool.ExtractRangeFromPool());
        
        // Populate bottom row
        bottomRow.PopulateRow(cardPool.ExtractRangeFromPool());
    }

    /// <summary>
    /// Invoke all registered callbacks when a card has been selected
    /// </summary>
    /// <param name="card">The card that has been selected</param>
    public void SelectCard(Card card)
    {
        OnCardSelected?.Invoke(card);
    }

    /// <summary>
    /// Moves cards and make new ones appear in the top row
    /// </summary>
    public void CommitTurn()
    {
        List<Card> topRowCards = topRow.GetCards();
        List<Card> middleRowCards = middleRow.GetCards();
        List<Card> bottomRowCards = bottomRow.GetCards();
        
        // Send the cards that were in the bottom row back to the pool
        cardPool.DestroyCards(bottomRowCards);
        
        // Generates new cards if there are still rounds to be played
        if (turns > 0)
        {
            // New cards from the pool will go the top row
            topRow.PopulateRow(cardPool.ExtractRangeFromPool());
            turns--;
        } else if (turns == 0)
        { // Generates cups cards
            topRow.PopulateRow(cardPool.ExtractCupsFromPool());
            turns--;
        }
            
        
        // The cards that were in the top row will go to the middle row now
        middleRow.PopulateRow(topRowCards);
        
        
        // The cards that were in the middle row will go to the bottom row now
        bottomRow.PopulateRow(middleRowCards);

    }

    public void EndGame()
    {
        Debug.Log("YOU WIN");
    }
}
