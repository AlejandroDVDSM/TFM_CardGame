using UnityEngine;

namespace CardGame.Player
{
    public class PlayerStatus : MonoBehaviour
    {
        public int IsPoisoned => m_isPoisoned;
        
        private int m_isPoisoned;
        private int m_isBlind;

        private Player m_player;

        private void Start()
        {
            m_player = GetComponent<Player>();
            GameManager.Instance.OnTurnCommited.AddListener(ApplyStatuses);
        }

        // TODO: make own script for player statuses?
        private void ApplyStatuses()
        {
            Debug.Log("[PLAYER] Applying statuses...");
            
            if (m_isPoisoned > 0)
            {
                ApplyPoison(m_isPoisoned - 1);
                m_player.Hit(1);
            }

            if (m_isBlind > 0)
            {
                ApplyBlind(m_isBlind - 1);
                // TODO: anything else?
            }
        }
        
        public void ApplyPoison(int turns)
        {
            Debug.Log($"[PLAYER] Applying poison ({turns} turns left)");
            m_isPoisoned = turns;
        }

        public void ApplyBlind(int turns)
        {
            Debug.Log($"PLAYER] Applying blind ({turns} turns left)");
            m_isBlind = turns;
            
            // TODO
        }
    }
}