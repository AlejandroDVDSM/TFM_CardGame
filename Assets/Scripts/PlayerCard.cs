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
    
    private ECardLane _currentLane = ECardLane.Middle;

    private int _currentHealth;
    
    private void Start()
    {
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
    
    public void PlaceInPosition(ECardLane cardLane)
    {
        // Checks if the player is currently in one side and is trying to select a card from the other side
        // If it is, he can't move there

        // TODO: add tween to give player the feedback that it can't move to the other side
        if (!CanMoveTo(cardLane))
            return;
        
        Vector3 newPos = Vector3.zero;
        
        switch (cardLane)
        {
            case ECardLane.Left:
                newPos = new Vector3(-lateralMovement, posY, 0);
                break;
            
            case ECardLane.Middle:
                newPos = new Vector3(0, posY, 0);
                break;
            
            case ECardLane.Right:
                newPos = new Vector3(lateralMovement, posY, 0);
                break;
        }

        // Update position and current lane
        transform.localPosition = newPos;
        _currentLane = cardLane;
        
        // 
        // GameManager.Instance.CommitTurn();
    }

    public bool CanMoveTo(ECardLane cardLane)
    {
        if (Mathf.Abs(_currentLane - cardLane) > 1)
        {
            Debug.LogWarning($"[Player] Player can not move from {_currentLane} to {cardLane}");
            return false;
        }

        return true;
    }
}
