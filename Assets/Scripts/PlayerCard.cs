using CardGame.Enums;
using TMPro;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    [Min(1)]
    [SerializeField] private int maxHealth;
    [SerializeField] private TMP_Text healthText;
    
    [SerializeField] private float lateralMovement;
    [SerializeField] private float posY;
    
    private ECardLane _currentLane = ECardLane.MIDDLE;

    private int _currentHealth;
    
    private void Start()
    {
        GameManager.Instance.OnCardSelected.AddListener(OnCardSelected);
        _currentHealth = maxHealth;
        healthText.text = _currentHealth.ToString();
    }

    public void Hit(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _currentHealth);
        
        if (_currentHealth == 0)
        {
            // TODO: end game
            // TODO: add tween
            Debug.Log("DEAD");
        }
        
        healthText.text = _currentHealth.ToString();
    }
    
    private void OnCardSelected(Card selectedCard)
    {
        PlacePlayerInPosition(selectedCard.CardLane);
    }
    
    private void PlacePlayerInPosition(ECardLane cardLane)
    {
        // Checks if the player is currently in one side and is trying to select a card from the other side
        // If it is, he can't move there
        if (Mathf.Abs(_currentLane - cardLane) > 1)
        {
            // TODO: add tween to give player the feedback that it can't move to the other side
            Debug.LogWarning($"[Player] Player can not move from {_currentLane} to {cardLane}");
            return;
        }
        
        Vector3 newPos = Vector3.zero;
        
        switch (cardLane)
        {
            case ECardLane.LEFT:
                newPos = new Vector3(-lateralMovement, posY, 0);
                break;
            
            case ECardLane.MIDDLE:
                newPos = new Vector3(0, posY, 0);
                break;
            
            case ECardLane.RIGHT:
                newPos = new Vector3(lateralMovement, posY, 0);
                break;
        }

        // Update position and current lane
        transform.localPosition = newPos;
        _currentLane = cardLane;
        
        // 
        GameManager.Instance.CommitTurn();
    }
}
