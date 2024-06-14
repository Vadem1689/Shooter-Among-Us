using System.Collections;
using System.Collections.Generic;
using BRAmongUS.Utils;
using TMPro;
using UnityEngine;
using YG;

namespace BRAmongUS.Loot
{
    public sealed class AmmoBox : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite openedSprite;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private AmmoBoxSCRO loot;
        [SerializeField] private List<ConsumableItem> consumableItem;
        [SerializeField] private Transform consumableSpawnPoint;
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private float rechargeTime = 7;
        [SerializeField] private int consumableHintTime = 2;
        [SerializeField] private LocalizedText inZoneHintText;

        private WaitForSeconds rechargeWaitForSeconds;
        private WaitForSeconds consumableHintWaitForSeconds;
        private ConsumableItem consumableItemInstance;
        private GameWindow gameWindow;

        private bool isPlayerInTriggerZone;
        private bool isRussianLanguage;
        private bool isMobile;

        public bool IsClosed { get; private set; } = true;

        private void Start()
        {
            rechargeWaitForSeconds = new WaitForSeconds(rechargeTime);
            consumableHintWaitForSeconds = new WaitForSeconds(consumableHintTime);
            gameWindow = GameWindow.Instance;
            isMobile = YandexGame.EnvironmentData.isMobile;
            
            isRussianLanguage = YandexGame.savesData.language == "ru";
            hintText.SetText(inZoneHintText.GetText(isRussianLanguage));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constants.Tags.Player))
            {
                //OnPlayerInTriggerZone(true);
                Open();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constants.Tags.Player))
            {
                OnPlayerInTriggerZone(false);
            }
        }

        public GunTypeSCRO Open()
        {
            if (!IsClosed) return null;

            consumableItemInstance = Instantiate(consumableItem[Random.Range(0, consumableItem.Count)],
                consumableSpawnPoint.position, Quaternion.identity);
            SetBoxStatus(false);
            StartCoroutine(RechargeBox());
            
            if(isMobile && isPlayerInTriggerZone)
                gameWindow.ActivateInteractionButton(false);

            consumableItemInstance.Init(this);
            //ShowConsumableHint(consumableItemInstance.HintText);
            return loot.lootGun;
        }

        IEnumerator RechargeBox()
        {
            yield return rechargeWaitForSeconds;
            SetBoxStatus(true);
            if (isPlayerInTriggerZone)
            {
                if (isMobile && isPlayerInTriggerZone)
                    gameWindow.ActivateInteractionButton(true); 
                else
                    hintText.enabled = true;
            }
        }

        public void ShowConsumableHint(in string text)
        {
            StartCoroutine(ShowConsumableHintCoroutine(text));
        }

        IEnumerator ShowConsumableHintCoroutine(string text)
        {
            hintText.SetText(text);
            hintText.enabled = true;

            yield return consumableHintWaitForSeconds;

            hintText.SetText(inZoneHintText.GetText(isRussianLanguage));
            hintText.enabled = false;
        }

        private void SetBoxStatus(bool isClosed)
        {
            IsClosed = isClosed;
            spriteRenderer.sprite = isClosed ? closedSprite : openedSprite;
        }

        private void OnPlayerInTriggerZone(bool insideZone)
        {
            isPlayerInTriggerZone = insideZone;
            
            if (isMobile)
            {
                if(insideZone)
                {
                    gameWindow.ActivateInteractionButton(IsClosed);
                    gameWindow.ShowInteractionButton();
                }
                else
                {
                    gameWindow.HideInteractionButton();
                }
            }
            else 
            {
                if (IsClosed)
                    hintText.enabled = insideZone;
            }
        }
    }
}
