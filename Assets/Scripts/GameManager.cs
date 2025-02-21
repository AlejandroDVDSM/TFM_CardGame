using System.Collections.Generic;
using CardGame;
using CardGame.Enums;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Player Player => m_player;
    
    [Header("Game settings")]
    [Min(1)]
    [SerializeField] private int m_turns;
    
    [SerializeField] private Player m_player;
    [SerializeField] private CardPool m_cardPool;
    
    [Header("Rows")]
    [SerializeField] private CardRow topRow;
    [SerializeField] private CardRow middleRow;
    [SerializeField] private CardRow bottomRow;
    
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
        topRow.PopulateRow(m_cardPool.ExtractRangeFromPool());
        
        // Populate middle row
        middleRow.PopulateRow(m_cardPool.ExtractRangeFromPool());
        
        // Populate bottom row
        bottomRow.PopulateRow(m_cardPool.ExtractRangeFromPool());
    }

    /// <summary>
    /// Make all entities perform their actions in the correct sequence
    /// </summary>
    /// <param name="card">The card that has been selected</param>
    public void PlayTurn(Card card)
    {
        // Check if the player can move to the selected card
        if (!Player.CanMoveTo(card.Lane))
            return;
        
        // Moves the player to the lane of the selected card
        Player.PlaceInPosition(card.Lane);
        
        // Apply card effect
        card.PerformAction();

        // If the card is an enemy, turn it into coins
        if (card is EnemyCard)
        {
            Debug.Log("Enemy is now a Coin");
            ItemCard coinCard = m_cardPool.ExtractItemCardOfType(EItemType.Coin);
            coinCard.UpdateValue(card.Value);
            
            bottomRow.PlaceSingleCard(coinCard, card.Lane);
            m_cardPool.DestroyCard(card);
        }
        else
        {
            CommitTurn();
        }
    }

    /// <summary>
    /// Moves cards and make new ones appear in the top row
    /// </summary>
    private void CommitTurn()
    {
        List<Card> topRowCards = topRow.GetCards();
        List<Card> middleRowCards = middleRow.GetCards();
        List<Card> bottomRowCards = bottomRow.GetCards();
        
        // Send the cards that were in the bottom row back to the pool
        m_cardPool.DestroyCards(bottomRowCards);
        
        // Generates new cards if there are still rounds to be played
        if (m_turns > 0)
        {
            // New cards from the pool will go the top row
            topRow.PopulateRow(m_cardPool.ExtractRangeFromPool());
            m_turns--;
        } else if (m_turns == 0)
        { // Generates cups cards
            topRow.PopulateRow(m_cardPool.ExtractCupsFromPool());
            m_turns--;
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
