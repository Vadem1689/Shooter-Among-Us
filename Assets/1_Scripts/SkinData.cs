using UnityEngine;

namespace BRAmongUS.Skins
{
    [System.Serializable]
    public struct SkinData
    {
        [field: SerializeField] 
        public RuntimeAnimatorController Animator { get; private set; }
        
        [field: SerializeField]
        public Sprite FaceSprite { get; private set; }
        
        [field: SerializeField] 
        public Sprite UiSprite { get; private set; }
        
        [field: SerializeField]
        public Sprite DeathSprite { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }
        
        [field: SerializeField]
        public int Price { get; private set; }
    }
}