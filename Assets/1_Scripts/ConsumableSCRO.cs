using BRAmongUS.Utils;
using UnityEngine;

namespace BRAmongUS.SRCO
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Consumable", order = 1)]
    public sealed class ConsumableSCRO : ScriptableObject
    {
        [field: SerializeField, Range(0, 100)]
        public int MinHealPercent { get; private set; }
        
        [field: SerializeField, Range(0, 100)]
        public int MaxHealPercent { get; private set; }
        
        [field: SerializeField, Space(10)]
        public int ShieldDuration { get; private set; }
        
        [field: SerializeField, Space(10)]
        public LocalizedText HealHintText { get; private set; }
        
        [field: SerializeField]
        public LocalizedText ShieldHintText { get; private set; }
        
        [field: SerializeField]
        public LocalizedText SecondsText { get; private set; }
    }
}