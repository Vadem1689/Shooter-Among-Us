using UnityEngine;

namespace BRAmongUS.Utils
{
    [System.Serializable]
    public struct LocalizedText
    {
        [field: SerializeField]
        public string Ru { get; private set; }
        
        [field: SerializeField]
        public string En { get; private set; }
        
        public string GetText(in bool isRu)
        {
            return isRu ? Ru : En;
        }
    }
}