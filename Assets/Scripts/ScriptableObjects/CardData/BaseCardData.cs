using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBaseCard", menuName = "Card / Base Card Data")]
    public class BaseCardData : ScriptableObject
    {
        public string Name;
        
        public Sprite Sprite;
        
        public Color BackgroundColor = Color.white;
        
        public int MinValue;
        
        public int MaxValue;
        
        public bool IgnoreData;

    }
}