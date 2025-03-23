using CardGame.Enums;
using UnityEngine;

namespace CardGame.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Position")]
        [SerializeField] private float m_lateralMovement;
        [SerializeField] private float m_posY;

        public ECardLane CurrentLane => m_currentLane;
        private ECardLane m_currentLane = ECardLane.Middle;
    
        /// <summary>
        /// Place the player in the desired lane
        /// </summary>
        /// <param name="cardLane">The desired lane</param>
        public void PlaceInPosition(ECardLane cardLane)
        {
            // Checks if the player is currently in one side and is trying to select a card from the other side
            // If it is, he can't move there
            if (!CanMoveTo(cardLane))
                return;
            
            Vector3 newPos = Vector3.zero;
            
            switch (cardLane)
            {
                case ECardLane.Left:
                    newPos = new Vector3(-m_lateralMovement, m_posY, 0);
                    break;
                
                case ECardLane.Middle:
                    newPos = new Vector3(0, m_posY, 0);
                    break;
                
                case ECardLane.Right:
                    newPos = new Vector3(m_lateralMovement, m_posY, 0);
                    break;
            }
    
            // Update position and current lane
            transform.localPosition = newPos;
            m_currentLane = cardLane;
        }
    
        /// <summary>
        /// Check if the player can move to the desired lane
        /// </summary>
        /// <param name="cardLane">The desired lane to check</param>
        /// <returns>Return true if the player can move to the desired lane. Return false if not</returns>
        public bool CanMoveTo(ECardLane cardLane)
        {
            if (Mathf.Abs(m_currentLane - cardLane) > 1)
            {
                Debug.LogWarning($"[Player] Player can not move from {m_currentLane} to {cardLane}");
                return false;
            }
    
            return true;
        }
    }
}