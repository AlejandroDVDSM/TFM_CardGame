using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardRow : MonoBehaviour
    {
        [Min(0)]
        [SerializeField] private float extraSeparation = 160;
        
        public void PopulateRow(List<Card> cards)
        {
            Vector3 pos = Vector3.zero;
            
            for(int i = 0; i < cards.Count; i++)
            {
                cards[i].transform.SetParent(transform);
                pos.x = extraSeparation * i;
                cards[i].transform.localPosition = pos;
            }
        }
    }
}