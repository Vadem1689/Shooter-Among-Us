using System.Collections;
using BRAmongUS.Audio;
using BRAmongUS.SRCO;
using BRAmongUS.Utils;
using DG.Tweening;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace BRAmongUS.Loot
{
    public enum EConsumableType
    {
        Heal,
        Shield,
    }
    
    public sealed class ConsumableItem : MonoBehaviour
    {
        [SerializeField] private EConsumableType consumableType;
        [SerializeField] private ConsumableSCRO consumableData;
        [SerializeField] private float pickupDistance = 1.1f;
        [SerializeField] private float moveSpeed = 30f;
        [SerializeField] private float scaleDuration = .2f;
        
        private Transform cachedTransform;
        private Player player;
        private int healPercent;
        
        private Tween scaleTween;
        
        public string HintText { get; private set; }
        
        public void Init(in AmmoBox ammoBox)
        {
            cachedTransform = transform;
            bool isRussianLanguage = YandexGame.savesData.language == "ru";
            
            if(consumableType == EConsumableType.Heal)
                healPercent = Random.Range(consumableData.MinHealPercent, consumableData.MaxHealPercent);
            
            HintText = consumableType switch
            {
                EConsumableType.Heal => $"{consumableData.HealHintText.GetText(isRussianLanguage)} {healPercent}% <br> {Constants.IconsTags.Heal}",
                EConsumableType.Shield => $"{consumableData.ShieldHintText.GetText(isRussianLanguage)} {consumableData.ShieldDuration} {consumableData.SecondsText.GetText(isRussianLanguage)} <br> {Constants.IconsTags.Shield}",
                _ => HintText
            };
            
            ammoBox.ShowConsumableHint(HintText);

            scaleTween = cachedTransform.DOScale(Vector3.one, scaleDuration);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (player != null) return;
            if (other.TryGetComponent(out player)) 
                StartCoroutine(MoveToPlayer(player.transform));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player tempPlayer) && tempPlayer == player)
            {
                StopAllCoroutines();
                player = null;
            }
        }

        private IEnumerator MoveToPlayer(Transform transformToMove)
        {
            while (true)
            {
                if (player == null) yield break;
                cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, transformToMove.position, moveSpeed * Time.deltaTime);
                
                if (Vector2.Distance(cachedTransform.position, player.transform.position) < pickupDistance)
                {
                    switch (consumableType)
                    {
                        case EConsumableType.Heal:
                            player.Heal(player.MaxHealth.FromPercent(healPercent));
                            break;
                        case EConsumableType.Shield:
                            player.ActivateShield(consumableData.ShieldDuration);
                            break;
                    }

                    if (player.IsRealPlayer)
                        AudioController.Instance.PlaySound(consumableType == EConsumableType.Heal ? ESoundType.PickupHeal : ESoundType.PickupShield);
                    
                    scaleTween.Kill();
                    Destroy(gameObject);
                }
                
                yield return null;
            }
        }
    }
}