using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCollection", menuName = "Card / Collection")]
    public class CardCollection : ScriptableObject
    {
        public List<BaseCardData> Cards;
        
        public BaseCardData GetRandomCard() {
            return GetCardByIndex(Random.Range(0, Cards.Count));
        }
        
        private BaseCardData GetCardByIndex(int index) {
            return Cards[index];
        }
    }
}