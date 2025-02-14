using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyCard", menuName = "Card / Enemy")]
    public class CardData : ScriptableObject
    {
        public string name;
        public Sprite cardImage;
    }
}