using UnityEngine;

namespace ScriptableObjects
{
    // [CreateAssetMenu(fileName = "BaseCard", menuName = "Card / BaseCard")]
    public class BaseCardData : ScriptableObject
    {
        public string Name;
        
        public Sprite Sprite;
        
        public int MinValue;
        
        public int MaxValue;
    }
}