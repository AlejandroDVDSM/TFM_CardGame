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
        public BaseCardData GetRandomData()
        {
            List<BaseCardData> cardsData = Cards.Where(d => !d.IgnoreData).ToList();
            return cardsData[Random.Range(0, cardsData.Count)];
        }
        
        /// <summary>
        /// Get an ItemCardData given its type
        /// </summary>
        /// <param name="itemType">Type of the item card to find</param>
        /// <returns>The data of the card matching the desired type</returns>
        public ItemCardData GetItemDataByType(EItemType itemType)
        {
            List<ItemCardData> itemCardsData = Cards.OfType<ItemCardData>().ToList();
            return itemCardsData.FirstOrDefault(d => d.Type == itemType);
        }
        
        /// <summary>
        /// Get the data of a card given an index
        /// </summary>
        /// <param name="index">Index of the data card to find</param>
        /// <returns>The data of the card in the requested index</returns>
        private BaseCardData GetDataByIndex(int index) {
            return Cards[index];
        }
    }
}