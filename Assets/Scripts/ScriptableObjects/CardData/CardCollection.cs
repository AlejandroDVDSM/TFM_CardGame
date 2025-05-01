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
        /// <returns>The data of a random card</returns>
        public BaseCardData GetRandomData()
        {
            List<BaseCardData> cardsData = Cards.Where(d => !d.IgnoreData).ToList();
            
            float total = 0f;
            for (int i = 0; i < cardsData.Count; i++)
            {
                total += cardsData[i].Weight;
            }
            
            float rand = Random.value;
            float cumulativeProbability = 0f;

            int count = cardsData.Count - 1;
            for (int i = 0; i < count; i++)
            {
                cumulativeProbability += cardsData[i].Weight / total;
                
                if (cumulativeProbability >= rand)
                {
                    return cardsData[i];
                }
            }
            
            return cardsData[count];
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
        /// Get a StatusCardData given its type
        /// </summary>
        /// <param name="statusType">Type of the status card to find</param>
        /// <returns>The data of the card matching the desired type</returns>
        public StatusCardData GetStatusDataByType(EStatusType statusType)
        {
            List<StatusCardData> statusCardsData = Cards.OfType<StatusCardData>().ToList();
            return statusCardsData.FirstOrDefault(d => d.Status == statusType);
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