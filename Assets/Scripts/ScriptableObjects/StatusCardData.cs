using CardGame.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewStatusCard", menuName = "Card / Status")]
    public class StatusCardData : BaseCardData
    {
        public EStatusType Status;
    }
}