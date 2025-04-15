using CardGame.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewStatusCard", menuName = "Card / Status")]
    public class StatusCardData : BaseCardData
    {
        [Header("Status Card Data")]
        public EStatusType Status;
        public Sprite Icon;
    }
}