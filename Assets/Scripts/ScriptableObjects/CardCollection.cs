using System.Collections.Generic;
using System.Linq;
using CardGame.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCollection", menuName = "Card / Collection")]
    public class CardCollection : ScriptableObject
    {
        public List<BaseCardData> Cards;
        
        /// <summary>
        /// Get a random card
        /// </summary>
        /// <returns>A random card</returns>
        public BaseCardData GetRandomCard() {
            return GetCardByIndex(Random.Range(0, Cards.Count));
        }
        
        /// <summary>
        /// Get an ItemCard given its type
        /// </summary>
        /// <param name="itemType">Type of the item card to find</param>
        /// <returns>A card matching the desired type</returns>
        public ItemCardData GetItemCardByType(EItemType itemType)
        {
            // Return null if the card's type is not ItemCardData
            if (Cards.First().GetType() != typeof(ItemCardData)) 
                return null;
            
            List<ItemCardData> itemCards = Cards.OfType<ItemCardData>().ToList();
            return itemCards.First(c => c.Type == itemType);
        }
        
        /// <summary>
        /// Get a card given an index
        /// </summary>
        /// <param name="index">Index of the card to find</param>
        /// <returns>A card in the requested index</returns>
        private BaseCardData GetCardByIndex(int index) {
            return Cards[index];
        }
    }
}