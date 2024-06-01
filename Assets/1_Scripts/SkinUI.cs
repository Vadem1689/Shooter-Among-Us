using UnityEngine;
using UnityEngine.UI;

namespace BRAmongUS.Skins
{
    public sealed class SkinUI : MonoBehaviour
    {
        [SerializeField] private Image skinImage;
        [SerializeField] private Image faceImage;
        
        private SkinData skinData;
        
        public void Initialize(SkinData tempSkinData)
        {
            skinData = tempSkinData;
            skinImage.sprite = skinData.UiSprite;
            faceImage.sprite = skinData.FaceSprite;
        }
    }
}