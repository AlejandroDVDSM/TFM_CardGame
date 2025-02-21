using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBaseCard", menuName = "Card / Base Card")]
    public class BaseCardData : ScriptableObject
    {
        public string Name;
        
        public Sprite Sprite;
        
        public int MinValue;
        
        public int MaxValue;
        
        public bool IgnoreData;

    }
}