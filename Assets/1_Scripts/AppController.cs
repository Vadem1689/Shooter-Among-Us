using UnityEngine;
using DG.Tweening;
using YG;

public class AppController : MonoBehaviour
{
    [SerializeField] GameController gameController;

    private void Awake()
    {
        gameController.Init();

        DOTween.Sequence().AppendInterval(0.5f).OnComplete(() =>
        {
            YandexGame.GameReadyAPI();
            Debug.Log("GameReadyAPI called");
        });
    }
}
