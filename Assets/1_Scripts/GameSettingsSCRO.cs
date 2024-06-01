using UnityEngine;

namespace BRAmongUS.SRCO
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public sealed class GameSettingsSCRO : ScriptableObject
    {
        [field: SerializeField]
        public int GameTimeInSeconds { get; private set; }
        
        [field: SerializeField]
        public int CoinsMultiplierOnShowedAd { get; private set; }
        
        [field: SerializeField]
        public int CoinsForShowedAd { get; private set; }
        
        [field: SerializeField]
        public int CoinsForKill { get; private set; }
        
        [field: SerializeField]
        public int PlayerHealth { get; private set; }
        
        [field: SerializeField]
        public float PlayerWeaponDamageMultiplier { get; private set; }
        
        [field: SerializeField]
        public float BotSpeed { get; private set; }
    }
}