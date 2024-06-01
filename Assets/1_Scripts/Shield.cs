using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace BRAmongUS.Loot
{
    public sealed class Shield : MonoBehaviour
    {
        [SerializeField] private float defaultScale = 2f;
        [SerializeField] private float activationDuration = .5f;
        
        private Transform cachedTransform;
        private float currentDuration;
        
        public bool IsActive { get; private set; }
        
        private void Awake()
        {
            cachedTransform = transform;
        }
        
        public void ActivateShield(in float duration)
        {
            if (IsActive)
            {
                currentDuration += duration; 
            }
            else
            {
                currentDuration = duration;
                gameObject.SetActive(true);
                StartCoroutine(ShieldCoroutine());
            }
        }

        private IEnumerator ShieldCoroutine()
        {
            IsActive = true;
            cachedTransform.DOScale(defaultScale, activationDuration);

            while (currentDuration > 0)
            {
                currentDuration -= Time.deltaTime;
                yield return null;
            }
            
            cachedTransform.DOScale(0, activationDuration);
            yield return new WaitForSeconds(activationDuration);
            
            currentDuration = 0;
            IsActive = false;
            gameObject.SetActive(false);
        }
    }
}