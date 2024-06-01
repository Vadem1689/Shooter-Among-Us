using BRAmongUS.Audio;
using BRAmongUS.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

namespace BRAmongUS.UI
{
    public sealed class MuteAudioButton : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite mutedSprite;
        [SerializeField] private Sprite unmutedSprite;
        
        private bool IsMuted => YandexGame.savesData.isAudioMuted;
        
        private void Start()
        {
            ChangeSprite(YandexGame.savesData.isAudioMuted);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                AudioController.Instance.MuteAudio(!IsMuted);
                ChangeSprite(IsMuted);
            }
        }
        
        private void ChangeSprite(bool isMuted)
        {
            image.sprite = isMuted ? mutedSprite : unmutedSprite;
        }
    }
}