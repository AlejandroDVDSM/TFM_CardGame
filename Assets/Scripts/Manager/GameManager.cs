using System.Collections.Generic;
using System.Linq;
using CardGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player Player => m_player;
    public CardPool CardPool => m_cardPool;
    public CardRow TopRow => topRow;
    public CardRow MiddleRow => middleRow;
    public CardRow BottomRow => bottomRow;
    
    [Header("Game settings")]
    [Min(1)]
    [SerializeField] private int m_turns;
    [SerializeField] private CardPool m_cardPool;
    
    [Header("Rows")]
    [SerializeField] private CardRow topRow;
    [SerializeField] private CardRow middleRow;
    [SerializeField] private CardRow bottomRow;

    [Header("Player Prefabs")]
    [SerializeField] private Transform m_playerSpawn;
    [SerializeField] private Player[] m_playerPrefabs;
    [Space(15)]
    
    public UnityEvent OnTurnCommited;

    private Player m_player;

    private void Awake()
    {
        // Singleton
        if (!Instance)
            Instance = this;
    }

    private void Start()
    {
        LoadPlayer();
        InitGame();
        
    }

    private void LoadPlayer()
    {
        if (!PlayerPrefs.HasKey("Character"))
        {
            Debug.LogError("The player could not be loaded as the key <Character> does not exist in the PlayerPrefs");
            return;
        }

        Player characterPrefab = m_playerPrefabs.FirstOrDefault(p => p.CharacterName == PlayerPrefs.GetString("Character"));

        if (!characterPrefab)
        {
            Debug.LogError($"Could not find any player prefab with the name {PlayerPrefs.GetString("Character")}");
            return;
        }
        m_player = Instantiate(characterPrefab, m_playerSpawn);
    }

    /// <summary>
    /// Initialize game
    /// </summary>
    private void InitGame()
    {
        AudioManager.Instance.Play("GameMusic");
        
        if (PlayerPrefs.GetInt("TutorialCompleted") == 1)
        {
            topRow.PopulateRow();
        
            middleRow.PopulateRow();
        
            bottomRow.PopulateRow();
        }
        else
        {
            m_turns = TutorialManager.Instance.TutorialTurns;
            TutorialManager.Instance.StartTutorial();    
        }
    }

    /// <summary>
    /// Make all entities perform their actions in the correct sequence
    /// </summary>
    /// <param name="card">The card that has been selected</param>
    public void PlayTurn(Card card)
    {
        // Check if the player can move to the selected card
        if (!Player.Movement.CanMoveTo(card.Lane))
            return;
        
        // Moves the player to the lane of the selected card
        Player.Movement.PlaceInPosition(card.Lane);
        
        // Apply card effect
        card.PerformAction();

        // We do not commit the turn when the card is an enemy as it has to turn into a coin first.
        // Then, the player must choose between selecting that coin or one of the other two cards in the row
        // if (card is not EnemyCard)
        //     CommitTurn();
    }

    /// <summary>
    /// Move the cards and make new ones appear in the top row
    /// </summary>
    public void CommitTurn()
    {
        // if (TutorialManager.Instance.IsRunning)
        // {
        //     TutorialManager.Instance.CommitTutorialTurn();
        //     OnTurnCommited?.Invoke();
        //     return;
        // }
        
        List<Card> topRowCards = topRow.GetCards();
        List<Card> middleRowCards = middleRow.GetCards();
        List<Card> bottomRowCards = bottomRow.GetCards();

        // Check if the bottom cards are CupCard. If it is, we cannot commit the turn 
        if (bottomRowCards[0].GetType() == typeof(CupCard))
        {
            return;
        }
        
        // Send the cards that were in the bottom row back to the pool
        m_cardPool.DestroyCards(bottomRowCards);
        
        // Generates new cards if there are still rounds to be played
        if (m_turns > 0)
        {
            // New cards from the pool will go the top row
            topRow.PopulateRow();
            m_turns--;
        } else if (m_turns == 0)
        { // Generates cups cards
            topRow.PopulateRow(m_cardPool.ExtractCupsFromPool());
            m_turns--;
        }
        
        // The cards in the top row will go to the middle row now
        middleRow.PopulateRow(topRowCards);
        
        // The cards in the middle row will go to the bottom row now
        bottomRow.PopulateRow(middleRowCards);

        OnTurnCommited?.Invoke();
    }

    public void EndGame(bool win)
    {
        // TODO: Show a special screen if the player won (?)
        if (win)
        {
            AudioManager.Instance.Play("Win");
            Debug.Log("YOU WIN");
        }
        
        SceneManager.LoadScene("MainMenu");
    }
}
