using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using UnityEngine;

namespace CardGame
{
    public class CardRow : MonoBehaviour
    {
        [SerializeField] private ERow row;
        
        [Min(0)]
        [SerializeField] private float offsetX = 160;

        public void PopulateRow(List<Card> cards)
        {
            Vector3 pos = Vector3.zero;
            
            for(int i = 0; i < cards.Count; i++)
            {
                cards[i].transform.SetParent(transform);
                pos.x = offsetX * i;
                cards[i].transform.localPosition = pos;
                cards[i].SetLaneAndRow(i, row);
            }
        }

        public List<Card> GetCards()
        {
            return GetComponentsInChildren<Card>().ToList();
        }
    }
}