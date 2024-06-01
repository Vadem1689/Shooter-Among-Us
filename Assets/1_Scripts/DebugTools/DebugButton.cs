using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BRAmongUS.DebugTools
{
    [RequireComponent(typeof(Button))]
    public class DebugButton : MonoBehaviour, IPointerUpHandler, IPointerExitHandler
    {
        public event Action OnPointerUpAction = () => { };
        public event Action OnPointerExitAction = () => { };
        
        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpAction.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitAction.Invoke();
        }
    }
}