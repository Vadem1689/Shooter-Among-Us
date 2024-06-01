using BRAmongUS;
using BRAmongUS.Audio;
using BRAmongUS.Coins;
using BRAmongUS.Loot;
using BRAmongUS.Utils;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private AmmoBox ammoBoxInReach;
    private bool inTriggerZone;
    
    private GameController gameController;
    private AudioController audioController;

    private void Start()
    {
        gameController = GameController.Instance;
        audioController = AudioController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.Tags.AmmoBox))
        {
            ammoBoxInReach = collision.GetComponent<AmmoBox>();
            inTriggerZone = true;
        }
        
        if (collision.CompareTag(Constants.Tags.Coin))
        {
            collision.GetComponent<Coin>().Pick();
            if(player.IsRealPlayer)
            {
                gameController.AddCoinsCount(1);
                audioController.PlaySound(ESoundType.PickupCoin);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.Tags.AmmoBox))
        {
            inTriggerZone = false;
        }
    }

    public bool CanOpenAmmoBox()
    {
        return inTriggerZone && ammoBoxInReach.IsClosed;
    }

    public GunTypeSCRO OpenBoxInReach()
    {
        return ammoBoxInReach.Open();
    }
}
