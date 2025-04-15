using CardGame.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewItemCard", menuName = "Card / Item")]
    public class ItemCardData : BaseCardData
    {
        [Header("Item Card Data")]
        public EItemType Type;

        public int Price;
    }
}