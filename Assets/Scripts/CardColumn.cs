using UnityEngine;

public class CardColumn : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;

    [Min(4)]
    [SerializeField] private int m_cardsPerColumn = 6;
    [SerializeField] private int m_cardSeparation;
    
    private void Start()
    {
        SpawnCards();
    }

    private void SpawnCards()
    {
        Vector3 cardPos = Vector3.zero;
        
        for (int i = 0; i < m_cardsPerColumn; i++)
        {
            // Instantiate a new card
            Card card = Instantiate(cardPrefab, transform);
            
            // Position new card with the given separation
            card.transform.SetLocalPositionAndRotation(cardPos, Quaternion.identity);
            cardPos.y += m_cardSeparation;
        }
    }
}
