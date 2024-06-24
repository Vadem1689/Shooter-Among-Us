using System;
using BRAmongUS.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BRAmongUS.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class UiButton : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] Button button;
        [SerializeField] private bool playSoundOnClick = true;
        
        private AudioController audioController;
        
        private void Awake()
        {
            audioController = AudioController.Instance;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && button.interactable) 
                OnClick();
        }
        
        private void OnClick()
        {
            if (playSoundOnClick)
            {
                print("Áûë");
                audioController.PlaySound(ESoundType.ButtonClick);
            }
        }
    }
}