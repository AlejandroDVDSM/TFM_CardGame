using CardGame.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewItemCard", menuName = "Card / Item")]
    public class ItemCardData : BaseCardData
    {
        public EItemType Type;
        public bool IgnoreDataWhenRandom;
    }
}