using System.Collections.Generic;
using BRAmongUS.Skins;
using UnityEngine;

namespace BRAmongUS.SRCO
{
    [CreateAssetMenu(fileName = "SkinsContainer", menuName = "ScriptableObjects/SkinsContainer", order = 1)]
    public sealed class SkinsContainerSCRO : ScriptableObject
    {
        [field: SerializeField] 
        public List<SkinData> Skins { get; private set; }
        
        [field: SerializeField]
        public int BotSkinIndex { get; private set; }
    }
}